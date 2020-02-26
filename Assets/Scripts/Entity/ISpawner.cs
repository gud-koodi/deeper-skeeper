namespace GudKoodi.DeeperSkeeper.Entity
{
    /// <summary>
    /// Interface for components that can spawn objects.
    /// </summary>
    public interface ISpawner
    {
        /// <summary>
        /// Spawns enemy conditionally using the given percent.
        /// </summary>
        /// <param name="percent">Random number between 0..1.</param>
        void Spawn(float percent);
    }
}
