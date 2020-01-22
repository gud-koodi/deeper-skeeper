using System;
using System.Collections.Generic;
using DarkRift;
using DarkRift.Server;
using DarkRift.Server.Unity;
using Network;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ServerManager : MonoBehaviour {

    [Tooltip("The server component this script will communicate with")]
    public XmlUnityServer Server;

    // List of synchronized objects indexed by their network IDs.
    // Do not use methods that cause these indexes to change!
    private readonly List<IDarkRiftSerializable> networkObjects =
        new List<IDarkRiftSerializable>();

    private DarkRiftServer server;

    public void Initialize() {
        if (Server == null) {
            Debug.LogError("Server component missing.");
            return;
        }

        server = Server.Server;

        server.ClientManager.ClientConnected += OnClientConnect;
        server.ClientManager.ClientDisconnected += OnClientDisconnect;
    }

    void OnDestroy() {
        if (server != null) {
            server.ClientManager.ClientConnected -= OnClientConnect;
            server.ClientManager.ClientDisconnected -= OnClientDisconnect;
        }
    }

    private void OnClientConnect(object sender, ClientConnectedEventArgs e) {
        e.Client.MessageReceived += OnRequest;
    }

    private void OnClientDisconnect(object sender, ClientDisconnectedEventArgs e) {
        e.Client.MessageReceived -= OnRequest;
    }

    private void OnRequest(object sender, MessageReceivedEventArgs e) {
        RequestTag tag = (RequestTag) e.Tag;

        switch (tag) {
            case RequestTag.CREATE_SPHERE:
                CreateSphere(e);
                break;
            case RequestTag.UDATE_SPHERE:
                UpdateSphere(e);
                break;
            default:
                Debug.LogException(new Exception("Unknown request " + tag));
                break;
        }
    }

    private void CreateSphere(MessageReceivedEventArgs e) {
        ClickerSphere sphere;
        using(Message message = e.GetMessage()) sphere = message.Deserialize<ClickerSphere>();

        int clientLocalID = sphere.ID;
        int id = networkObjects.Count;
        sphere.ID = id;

        networkObjects.Insert(id, sphere);

        using(DarkRiftWriter writer = DarkRiftWriter.Create()) {
            writer.Write(clientLocalID);
            writer.Write(id);
            using(Message message = Message.Create((ushort) ResponseTag.CREATION_ID, writer)) {
                e.Client.SendMessage(message, SendMode.Reliable);
            }
        }

        using(Message broadcast = Message.Create((ushort) ResponseTag.CREATE_SPHERE, sphere)) {
            foreach (var client in server.ClientManager.GetAllClients()) {
                if (client.ID != e.Client.ID) {
                    client.SendMessage(broadcast, SendMode.Reliable);
                }
            }
        }
    }

    private void UpdateSphere(MessageReceivedEventArgs e) {
        ClickerSphere sphere;
        using(Message message = e.GetMessage()) sphere = message.Deserialize<ClickerSphere>();
        networkObjects[sphere.ID] = sphere;

        using(Message broadcast = Message.Create((ushort) ResponseTag.UPDATE_SPHERE, sphere)) {
            foreach (var client in server.ClientManager.GetAllClients()) {
                if (client.ID != e.Client.ID) {
                    client.SendMessage(broadcast, SendMode.Reliable);
                }
            }
        }
    }

}
