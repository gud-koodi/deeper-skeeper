namespace GudKoodi.DeeperSkeeper.Network
{
    using UnityEngine;
    using Event;

    /// <summary>
    /// Common network event container for client and server controllers.
    /// </summary>
    [CreateAssetMenu(fileName = "NetworkEvents", menuName = "Config/NetworkEvents")]
    public class NetworkEvents : ScriptableObject
    {
        /// <summary>
        /// Event for when server should create a new enemy.
        /// </summary>
        [Tooltip("Event for when server should create a new enemy.")]
        public ObjectCreationRequested EnemyCreationRequested;
    }
}
