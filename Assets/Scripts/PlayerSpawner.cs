using System;
using System.Collections;
using System.Collections.Generic;
using DarkRift;
using DarkRift.Client;
using DarkRift.Client.Unity;
using Network;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{

    // Objects are tracked by their network IDs decided by the server.
    private readonly NetworkObjectList networkObjectList = new NetworkObjectList();

    // Locally created objects are temporally stored by their instance IDs until server.
    private readonly Dictionary<int, GameObject> localPlayers = new Dictionary<int, GameObject>();

    [Tooltip("The server component this script will communicate with.")]
    public UnityClient client;

    [Tooltip("Object that should be spawned by this component.")]
    public GameObject ObjectToSpawn;

    void Awake()
    {
        if (client != null)
        {
            client.MessageReceived += OnResponse;
        }
    }

    private GameObject InstantiatePlayer(Player player)
    {
        GameObject go = null;
        if (networkObjectList.IsVacant(player.NetworkID))
        {
            go = Instantiate(ObjectToSpawn, player.Position, Quaternion.identity);
            networkObjectList[player.NetworkID] = go;
            Debug.Log($"Assigned network ID {player.NetworkID}  to new Player instance.");
        }
        else
        {
            Debug.LogError($"Network ID {player.NetworkID} was already present.");
        }
        return go;
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

        Player[] players = data.Players;
        foreach (Player player in players)
        {
            GameObject go = InstantiatePlayer(player);
            if (player.NetworkID == data.PlayerObjectID)
            {
                go.AddComponent<PlayerController>();
                PlayerController pc = go.GetComponent<PlayerController>();
                pc.playerRigidbody = go.GetComponent<Rigidbody>();
                pc.speed = 10f;
            }
        }
    }

    private void CreatePlayer(MessageReceivedEventArgs e)
    {
        Player player;
        using (Message message = e.GetMessage()) player = message.Deserialize<Player>();
        InstantiatePlayer(player);
    }

    private void DeleteObject(MessageReceivedEventArgs e)
    {
        ushort id;
        using (Message message = e.GetMessage())
        using (DarkRiftReader reader = message.GetReader())
        {
            id = reader.ReadUInt16();
        }

        if (!networkObjectList.IsVacant(id))
        {
            GameObject go = networkObjectList.RemoveAt(id);
            Debug.Log("Removing " + go + " at network id " + id);
            Destroy(go);
        }
        else
        {
            Debug.LogError(id + " the EVIL!~!!!");
        }
    }
}
