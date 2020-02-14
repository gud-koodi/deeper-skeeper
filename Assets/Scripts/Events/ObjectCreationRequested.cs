namespace GudKoodi.DeeperSkeeper.Event
{
    using UnityEngine;

    /// <summary>
    /// Event for requesting the creation of objects.
    /// </summary>
    [CreateAssetMenu(fileName = "ObjectCreationRequested", menuName = "Event/ObjectCreationRequested")]
    public class ObjectCreationRequested : BaseEvent<GameObject, Vector3, object, object>
    {
        /// <summary>
        /// Triggers the event and notifies all listeners.
        /// </summary>
        /// <param name="gameObject">Prefab of the object to create.</param>
        /// <param name="position">Position for where to create the object.</param>
        public void Trigger(GameObject gameObject, Vector3 position)
        {
            base.Trigger(gameObject, position, null, null);
        }
    }
}
