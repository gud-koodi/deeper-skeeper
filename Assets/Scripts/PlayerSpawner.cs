using System;
using System.Collections;
using System.Collections.Generic;
using DarkRift;
using DarkRift.Client;
using DarkRift.Client.Unity;
using Network;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour {
    // Objects are tracked by their network IDs decided by the server.
    private readonly Dictionary<ushort, GameObject> playerObjects = new Dictionary<ushort, GameObject>();

    // Lookup to find the network id of the object.
    private readonly Dictionary<int, ushort> networkIDLookUp = new Dictionary<int, ushort>();

    // Locally created objects are temporally stored by their instance IDs until server.
    private readonly Dictionary<int, GameObject> localPlayers = new Dictionary<int, GameObject>();

    [Tooltip("The server component this script will communicate with.")]
    public UnityClient client;

    void Awake() {
        if (client != null) {
            client.MessageReceived += OnResponse;
        }
    }

    private void InstantiatePlayer(Player player) {
        GameObject go = player.toGameObject();

        if (!playerObjects.ContainsKey(player.ID)) {
            playerObjects[player.ID] = go;
            networkIDLookUp[go.GetInstanceID()] = player.ID;
            Debug.Log("Assigned network ID " + player.ID + " to new Player instance.");
        } else {
            Debug.LogError("New network ID " + player.ID + " was already present.");
        }
    }

    private void OnResponse(object sender, MessageReceivedEventArgs e) {
        ResponseTag tag = (ResponseTag) e.Tag;

        switch (tag) {
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

    private void SetupServerData(MessageReceivedEventArgs e) {
        ushort clientId;
        Player[] players;
        using(Message message = e.GetMessage())
        using(DarkRiftReader reader = message.GetReader()) {
            clientId = reader.ReadUInt16();
            players = reader.ReadSerializables<Player>();
        }

        Debug.Log("Client id is " + clientId);

        foreach (Player player in players) {
            InstantiatePlayer(player);
        }
    }

    private void CreatePlayer(MessageReceivedEventArgs e) {
        Player player;
        using(Message message = e.GetMessage()) player = message.Deserialize<Player>();
        InstantiatePlayer(player);
    }

    private void DeleteObject(MessageReceivedEventArgs e) {
        ushort id;
        using(Message message = e.GetMessage())
        using(DarkRiftReader reader = message.GetReader()) {
            id = reader.ReadUInt16();
        }

        if (playerObjects.ContainsKey(id)) {
            GameObject go = playerObjects[id];
            playerObjects.Remove(id);
            GameObject.Destroy(go);
        } else {
            Debug.LogError("ID missing from client");
        }
    }
}
