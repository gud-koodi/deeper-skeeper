using System;
using System.Collections.Generic;
using DarkRift;
using DarkRift.Server;
using DarkRift.Server.Unity;
using Network;
using UnityEngine;

/// <summary>
/// Component that handles all communication between the server and all clients.
/// </summary>
public class ServerManager : MonoBehaviour
{
    [Tooltip("The server component this script will communicate with")]
    public XmlUnityServer Server;

    [Tooltip("Instantiator used to create objects")]
    public NetworkInstantiator NetworkInstantiator;

    private readonly NetworkIdPool networkIdPool = new NetworkIdPool();

    private readonly Dictionary<ushort, ushort> clientToPlayerObject = new Dictionary<ushort, ushort>();

    public NetworkConfig NetworkConfig;

    private PlayerList players;

    public void Initialize()
    {
        if (Server == null)
        {
            Debug.LogError("Server component missing.");
            return;
        }

        ushort id = networkIdPool.Next();
        Player player = new Player(id, Vector3.zero);
        players.Create(player, true);

        DarkRiftServer server = Server.Server;
        server.ClientManager.ClientConnected += OnClientConnect;
        server.ClientManager.ClientDisconnected += OnClientDisconnect;
    }

    public void SendObject(GameObject gameObject)
    {
        // TODO: Distinguish between different network objects
        using (Message message = players.SerializeUpdate(gameObject, ServerMessage.UPDATE_PLAYER))
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
        players = new PlayerList(NetworkInstantiator);
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
        Player player = new Player(id, Vector3.zero);
        players.Create(player);

        ConnectionData data = new ConnectionData(e.Client.ID, id, players.ToArray());
        clientToPlayerObject[data.ClientID] = data.PlayerObjectID;
        using (Message message = Message.Create(ServerMessage.CONNECTION_DATA, data))
        {
            e.Client.SendMessage(message, SendMode.Reliable);
        }

        using (Message broadcast = Message.Create(ServerMessage.CREATE_PLAYER, player))
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
        players.Remove(playerId);
        networkIdPool.Release(playerId);
        Debug.Log($"Sending destruct message for player ID {playerId}");
        using (DarkRiftWriter writer = DarkRiftWriter.Create())
        {
            writer.Write(playerId);
            using (Message message = Message.Create(ServerMessage.DELETE_PLAYER, writer))
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
            case ClientMessage.UPDATE_PLAYER:
                UpdatePlayer(e);
                break;
        }
    }

    private void UpdatePlayer(MessageReceivedEventArgs e)
    {
        using (Message message = e.GetMessage())
        {
            players.DeserializeUpdate(message);
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
