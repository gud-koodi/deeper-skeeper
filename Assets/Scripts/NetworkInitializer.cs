using DarkRift;
using DarkRift.Client.Unity;
using DarkRift.Server.Unity;
using GudKoodi.DeeperSkeeper.Network;
using UnityEngine;

using Event = GudKoodi.DeeperSkeeper.Event.Event;

public class NetworkInitializer : MonoBehaviour
{
    [Tooltip("NetworkConfig to read configurations from")]
    public NetworkConfig NetworkConfig;

    [Tooltip("DarkRift server to initialize if host")]
    public XmlUnityServer Server;

    [Tooltip("DarkRift client to initialize")]
    public UnityClient Client;

    [Tooltip("Event to trigger after server is initialized")]
    public Event ServerInitialized;

    void Start()
    {
        if (Client == null || NetworkConfig == null || Server == null)
        {
            Debug.LogError("Missing network component references, unable to initialize");
            return;
        }

        if (NetworkConfig.isHost)
        {
            Server.Create();
            ServerInitialized.Trigger();
        } else {
            Client.Connect(NetworkConfig.ip, NetworkConfig.port, IPVersion.IPv4);
        }
    }
}
