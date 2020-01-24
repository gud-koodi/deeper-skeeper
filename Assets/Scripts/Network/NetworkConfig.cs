using System;
using System.Net;
using UnityEngine;

namespace Network {
    [CreateAssetMenu(fileName = "NetworkConfig", menuName = "Config/NetworkConfig")]
    public class NetworkConfig : ScriptableObject, ISerializationCallbackReceiver {

        public IPAddress ip;

        public int port;

        public bool isHost;

        // Prefab hooks for network objects

        [Tooltip("Reference to player prefab")]
        public GameObject playerPrefab;

        public string Ip {
            set {
                try {
                    IPAddress address = IPAddress.Parse(value);
                    ip = address;
                } catch (FormatException) { }
            }
        }

        public string Port {
            set {
                try {
                    port = int.Parse(value);
                } catch (FormatException) { }
            }
        }

        // Temporary serialization resets at launch

        public void OnAfterDeserialize() {
            Ip = "127.0.0.1";
            port = 4296;
            isHost = true;

            Player.Prefab = playerPrefab;
        }

        public void OnBeforeSerialize() { }
    }
}
