using System.Collections.Generic;
using DarkRift;
using DarkRift.Server;
using DarkRift.Server.Unity;
using Network;
using UnityEngine;
using Value;

/// <summary>
/// Component that handles all communication between the server and all clients.
/// </summary>
public class ServerController : MonoBehaviour
{
    [Tooltip("The server component this script will communicate with")]
    public XmlUnityServer Server;

    [Tooltip("Instantiator used to create objects")]
    public NetworkInstantiator NetworkInstantiator;

    [Tooltip("Seed for creating the level.")]
    public IntValue LevelSeed;

    private readonly NetworkIdPool networkIdPool = new NetworkIdPool();

    private readonly Dictionary<ushort, ushort> clientToPlayerObject = new Dictionary<ushort, ushort>();

    public NetworkConfig NetworkConfig;

    private PlayerManager players;

    public void Initialize()
    {
        if (Server == null)
        {
            Debug.LogError("Server component missing.");
            return;
        }

        this.LevelSeed.Value = new System.Random().Next();

        ushort id = networkIdPool.Next();
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
        if (!NetworkConfig.isHost)
        {
            Debug.Log("Server manager shutting up.");
            gameObject.SetActive(false);
        }
        players = new PlayerManager(this.NetworkInstantiator.MasterPlayerCreated, this.NetworkInstantiator.NetworkUpdate);
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
        ushort id = networkIdPool.Next();
        Player player = new Player(id, Vector3.zero, 0);
        players.Create(this.NetworkInstantiator.PlayerPrefab, player);

        ConnectionData data = new ConnectionData(e.Client.ID, id, LevelSeed.Value, players.ToArray());
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
        networkIdPool.Release(playerId);
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
}
