namespace GudKoodi.DeeperSkeeper.Event
{
    using UnityEngine;

    /// <summary>
    /// Temporary solution to distinguish between objects.
    /// </summary>
    public enum ObjectType
    {
        /// <summary>Player type object.</summary>
        Player,

        /// <summary>Enemy type object.</summary>
        Enemy
    }

    /// <summary>
    /// Event for requesting object updates.
    /// </summary>
    [CreateAssetMenu(fileName = "ObjectUpdateRequested", menuName = "Event/ObjectUpdateRequested")]
    public class ObjectUpdateRequested : BaseEvent<GameObject, ObjectType, object, object>
    {
        /// <summary>
        /// Triggers the event and notifies all listeners.
        /// </summary>
        /// <param name="gameObject">GameObject to trigger the event for.</param>
        /// <param name="objectType">Type of Object.</param>
        public void Trigger(GameObject gameObject, ObjectType objectType)
        {
            base.Trigger(gameObject, objectType, null, null);
        }
    }
}
