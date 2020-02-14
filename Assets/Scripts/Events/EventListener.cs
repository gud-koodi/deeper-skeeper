namespace GudKoodi.DeeperSkeeper.Event
{
    using UnityEngine;

    /// <summary>
    /// Listener for <see cref="Event" />.
    /// </summary>
    public class EventListener : MonoBehaviour, IListener<object, object, object, object>
    {
        /// <summary>
        /// Event to listen to.
        /// </summary>
        [Tooltip("Event to listen to.")]
        public Event Event;

        /// <summary>
        /// Unity event response.
        /// </summary>
        [Tooltip("Unity Event response.")]
        public EventResponse Response;

        /// <summary>
        /// Invokes response on trigger.
        /// </summary>
        /// <param name="p0">The parameter is not used.</param>
        /// <param name="p1">The parameter is not used.</param>
        /// <param name="p2">The parameter is not used.</param>
        /// <param name="p3">The parameter is not used.</param>
        public void OnTriggered(object p0, object p1, object p2, object p3)
        {
            this.Response.Invoke(this.gameObject);
        }

        void OnDisable()
        {
            this.Event.UnSubscribe(OnTriggered);
        }

        void OnEnable()
        {
            this.Event.Subscribe(OnTriggered);
        }
    }
}
