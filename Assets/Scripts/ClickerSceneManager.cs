using System.Collections;
using System.Collections.Generic;
using DarkRift;
using DarkRift.Client;
using DarkRift.Client.Unity;
using UnityEngine;

public class ClickerSceneManager : MonoBehaviour
{
    const ushort CREATE_BALL = 0;

    [SerializeField]
    [Tooltip("Client to communicate with")]
    UnityClient client;

    [SerializeField]
    [Tooltip("Object to create")]
    GameObject toCreate;

    void Awake() {
        client.MessageReceived += Client_MessageReceived;
    }

    void Client_MessageReceived(object sender, MessageReceivedEventArgs e) {
        using (Message message = e.GetMessage() as Message) {
            using (DarkRiftReader reader = message.GetReader()) {
                switch (message.Tag) {
                    case CREATE_BALL:
                        Vector3 position = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                        Instantiate(toCreate, position, Quaternion.identity);
                        break;
                }
            }
        }
    }

    void OnMouseDown() {
        Vector3 position = Camera.main.ScreenPointToRay(Input.mousePosition).GetPoint(10);
        using (DarkRiftWriter writer = DarkRiftWriter.Create()) {
            writer.Write(position.x);
            writer.Write(position.y);
            writer.Write(position.z);

            using (Message message = Message.Create(CREATE_BALL, writer)) {
                client.SendMessage(message, SendMode.Unreliable);
            }
        }
    }
}
