namespace GudKoodi.DeeperSkeeper.Network
{
    using UnityEngine;

    public class EnemyManager : ObjectManager<Enemy>
    {
        protected override void DeserializeState(Enemy enemy, GameObject gameObject)
        {
            gameObject.transform.position = enemy.CurrentPosition;
        }

        protected override GameObject InstantiateMaster(GameObject prefab, Enemy enemy)
        {
            GameObject go = GameObject.Instantiate(prefab, enemy.CurrentPosition, Quaternion.identity);
            return go;
        }

        protected override GameObject InstantiateSlave(GameObject prefab, Enemy enemy)
        {
            GameObject go = GameObject.Instantiate(prefab, enemy.CurrentPosition, Quaternion.identity);
            return go;
        }

        protected override void SerializeState(Enemy enemy, GameObject gameObject)
        {
            enemy.CurrentPosition = gameObject.transform.position;
        }
    }
}
