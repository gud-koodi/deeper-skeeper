namespace GudKoodi.DeeperSkeeper.Event
{
    using UnityEngine;

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
        /// <param name="p1">The parameter is not used.</param>
        /// <param name="p2">The parameter is not used.</param>
        /// <param name="p3">The parameter is not used.</param>
        public void OnTriggered(GameObject gameObject, object p1, object p2, object p3)
        {
            this.Response.Invoke(gameObject);
        }

        void OnDisable()
        {
            this.ObjectUpdateRequested.UnSubscribe(OnTriggered);
        }

        void OnEnable()
        {
            this.ObjectUpdateRequested.Subscribe(OnTriggered);
        }
    }
}
