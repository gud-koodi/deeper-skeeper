namespace GudKoodi.DeeperSkeeper.Network
{
    using System;
    using System.Net;
    using UnityEngine;

    [CreateAssetMenu(fileName = "NetworkConfig", menuName = "Config/NetworkConfig")]
    public class NetworkConfig : ScriptableObject, ISerializationCallbackReceiver
    {
        public IPAddress ip;

        public int port;

        public bool isHost;

        // These two below are required for UI elements
        public string Ip
        {
            set
            {
                try
                {
                    IPAddress address = IPAddress.Parse(value);
                    ip = address;
                }
                catch (FormatException)
                {
                }
            }
        }

        public string Port
        {
            set
            {
                try
                {
                    port = int.Parse(value);
                }
                catch (FormatException)
                {
                }
            }
        }

        // Temporary serialization resets at launch
        public void OnAfterDeserialize()
        {
            Ip = "127.0.0.1";
            port = 4296;
            isHost = true;
        }

        public void OnBeforeSerialize()
        {
        }
    }
}
