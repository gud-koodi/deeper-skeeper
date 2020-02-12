namespace GudKoodi.DeeperSkeeper.Event
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;

    /// <summary>
    /// Listener for <see cref="ObjectUpdateRequested" />.
    /// </summary>
    public class ObjectUpdateRequestListener : MonoBehaviour, IListener<GameObject, object, object, object>
    {
        /// <summary>
        /// Event to listen to.
        /// </summary>
        [Tooltip("Event to listen to.")]
        public ObjectUpdateRequested ObjectUpdateRequested;

        /// <summary>
        /// Unity event response.
        /// </summary>
        [Tooltip("Unity Event response.")]
        public ObjectUpdateRequestResponse Response;

        /// <summary>
        /// Invokes response on trigger.
        /// </summary>
        /// <param name="gameObject">GameObject to invoke the response for.</param>
        public void OnTriggered(GameObject gameObject, object p1, object p2, object p3)
        {
            this.Response.Invoke(gameObject);
        }

        void OnDisable()
        {
            this.ObjectUpdateRequested.UnSubscribe(this);
        }

        void OnEnable()
        {
            this.ObjectUpdateRequested.Subscribe(this);
        }
    }

    /// <summary>
    /// Mandatory Unity overhead.
    /// </summary>
    [Serializable]
    public class ObjectUpdateRequestResponse : UnityEvent<GameObject>
    {
    }
}
