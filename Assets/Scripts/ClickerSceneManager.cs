using System.Collections;
using System.Collections.Generic;
using DarkRift;
using DarkRift.Client;
using DarkRift.Client.Unity;
using UnityEngine;

public class ClickerSceneManager : MonoBehaviour
{

    [SerializeField]
    [Tooltip("Client to communicate with")]
    UnityClient client;

    [SerializeField]
    [Tooltip("Object to create")]
    GameObject toCreate;

    private Dictionary<int, GameObject> balls;

    void Awake() {
        client.MessageReceived += Client_MessageReceived;
        balls = new Dictionary<int, GameObject>();
    }

    void Client_MessageReceived(object sender, MessageReceivedEventArgs e) {
        using (Message message = e.GetMessage() as Message) {
            using (DarkRiftReader reader = message.GetReader()) {
                switch ((ClickerMessageTag) message.Tag) {
                    case ClickerMessageTag.CREATE_BALL: {
                        Vector3 position = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                        int id = reader.ReadInt32();
                        GameObject go = Instantiate(toCreate, position, Quaternion.identity);
                        go.GetComponent<ClickerSphere>().id = id;
                        go.GetComponent<ClickerSphere>().manager = this;
                        balls[id] = go;
                        break;
                    }
                    case ClickerMessageTag.GROW_BALL: {
                        float newScale = reader.ReadSingle();
                        int id = reader.ReadInt32();
                        balls[id].transform.localScale = newScale * Vector3.one;
                        break;
                    }
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

            using (Message message = Message.Create((ushort) ClickerMessageTag.CREATE_BALL, writer)) {
                client.SendMessage(message, SendMode.Unreliable);
            }
        }
    }

    public void GrowBall(ClickerSphere sphere) {
        using (DarkRiftWriter writer = DarkRiftWriter.Create()) {
            writer.Write(sphere.transform.localScale.x + 0.1f);
            writer.Write(sphere.id);

            using (Message message = Message.Create((ushort) ClickerMessageTag.GROW_BALL, writer)) {
                client.SendMessage(message, SendMode.Unreliable);
            }
        }
    }
}
