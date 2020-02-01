﻿using System;
using System.Collections;
using System.Collections.Generic;
using DarkRift;
using DarkRift.Client;
using DarkRift.Client.Unity;
using Network;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [Tooltip("The server component this script will communicate with.")]
    public UnityClient client;

    [Tooltip("Instantiator used to create objects")]
    public NetworkInstantiator NetworkInstantiator;

    // Locally created objects are temporally stored by their instance IDs until server.
    // private readonly Dictionary<int, GameObject> localPlayers = new Dictionary<int, GameObject>();

    private PlayerList players;

    void Awake()
    {
        players = new PlayerList(NetworkInstantiator);
        if (client != null)
        {
            client.MessageReceived += OnResponse;
        }
    }

    private void OnResponse(object sender, MessageReceivedEventArgs e)
    {
        ResponseTag tag = (ResponseTag)e.Tag;

        switch (tag)
        {
            case ResponseTag.CONNECTION_DATA:
                SetupServerData(e);
                break;
            case ResponseTag.CREATE_PLAYER:
                CreatePlayer(e);
                break;
            case ResponseTag.DELETE_OBJECT:
                DeleteObject(e);
                break;
        }
    }

    private void SetupServerData(MessageReceivedEventArgs e)
    {
        ConnectionData data;
        using (Message message = e.GetMessage()) { data = message.Deserialize<ConnectionData>(); }

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

    private void DeleteObject(MessageReceivedEventArgs e)
    {
        ushort id;
        using (Message message = e.GetMessage())
        using (DarkRiftReader reader = message.GetReader())
        {
            id = reader.ReadUInt16();
        }
        players.Remove(id);
    }
}