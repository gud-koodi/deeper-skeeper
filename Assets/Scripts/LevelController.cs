namespace GudKoodi.DeeperSkeeper
{
    using System.Collections;
    using Entity;
    using UnityEngine;
    using UnityEngine.AI;
    using UnityEngine.Serialization;

    /// <summary>
    /// Controller for handling the level generation.
    /// </summary>
    public class LevelController : MonoBehaviour
    {
        [FormerlySerializedAs("spawnPosition")]
        public Transform SpawnPosition;

        private const int YDISTANCE = 50;
        private const int TILESIZE = 30 * 2;
        private const int NextTileOffset = TILESIZE;
        private FloorInfo floorInfo;

        private GameObject nextLevel;
        private int depth = 10;
        private System.Random random;

        /// <summary>
        /// Sets the seed for the random generator.
        /// </summary>
        /// <value>Any int</value>
        public int RandomSeed
        {
            set => this.random = new System.Random(value);
        }

        /// <summary>
        /// Generates 10 more levels down.
        /// </summary>
        public void GenerateNext()
        {
            float nextSpawnZ = SpawnPosition.position.z;
            float nextSpawnX = SpawnPosition.position.x;

            GameObject prefab = (GameObject)Resources.Load("Prefabs/Floor1", typeof(GameObject));
            int i = 0;
            for (; i < depth; i++)
            {
                nextLevel = Instantiate(prefab, new Vector3(nextSpawnX, -YDISTANCE * (i + 1), nextSpawnZ), prefab.transform.rotation);
                floorInfo = nextLevel.GetComponent<FloorInfo>();
                int direction = random.Next(1, 4);
                if (direction == 1)
                {
                    nextSpawnZ = nextSpawnZ + NextTileOffset;
                    floorInfo.DoorNorth.SetActive(false);
                    
                    NavMeshLink link = nextLevel.gameObject.AddComponent<NavMeshLink>();
                    link.startPoint = floorInfo.PointNorth.transform.localPosition;
                    link.endPoint = new Vector3(floorInfo.PointNorth.transform.localPosition.x, -50, floorInfo.PointNorth.transform.localPosition.z + (NextTileOffset / 2));
                    link.width = 20;
                    link.bidirectional = false;
                    link.area = 2;
                    link.UpdateLink();
                }
                else if (direction == 2)
                {
                    nextSpawnX = nextSpawnX + NextTileOffset;
                    floorInfo.DoorEast.SetActive(false);

                    NavMeshLink link = nextLevel.gameObject.AddComponent<NavMeshLink>();
                    link.startPoint = floorInfo.PointEast.transform.localPosition;
                    link.endPoint = new Vector3(floorInfo.PointEast.transform.localPosition.x + (NextTileOffset / 2), -50, floorInfo.PointEast.transform.localPosition.z);
                    link.width = 20;
                    link.bidirectional = false;
                    link.area = 2;
                    link.UpdateLink();
                }
                else if (direction == 3)
                {
                    nextSpawnZ = nextSpawnZ - NextTileOffset;
                    floorInfo.DoorSouth.SetActive(false);

                    NavMeshLink link = nextLevel.gameObject.AddComponent<NavMeshLink>();
                    link.startPoint = floorInfo.PointSouth.transform.localPosition;
                    link.endPoint = new Vector3(floorInfo.PointSouth.transform.localPosition.x, -50, floorInfo.PointSouth.transform.localPosition.z - (NextTileOffset / 2));
                    link.width = 20;
                    link.bidirectional = false;
                    link.area = 2;
                    link.UpdateLink();
                }
                else if (direction == 4)
                {
                    nextSpawnX = nextSpawnX - NextTileOffset;
                    floorInfo.DoorWest.SetActive(false);

                    NavMeshLink link = nextLevel.gameObject.AddComponent<NavMeshLink>();
                    link.startPoint = floorInfo.PointWest.transform.localPosition;
                    link.endPoint = new Vector3(floorInfo.PointWest.transform.localPosition.x - (NextTileOffset / 2), -50, floorInfo.PointWest.transform.localPosition.z);
                    link.width = 20;
                    link.bidirectional = false;
                    link.area = 2;
                    link.UpdateLink();
                }

                InstantiateEnemies(nextLevel);
            }

            nextLevel = Instantiate(prefab, new Vector3(nextSpawnX, -YDISTANCE * (i + 1), nextSpawnZ), prefab.transform.rotation);
            InstantiateEnemies(nextLevel);
        }

        void Awake()
        {
            if (this.random == null)
            {
                this.random = new System.Random();
            }
        }

        private void InstantiateEnemies(GameObject room, float delay = 0.3f)
        {
            foreach (var spawner in room.GetComponentsInChildren<ISpawner>())
            {
                StartCoroutine(InstantiateEnemyDelayed(spawner, (float)random.NextDouble()));
            }
        }

        private IEnumerator InstantiateEnemyDelayed(ISpawner spawner, float percent)
        {
            yield return null;
            spawner.Spawn(percent);
        }
    }
}
