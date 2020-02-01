using UnityEngine;
using Network;


namespace Network
{
    /// <summary>
    /// Utility object for creating new gameobjects from assigned prefabs
    /// </summary>
    [CreateAssetMenu(fileName = "NetworkInstantiator", menuName = "Config/NetworkInstantiator")]
    public class NetworkInstantiator : ScriptableObject
    {
        public GameEvent NetworkUpdate;
        public GameObject PlayerPrefab;

        internal GameObject InstantiatePlayer(Player player, bool masterObject = false)
        {
            GameObject go = Instantiate(PlayerPrefab, player.Position, Quaternion.identity);
            if (masterObject)
            {
                go.AddComponent<PlayerController>();
                PlayerController pc = go.GetComponent<PlayerController>();
                pc.playerRigidbody = go.GetComponent<Rigidbody>();
                pc.weapon = go.GetComponentInChildren<Weapon>();
                pc.hitSpeed = 1.5f;
                pc.speed = 10f;

                NetworkMaster master = go.AddComponent<NetworkMaster>();
                master.UpdateEvent = NetworkUpdate;
            } else {
                NetworkSlave slave = go.AddComponent<NetworkSlave>();
            }
            return go;
        }
    }
}
