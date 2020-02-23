namespace GudKoodi.DeeperSkeeper.Network
{
    using Event;
    using UnityEngine;

    using Event = Event.Event;

    /// <summary>
    /// Common network event container for client and server controllers.
    /// </summary>
    [CreateAssetMenu(fileName = "NetworkEvents", menuName = "Config/NetworkEvents")]
    public class NetworkEvents : ScriptableObject
    {
        /// <summary>
        /// Event for when a level is started.
        /// </summary>
        public Event LevelStarted;

        /// <summary>
        /// Event for requesting level start.
        /// </summary>
        [Tooltip("Event for when the run should start.")]
        public Event LevelStartRequested;

        /// <summary>
        /// Event for when server should create a new enemy.
        /// </summary>
        [Tooltip("Event for when server should create a new enemy.")]
        public ObjectCreationRequested EnemyCreationRequested;

        /// <summary>
        /// Event for when object should be destroyed.
        /// </summary>
        public ObjectUpdateRequested ObjectDestructionRequested;
    }
}
