namespace GudKoodi.DeeperSkeeper.Network
{
    using System.Collections.Generic;
    using DarkRift;
    using DarkRift.Server;
    using DarkRift.Server.Unity;
    using Event;
    using UnityEngine;

    /// <summary>
    /// Component that handles all communication between the server and all clients.
    /// </summary>
    public class ServerController : MonoBehaviour
    {
        [Tooltip("The server component this script will communicate with")]
        public XmlUnityServer Server;

        [Tooltip("Instantiator used to create objects")]
        public NetworkInstantiator NetworkInstantiator;

        /// <summary>
        /// Level generation request event.
        /// </summary>
        [Tooltip("Level generation request event")]
        public LevelGenerationRequested LevelGenerationRequested;

        public NetworkConfig NetworkConfig;

        /// <summary>
        /// Network Events container.
        /// </summary>
        [Tooltip("Network Events container.")]
        public NetworkEvents NetworkEvents;

        private PlayerManager players;

        private EnemyManager enemies;

        private readonly NetworkIdPool playerIDPool = new NetworkIdPool();

        private readonly NetworkIdPool enemyIDPool = new NetworkIdPool();

        private readonly Dictionary<ushort, ushort> clientToPlayerObject = new Dictionary<ushort, ushort>();

        /// <summary>
        /// Level seed for the game.
        /// </summary>
        private int levelSeed = -1;

        public void Initialize()
        {
            if (Server == null)
            {
                Debug.LogError("Server component missing.");
                return;
            }

            this.levelSeed = new System.Random().Next();
            this.LevelGenerationRequested.Trigger(levelSeed);

            ushort id = playerIDPool.Next();
            Player player = new Player(id, Vector3.zero, 0);
            players.Create(this.NetworkInstantiator.PlayerPrefab, player, true);

            DarkRiftServer server = Server.Server;
            server.ClientManager.ClientConnected += OnClientConnect;
            server.ClientManager.ClientDisconnected += OnClientDisconnect;
        }

        public void SendObject(GameObject gameObject)
        {
            // TODO: Distinguish between different network objects
            using (Message message = players.UpdateAndSerialize(gameObject, ServerMessage.UpdatePlayer))
            {
                foreach (var client in Server.Server.ClientManager.GetAllClients())
                {
                    client.SendMessage(message, SendMode.Unreliable);
                }
            }
        }

        void Awake()
        {
            if (!this.NetworkConfig.isHost)
            {
                Debug.Log("Server manager shutting up.");
                gameObject.SetActive(false);
            }
            this.players = new PlayerManager(this.NetworkInstantiator.MasterPlayerCreated, this.NetworkInstantiator.PlayerUpdateRequested);
            this.enemies = new EnemyManager();
            this.NetworkEvents.EnemyCreationRequested.Subscribe(EnemyCreationRequested);
        }

        void OnDestroy()
        {
            DarkRiftServer server = Server.Server;
            if (server != null)
            {
                server.ClientManager.ClientConnected -= OnClientConnect;
                server.ClientManager.ClientDisconnected -= OnClientDisconnect;
            }
        }

        private void OnClientConnect(object sender, ClientConnectedEventArgs e)
        {
            e.Client.MessageReceived += OnMessageReceived;
            ushort id = playerIDPool.Next();
            Player player = new Player(id, Vector3.zero, 0);
            players.Create(this.NetworkInstantiator.PlayerPrefab, player);

            ConnectionData data = new ConnectionData(e.Client.ID, enemies.ToArray(), id, levelSeed, players.ToArray());
            clientToPlayerObject[data.ClientID] = data.PlayerObjectID;
            using (Message message = Message.Create(ServerMessage.ConnectionData, data))
            {
                e.Client.SendMessage(message, SendMode.Reliable);
            }

            using (Message broadcast = Message.Create(ServerMessage.CreatePlayer, player))
            {
                foreach (var client in Server.Server.ClientManager.GetAllClients())
                {
                    if (client.ID != e.Client.ID)
                    {
                        client.SendMessage(broadcast, SendMode.Reliable);
                    }
                }
            }
        }

        private void OnClientDisconnect(object sender, ClientDisconnectedEventArgs e)
        {
            e.Client.MessageReceived -= OnMessageReceived;
            ushort playerId = clientToPlayerObject[e.Client.ID];
            players.Destroy(playerId);
            playerIDPool.Release(playerId);
            Debug.Log($"Sending destruct message for player ID {playerId}");
            using (DarkRiftWriter writer = DarkRiftWriter.Create())
            {
                writer.Write(playerId);
                using (Message message = Message.Create(ServerMessage.DeletePlayer, writer))
                {
                    foreach (var client in Server.Server.ClientManager.GetAllClients())
                    {
                        if (client.ID != e.Client.ID)
                        {
                            client.SendMessage(message, SendMode.Reliable);
                        }
                    }
                }
            }
        }

        private void OnMessageReceived(object sender, MessageReceivedEventArgs e)
        {
            switch (e.Tag)
            {
                case ClientMessage.UpdatePlayer:
                    UpdatePlayer(e);
                    break;
            }
        }

        private void UpdatePlayer(MessageReceivedEventArgs e)
        {
            using (Message message = e.GetMessage())
            {
                players.DeserializeAndUpdate(message);
                foreach (var client in Server.Server.ClientManager.GetAllClients())
                {
                    if (client.ID != e.Client.ID)
                    {
                        client.SendMessage(message, SendMode.Reliable);
                    }
                }
            }
        }

        private void EnemyCreationRequested(GameObject prefab, Vector3 position, object p2, object p3)
        {
            Debug.Log($"Requested creation of {prefab} in {position}");
            Enemy enemy = new Enemy(enemyIDPool.Next(), position, position);
            enemies.Create(prefab, enemy, true);
        }
    }
}
