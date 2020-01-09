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
        foreach(var client in server.ClientManager.GetAllClients()) {
            client.SendMessage(e.GetMessage(), SendMode.Unreliable);
        }
    }
}
