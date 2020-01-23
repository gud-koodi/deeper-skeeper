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
    private readonly Dictionary<int, GameObject> playerObjects = new Dictionary<int, GameObject>();

    // Lookup to find the network id of the object.
    private readonly Dictionary<int, int> networkIDLookUp = new Dictionary<int, int>();

    // Locally created objects are temporally stored by their instance IDs until server.
    private readonly Dictionary<int, GameObject> localBalls = new Dictionary<int, GameObject>();

    [Tooltip("The server component this script will communicate with.")]
    public UnityClient client;

    [Tooltip("The player object to spawn.")]
    public GameObject PlayerPrefab;

    void Awake() {
        if (client != null) {
            client.MessageReceived += OnResponse;
        }
    }

    private void InstantiatePlayer(Player player) {
        GameObject go = Instantiate(PlayerPrefab, player.Position, Quaternion.identity);

        if (!playerObjects.ContainsKey(player.ID)) {
            playerObjects[player.ID] = go;
            networkIDLookUp[go.GetInstanceID()] = player.ID;
        } else {
            Debug.LogError("New network ID " + player.ID + " was already prsent.");
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

}
