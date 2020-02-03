using System;
using System.Collections;
using System.Collections.Generic;
using DarkRift;
using DarkRift.Client;
using DarkRift.Client.Unity;
using Network;
using UnityEngine;
using Value;

public class ClientController : MonoBehaviour
{
    [Tooltip("The server component this script will communicate with.")]
    public UnityClient client;

    [Tooltip("Instantiator used to create objects")]
    public NetworkInstantiator NetworkInstantiator;

    [Tooltip("Network configuration to read check host status from.")]
    public NetworkConfig NetworkConfig;

    [Tooltip("Seed for generating level.")]
    public IntValue LevelSeed;

    private PlayerList players;

    public void SendObject(GameObject gameObject)
    {
        // TODO: Distinguish between different network objects
        using (Message message = players.SerializeUpdate(gameObject, ClientMessage.UpdatePlayer))
        {
            client.SendMessage(message, SendMode.Unreliable);
        }
    }

    void Awake()
    {
        if (NetworkConfig.isHost)
        {
            Debug.Log("Client manager shutting up.");
            gameObject.SetActive(false);
        }
        players = new PlayerList(NetworkInstantiator);
        if (client != null)
        {
            client.MessageReceived += OnResponse;
        }
    }

    private void OnResponse(object sender, MessageReceivedEventArgs e)
    {
        switch (e.Tag)
        {
            case ServerMessage.ConnectionData:
                SetupServerData(e);
                break;
            case ServerMessage.CreatePlayer:
                CreatePlayer(e);
                break;
            case ServerMessage.UpdatePlayer:
                UpdatePlayer(e);
                break;
            case ServerMessage.DeletePlayer:
                DeleteObject(e);
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
        this.LevelSeed.Value = data.LevelSeed;
        ushort clientId = data.ClientID;
        Debug.Log("Client id is " + clientId);

        foreach (Player player in data.Players)
        {
            players.Create(player, player.NetworkID == data.PlayerObjectID);
        }
    }

    private void CreatePlayer(MessageReceivedEventArgs e)
    {
        using (Message message = e.GetMessage()) { players.Create(message.Deserialize<Player>()); }
    }

    private void UpdatePlayer(MessageReceivedEventArgs e)
    {
        using (Message message = e.GetMessage())
        {
            players.DeserializeUpdate(message);
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
