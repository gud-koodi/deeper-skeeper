using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DarkRift;
using DarkRift.Server;
using DarkRift.Server.Unity;

/*
    Reference for used initial setup:
    https://lestaallmaron.github.io/EmbeddedFPSExample/guide/server-basics-and-login-system.html
*/

public class Server : MonoBehaviour {
    private DarkRiftServer server;

    // Start is called before the first frame update
    void Start() {
        server = GetComponent<XmlUnityServer>().Server;
        server.ClientManager.ClientConnected += OnClientConnect;
        server.ClientManager.ClientDisconnected += OnClientDisconnect;
        print("Load scene");
        SceneManager.LoadScene("ClickerTest", LoadSceneMode.Additive);
    }

    void OnDestroy() {
        server.ClientManager.ClientConnected -= OnClientConnect;
        server.ClientManager.ClientDisconnected -= OnClientDisconnect;
    }

    private void OnClientConnect(object sender, ClientConnectedEventArgs e) {
        e.Client.MessageReceived += OnMessage;
    }

    private void OnClientDisconnect(object sender, ClientDisconnectedEventArgs e) {
        e.Client.MessageReceived -= OnMessage;
    }

    private void OnMessage(object sender, MessageReceivedEventArgs e) {
        Message response;

        using (Message message = e.GetMessage()) {
            using (DarkRiftWriter writer = DarkRiftWriter.Create()) {
                using (DarkRiftReader reader = message.GetReader()) {
                    int length = reader.Length;
                    byte[] data = reader.ReadRaw(length);
                    writer.WriteRaw(data, 0, length);
                }
                switch(e.Tag) {
                    case (ushort) ClickerMessageTag.CREATE_BALL:
                        // If creating a new ball, generate id
                        writer.Write(GameObject.FindGameObjectsWithTag("ball").Length + 1);
                        break;
                }
                response = Message.Create(e.Tag, writer);
            }
        }
        foreach(var client in server.ClientManager.GetAllClients()) {
            client.SendMessage(response, SendMode.Unreliable);
        }
    }
}
