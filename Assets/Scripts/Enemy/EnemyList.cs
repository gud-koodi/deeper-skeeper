namespace GudKoodi.DeeperSkeeper.Enemy
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "EnemyList", menuName = "Config/EnemyList")]
    public class EnemyList : ScriptableObject
    {
        public GameObject[] Enemies;
    }
}
