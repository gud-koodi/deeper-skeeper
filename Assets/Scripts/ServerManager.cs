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

    public List<IDarkRiftSerializable> networkObjects =
        new List<IDarkRiftSerializable>();

    private DarkRiftServer server;

    void Start() {
        if (Server == null) {
            Debug.LogError("No server component given.");
            Application.Quit();
            return;
        }

        server = Server.Server;

        server.ClientManager.ClientConnected += OnClientConnect;
        server.ClientManager.ClientDisconnected += OnClientDisconnect;

        print("Load scene");
        SceneManager.LoadScene("ClickerTest", LoadSceneMode.Additive);
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
                createSphere(e);
                break;
            default:
                Debug.LogException(new Exception("Unknown request " + tag));
                break;
        }
    }

    private void createSphere(MessageReceivedEventArgs e) {
        Network.ClickerSphere sphere = e.GetMessage().Deserialize<Network.ClickerSphere>();
        int clientLocalID = sphere.ID;
        int id = networkObjects.Count;
        sphere.ID = id;

        networkObjects.Add(sphere);

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
}
