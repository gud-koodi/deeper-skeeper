namespace GudKoodi.DeeperSkeeper.Event
{
    using UnityEngine;

    /// <summary>
    /// Simple event with no arguments.
    /// </summary>
    [CreateAssetMenu(fileName = "Event", menuName = "Event/Event")]
    public class Event : BaseEvent<object, object, object, object>
    {
        /// <summary>
        /// Triggers the event and notifies all listeners.
        /// </summary>
        public void Trigger()
        {
            base.Trigger(null, null, null, null);
        }
    }
}
