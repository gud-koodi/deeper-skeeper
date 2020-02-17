namespace GudKoodi.DeeperSkeeper.Level
{
    using UnityEngine;

    using Event = Event.Event;

    /// <summary>
    /// Component for start button in lobby.
    /// </summary>
    public class LobbyStartButton : MonoBehaviour
    {
        /// <summary>
        /// Let players into the level when this event is called.
        /// </summary>
        public Event LevelStarted;

        /// <summary>
        /// Send this event to request level start.
        /// </summary>
        public Event LevelStartRequested;

        /// <summary>
        /// Floor to destroy on start.
        /// </summary>
        public GameObject Floor;

        void Awake()
        {
            this.LevelStarted.Subscribe(this.StartLevel);
        }

        void ApplyDamage()
        {
            this.LevelStartRequested.Trigger();
        }

        /// <summary>
        /// Lets all players to game from lobby.
        /// </summary>
        /// <param name="p0">The parameter is not used.</param>
        /// <param name="p1">The parameter is not used.</param>
        /// <param name="p2">The parameter is not used.</param>
        /// <param name="p3">The parameter is not used.</param>
        private void StartLevel(object p0, object p1, object p2, object p3)
        {
            Destroy(this.Floor);
        }
    }
}
