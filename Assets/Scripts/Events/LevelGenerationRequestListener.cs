namespace GudKoodi.DeeperSkeeper.Event
{
    using UnityEngine;

    /// <summary>
    /// Listener for <see cref="LevelGenerationRequested" />.
    /// </summary>
    public class LevelGenerationRequestListener : MonoBehaviour, IListener<int, object, object, object>
    {
        /// <summary>
        /// Event to listen to.
        /// </summary>
        [Tooltip("Event to listen to.")]
        public LevelGenerationRequested LevelGenerationRequested;

        /// <summary>
        /// Unity event response.
        /// </summary>
        [Tooltip("Unity Event response.")]
        public LevelGenerationRequestResponse Response;

        /// <summary>
        /// Invokes response on trigger.
        /// </summary>
        /// <param name="seed">Random seed for generation.</param>
        /// <param name="p1">The parameter is not used.</param>
        /// <param name="p2">The parameter is not used.</param>
        /// <param name="p3">The parameter is not used.</param>
        public void OnTriggered(int seed, object p1, object p2, object p3)
        {
            this.Response.Invoke(seed);
        }

        void OnDisable()
        {
            this.LevelGenerationRequested.UnSubscribe(OnTriggered);
        }

        void OnEnable()
        {
            this.LevelGenerationRequested.Subscribe(OnTriggered);
        }
    }
}
