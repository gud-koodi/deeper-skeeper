namespace GudKoodi.DeeperSkeeper.Enemy
{
    using System.Collections;
    using Entity;
    using Event;
    using UnityEngine;

    /// <summary>
    /// Creates the designed enemy and destroys spawner.
    /// </summary>
    public class EnemySpawner : MonoBehaviour, ISpawner
    {
        /// <summary>
        /// List of enemies to choose from.
        /// </summary>
        [Tooltip("List of enemies to choose from.")]
        public EnemyList Enemies;

        /// <summary>
        /// Event to call when enemy is decided.
        /// </summary>
        public ObjectCreationRequested EnemyCreationRequested;

        [Range(0, 1)]
        public float SpawnChange = 1.0f;

        /// <summary>
        /// Spawns a single enemy if given percent is high enough.
        /// </summary>
        /// <param name="percent">Chance percent given for the call.</param>
        public void Spawn(float percent)
        {
            if (this.SpawnChange > percent)
            {
                EnemyCreationRequested.Trigger(Enemies.Enemies[0], transform.position);
            }
        }

        private IEnumerator Wait()
        {
            yield return new WaitForSeconds(0.3f);
            EnemyCreationRequested.Trigger(Enemies.Enemies[0], transform.position);
        }
    }
}
