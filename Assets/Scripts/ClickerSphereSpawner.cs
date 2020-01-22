using System;
using System.Collections.Generic;
using DarkRift;
using DarkRift.Client;
using DarkRift.Client.Unity;
using Network;
using UnityEngine;

public class ClickerSphereSpawner : MonoBehaviour {

    [Tooltip("The server component this script will communicate with.")]
    public UnityClient client;

    [Tooltip("The object to spawn.")]
    public GameObject ToSpawn;

    // Objects are tracked by their network IDs decided by the server.
    private readonly Dictionary<int, GameObject> balls = new Dictionary<int, GameObject>();

    // Lookup to find the network id of the object.
    private readonly Dictionary<int, int> networkIDLookUp = new Dictionary<int, int>();

    // Locally created objects are temporally stored by their instance IDs until server.
    private readonly Dictionary<int, GameObject> localBalls = new Dictionary<int, GameObject>();

    void Awake() {
        client.MessageReceived += OnResponse;
    }

    /// <summary>
    /// Spawn new ClickerSphere to given position.
    /// </summary>
    /// <param name="position">position of the new ClickerSphere</param>
    public void Spawn(Vector3 position) {
        GameObject go = Instantiate(ToSpawn, position, Quaternion.identity);
        int instanceID = go.GetInstanceID();
        localBalls[instanceID] = go;

        ClickerSphere sphere = new ClickerSphere(instanceID, position);
        using(Message message = Message.Create((ushort) RequestTag.CREATE_SPHERE, sphere)) {
            client.SendMessage(message, SendMode.Reliable);
        }
    }

    /// <summary>
    /// Serialize and send the given gameObject to server if it is handled by this spawner.
    /// </summary>
    /// <param name="gameObject"></param>
    public void SendUpdate(GameObject gameObject) {
        int instanceId = gameObject.GetInstanceID();

        if (networkIDLookUp.ContainsKey(instanceId)) {
            int networkID = networkIDLookUp[instanceId];
            ClickerSphere sphere = new ClickerSphere(
                networkID,
                gameObject.transform.localPosition,
                gameObject.transform.localScale.x
            );

            using(Message message = Message.Create((ushort) RequestTag.UDATE_SPHERE, sphere)) {
                client.SendMessage(message, SendMode.Unreliable);
            }
        } else {
            Debug.LogError("Invalid GameObject " + gameObject + " or sent too soon");
        }

    }

    // Receive message from server and handle
    private void OnResponse(object sender, MessageReceivedEventArgs e) {
        ResponseTag tag = (ResponseTag) e.Tag;

        switch (tag) {
            case ResponseTag.CREATE_SPHERE:
                CreateSphere(e);
                break;
            case ResponseTag.UPDATE_SPHERE:
                UpdateSphere(e);
                break;
            case ResponseTag.CREATION_ID:
                SetNetworkID(e);
                break;
            default:
                Debug.Log("Unsupported response " + tag);
                break;
        }
    }

    private void CreateSphere(MessageReceivedEventArgs e) {
        ClickerSphere sphere;
        using(Message message = e.GetMessage()) sphere = message.Deserialize<ClickerSphere>();
        GameObject go = Instantiate(ToSpawn, sphere.Position, Quaternion.identity);
        go.transform.localScale = sphere.Scale * Vector3.one;

        if (balls.ContainsKey(sphere.ID)) {
            Debug.LogError("New network ID " + sphere.ID + " was already prsent.");
        }

        balls[sphere.ID] = go;
        networkIDLookUp[go.GetInstanceID()] = sphere.ID;
    }

    private void UpdateSphere(MessageReceivedEventArgs e) {
        ClickerSphere sphere;
        using(Message message = e.GetMessage()) sphere = message.Deserialize<ClickerSphere>();

        if (balls.ContainsKey(sphere.ID)) {
            GameObject go = balls[sphere.ID];
            go.transform.localScale = sphere.Scale * Vector3.one;
        } else {
            Debug.Log("Sphere to update wasn't found.");
        }
    }

    private void SetNetworkID(MessageReceivedEventArgs e) {
        using(Message message = e.GetMessage())
        using(DarkRiftReader reader = message.GetReader()) {
            int localID = reader.ReadInt32();
            int networkID = reader.ReadInt32();

            if (localBalls.ContainsKey(localID)) {
                GameObject go = localBalls[localID];
                localBalls.Remove(localID);
                balls[networkID] = go;
                networkIDLookUp[localID] = networkID;
            }
        }
    }
}
