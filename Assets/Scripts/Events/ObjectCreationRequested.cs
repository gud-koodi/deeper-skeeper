namespace GudKoodi.DeeperSkeeper.Event
{
    using UnityEngine;

    /// <summary>
    /// Event for requesting 
    /// </summary>
    [CreateAssetMenu(fileName = "ObjectCreationRequested", menuName = "Event/ObjectCreationRequested")]
    public class ObjectCreationRequested : BaseEvent<GameObject, Vector3, object, object>
    {
        public void Trigger(GameObject gameObject, Vector3 vector)
        {
            base.Trigger(gameObject, vector, null, null);
        }
    }
}
