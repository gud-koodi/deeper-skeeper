using System;
using System.Collections.Generic;
using DarkRift;
using DarkRift.Client;
using DarkRift.Client.Unity;
using Network;
using UnityEngine;

public class ClickerSphereSpawner : MonoBehaviour {

    [Tooltip("The server component this script will communicate with")]
    public UnityClient client;

    public GameObject ToCreate;

    [SerializeField]
    public Dictionary<int, GameObject> balls = new Dictionary<int, GameObject>();
    [SerializeField]
    public Dictionary<int, GameObject> localBalls = new Dictionary<int, GameObject>();

    void Awake() {
        client.MessageReceived += OnResponse;
    }

    private void OnResponse(object sender, MessageReceivedEventArgs e) {
        ResponseTag tag = (ResponseTag) e.Tag;

        switch (tag) {
            case ResponseTag.CREATE_SPHERE:
                createSphere(e);
                break;
            case ResponseTag.CREATION_ID:
                setID(e);
                break;
            default:
                Debug.LogException(new Exception("Unknown request " + tag));
                break;
        }
    }

    public void CreateSphere(Network.ClickerSphere sphere) {
        Vector3 position = new Vector3(sphere.X, sphere.Y, sphere.Z);
        GameObject go = Instantiate(ToCreate, position, Quaternion.identity);
        sphere.ID = go.GetInstanceID();

        localBalls[sphere.ID] = go;
        using(Message message = Message.Create((ushort) RequestTag.CREATE_SPHERE, sphere)) {
            client.SendMessage(message, SendMode.Reliable);
        }
    }

    private void createSphere(MessageReceivedEventArgs e) {
        Network.ClickerSphere sphere = e.GetMessage().Deserialize<Network.ClickerSphere>();
        Vector3 position = new Vector3(sphere.X, sphere.Y, sphere.Z);
        GameObject go = Instantiate(ToCreate, position, Quaternion.identity);
        go.transform.localScale = sphere.Scale * Vector3.one;

        if (balls.ContainsKey(sphere.ID)) {
            Debug.LogError("New network ID " + sphere.ID + " was already prsent.");
        }
        balls[sphere.ID] = go;
    }

    private void setID(MessageReceivedEventArgs e) {
        using(DarkRiftReader reader = e.GetMessage().GetReader()) {
            int localID = reader.ReadInt32();
            int networkID = reader.ReadInt32();
            if (localBalls.ContainsKey(localID)) {
                GameObject go = localBalls[localID];
                localBalls.Remove(localID);
                balls[networkID] = go;
            }
        }
    }
}
