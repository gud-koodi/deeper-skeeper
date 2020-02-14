namespace GudKoodi.DeeperSkeeper.Enemy
{
    using UnityEngine;

    /// <summary>
    /// Container class for enemy prefabs.
    /// </summary>
    [CreateAssetMenu(fileName = "EnemyList", menuName = "Config/EnemyList")]
    public class EnemyList : ScriptableObject
    {
        /// <summary>
        /// List of enemy prefabs.
        /// </summary>
        public GameObject[] Enemies;
    }
}
