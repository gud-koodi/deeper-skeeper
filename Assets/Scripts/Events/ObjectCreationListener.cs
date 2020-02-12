namespace GudKoodi.DeeperSkeeper.Event
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;

    /// <summary>
    /// Listener for <see cref="ObjectCreated" />.
    /// </summary>
    public class ObjectCreationListener : MonoBehaviour, IListener<GameObject, object, object, object>
    {
        /// <summary>
        /// Event to listen to.
        /// </summary>
        [Tooltip("Event to listen to.")]
        public ObjectCreated ObjectCreated;

        /// <summary>
        /// Unity event response.
        /// </summary>
        [Tooltip("Unity Event response.")]
        public ObjectCreationResponse Response;

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
            this.ObjectCreated.UnSubscribe(this);
        }

        void OnEnable()
        {
            this.ObjectCreated.Subscribe(this);
        }
    }

    /// <summary>
    /// Mandatory Unity overhead.
    /// </summary>
    [Serializable]
    public class ObjectCreationResponse : UnityEvent<GameObject>
    {
    }
}
