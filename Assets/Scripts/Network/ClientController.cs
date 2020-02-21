namespace GudKoodi.DeeperSkeeper.Network
{
    using DarkRift;
    using DarkRift.Client;
    using DarkRift.Client.Unity;
    using Event;
    using UnityEngine;

    using EnemyList = GudKoodi.DeeperSkeeper.Enemy.EnemyList;

    /// <summary>
    /// Component that handles all communication to server.
    /// </summary>
    public class ClientController : MonoBehaviour
    {
        [Tooltip("The server component this script will communicate with.")]
        public UnityClient Client;

        [Tooltip("Instantiator used to create objects")]
        public NetworkInstantiator NetworkInstantiator;

        [Tooltip("Network configuration to read check host status from.")]
        public NetworkConfig NetworkConfig;

        /// <summary>
        /// Level generation request event.
        /// </summary>
        [Tooltip("Level generation request event")]
        public LevelGenerationRequested LevelGenerationRequested;

        /// <summary>
        /// Network Events container.
        /// </summary>
        [Tooltip("Network Events container.")]
        public NetworkEvents NetworkEvents;

        public EnemyList EnemyList;

        private PlayerManager players;

        private EnemyManager enemies;

        public void SendObject(GameObject gameObject, ObjectType objectType)
        {
            // TODO: Distinguish between different network objects
            using (Message message = this.players.UpdateAndSerialize(gameObject, ClientMessage.UpdatePlayer))
            {
                this.Client.SendMessage(message, SendMode.Unreliable);
            }
        }

        void Awake()
        {
            if (this.NetworkConfig.isHost)
            {
                Debug.Log("Client manager shutting up.");
                gameObject.SetActive(false);
                return;
            }
            this.players = new PlayerManager(this.NetworkInstantiator.MasterPlayerCreated, this.NetworkInstantiator.PlayerUpdateRequested);
            this.enemies = new EnemyManager(players);
            if (this.Client != null)
            {
                this.Client.MessageReceived += OnResponse;
                this.NetworkEvents.LevelStartRequested.Subscribe(this.LevelStartRequested);
            }
        }

        private void OnResponse(object sender, MessageReceivedEventArgs e)
        {
            switch (e.Tag)
            {
                case ServerMessage.ConnectionData:
                    this.SetupServerData(e);
                    break;
                case ServerMessage.LevelStart:
                    this.NetworkEvents.LevelStarted.Trigger();
                    break;
                case ServerMessage.CreatePlayer:
                    this.CreatePlayer(e);
                    break;
                case ServerMessage.UpdatePlayer:
                    this.UpdatePlayer(e);
                    break;
                case ServerMessage.DeletePlayer:
                    this.DeleteObject(e);
                    break;
                case ServerMessage.UpdateEnemy:
                    UpdateEnemy(e);
                    break;
            }
        }

        private void SetupServerData(MessageReceivedEventArgs e)
        {
            ConnectionData data;
            using (Message message = e.GetMessage())
            {
                data = message.Deserialize<ConnectionData>();
            }
            
            LevelGenerationRequested.Trigger(data.LevelSeed);
            ushort clientId = data.ClientID;
            Debug.Log("Client id is " + clientId);

            foreach (Player player in data.Players)
            {
                players.Create(this.NetworkInstantiator.PlayerPrefab, player, player.NetworkID == data.PlayerObjectID);
            }

            foreach (var enemy in data.Enemies)
            {
                enemies.Create(this.EnemyList.Enemies[0], enemy, false);
            }
        }

        /// <summary>
        /// Sends level start request to server.
        /// </summary>
        /// <param name="p0">The parameter is not used.</param>
        /// <param name="p1">The parameter is not used.</param>
        /// <param name="p2">The parameter is not used.</param>
        /// <param name="p3">The parameter is not used.</param>
        private void LevelStartRequested(object p0, object p1, object p2, object p3)
        {
            using (Message message = Message.CreateEmpty(ClientMessage.LevelStartRequest))
            {
                this.Client.SendMessage(message, SendMode.Unreliable);
            }
        }

        private void CreatePlayer(MessageReceivedEventArgs e)
        {
            using (Message message = e.GetMessage())
            {
                players.Create(this.NetworkInstantiator.PlayerPrefab, message.Deserialize<Player>());
            }
        }

        private void UpdatePlayer(MessageReceivedEventArgs e)
        {
            using (Message message = e.GetMessage())
            {
                players.DeserializeAndUpdate(message);
            }
        }

        private void UpdateEnemy(MessageReceivedEventArgs e)
        {
            using (Message message = e.GetMessage())
            {
                enemies.DeserializeAndUpdate(message);
            }
        }

        private void DeleteObject(MessageReceivedEventArgs e)
        {
            ushort id;
            using (Message message = e.GetMessage())
            using (DarkRiftReader reader = message.GetReader())
            {
                id = reader.ReadUInt16();
            }
            players.Destroy(id);
        }
    }
}

