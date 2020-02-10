namespace GudKoodi.DeeperSkeeper.Network
{
    using UnityEngine;
    using Event;

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
    }
}
