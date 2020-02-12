namespace GudKoodi.DeeperSkeeper.Event
{
    using UnityEngine;

    /// <summary>
    /// Event for reacting to object updates.
    /// </summary>
    [CreateAssetMenu(fileName = "ObjectUpdateRequested", menuName = "Event/ObjectUpdateRequested")]
    public class ObjectUpdateRequested : BaseEvent<GameObject, object, object, object>
    {
        /// <summary>
        /// Triggers the event and notifies all listeners.
        /// </summary>
        /// <param name="gameObject">GameObject to trigger the event for.</param>
        public void Trigger(GameObject gameObject)
        {
            base.Trigger(gameObject, null, null, null);
        }
    }
}
