namespace GudKoodi.DeeperSkeeper.Event
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;

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
        public void OnTriggered(object p0, object p1, object p2, object p3)
        {
            this.Response.Invoke(this.gameObject);
        }

        void OnDisable()
        {
            this.Event.UnSubscribe(this);
        }

        void OnEnable()
        {
            this.Event.Subscribe(this);
        }
    }

    /// <summary>
    /// Mandatory Unity overhead.
    /// </summary>
    [Serializable]
    public class EventResponse : UnityEvent<GameObject>
    {
    }
}
