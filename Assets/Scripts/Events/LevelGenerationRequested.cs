namespace GudKoodi.DeeperSkeeper.Event
{
    using UnityEngine;

    /// <summary>
    /// Event for requesting level generation.
    /// </summary>
    [CreateAssetMenu(fileName = "LevelGenerationRequested", menuName = "Event/LevelGenerationRequested")]
    public class LevelGenerationRequested : BaseEvent<int, object, object, object>
    {
        /// <summary>
        /// Triggers the event and notifies all listeners.
        /// </summary>
        /// <param name="seed">Random seed for generation.</param>
        public void Trigger(int seed)
        {
            base.Trigger(seed, null, null, null);
        }
    }
}
