namespace GudKoodi.DeeperSkeeper.Event
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;

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
        public void OnTriggered(int seed, object p1, object p2, object p3)
        {
            this.Response.Invoke(seed);
        }

        void OnDisable()
        {
            this.LevelGenerationRequested.UnSubscribe(this);
        }

        void OnEnable()
        {
            this.LevelGenerationRequested.Subscribe(this);
        }
    }

    /// <summary>
    /// Mandatory Unity overhead.
    /// </summary>
    [Serializable]
    public class LevelGenerationRequestResponse : UnityEvent<int>
    {
    }
}
