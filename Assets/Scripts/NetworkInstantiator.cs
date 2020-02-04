namespace Network
{
    using UnityEngine;

    /// <summary>
    /// Utility object for creating new gameobjects from assigned prefabs
    /// </summary>
    [CreateAssetMenu(fileName = "NetworkInstantiator", menuName = "Config/NetworkInstantiator")]
    public class NetworkInstantiator : ScriptableObject
    {
        [Tooltip("Network update event called by master objects.")]

        /// <summary>
        /// Network update event called by master objects.
        /// </summary>
        public GameEvent NetworkUpdate;

        [Tooltip("Game event called when new master player is created.")]
        
        /// <summary>
        /// Game event called when new master player is created.
        /// </summary>
        public GameEvent MasterPlayerCreated;

        [Tooltip("Prefab used for creating new players.")]

        /// <summary>
        /// Prefab used for creating new players.
        /// </summary>
        public GameObject PlayerPrefab;

        /// <summary>
        /// Instantiates a new player GameObject.
        /// </summary>
        /// <param name="player">Data used when creating player.</param>
        /// <param name="masterObject">Whether or not this object is controlled locally.</param>
        /// <returns>Created player.</returns>
        internal GameObject InstantiatePlayer(Player player, bool masterObject = false)
        {
            GameObject go = Instantiate(PlayerPrefab, player.CurrentPosition, Quaternion.identity);
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

                // TODO: Do this call elsewhere, but for now having a direct access to its reference is sooo niice.
                Debug.Log("gooogg " + go);
                MasterPlayerCreated.Trigger(go);
            }
            else
            {
                NetworkSlave slave = go.AddComponent<NetworkSlave>();
                slave.Rigidbody = go.GetComponent<Rigidbody>();
                slave.UpdateState(player);
            }
            return go;
        }
    }
}
