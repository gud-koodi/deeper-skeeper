namespace GudKoodi.DeeperSkeeper.Enemy
{
    using System.Collections;
    using Event;
    using UnityEngine;

    /// <summary>
    /// Creates the designed enemy and destroys spawner.
    /// </summary>
    public class EnemySpawner : MonoBehaviour
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

        void Start()
        {
            // TODO: Choose random enemy from the list.
            StartCoroutine(Wait());
        }

        private IEnumerator Wait()
        {
            yield return new WaitForSeconds(0.3f);

            EnemyCreationRequested.Trigger(Enemies.Enemies[0], transform.position);
            Destroy(gameObject);
        }
    }
}
