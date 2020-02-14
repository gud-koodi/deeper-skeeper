using UnityEngine;
using UnityEngine.AI;
using GudKoodi.DeeperSkeeper.Value;

public class LevelController : MonoBehaviour
{
    public Transform spawnPosition;

    private GameObject nextLevel;
    private int depth = 10;
    private System.Random random;
    private const int YDISTANCE = 50;
    private const int TILESIZE = 30 * 2;
    private const int NEXT_TILE_OFFSET = TILESIZE;
    private FloorInfo floorInfo;


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
        float nextSpawnZ = spawnPosition.position.z;
        float nextSpawnX = spawnPosition.position.x;

        GameObject prefab = (GameObject)Resources.Load("Prefabs/Floor1", typeof(GameObject));
        int i = 0;
        for (; i < depth; i++)
        {

            nextLevel = Instantiate(prefab, new Vector3(nextSpawnX, -YDISTANCE * (i + 1), nextSpawnZ), prefab.transform.rotation);
            floorInfo = nextLevel.GetComponent<FloorInfo>();
            int direction = random.Next(1, 4);
            if (direction == 1)
            {
                nextSpawnZ = nextSpawnZ + NEXT_TILE_OFFSET;
                floorInfo.DoorNorth.SetActive(false);

                NavMeshLink link = nextLevel.gameObject.AddComponent<NavMeshLink>();
                link.startPoint = floorInfo.PointNorth.transform.localPosition;
                link.endPoint = new Vector3(floorInfo.PointNorth.transform.localPosition.x, -50, floorInfo.PointNorth.transform.localPosition.z + (NEXT_TILE_OFFSET / 2));
                link.width = 20;
                link.bidirectional = false;
                link.area = 2;
                link.UpdateLink();

            }
            else if (direction == 2)
            {
                nextSpawnX = nextSpawnX + NEXT_TILE_OFFSET;
                floorInfo.DoorEast.SetActive(false);

                NavMeshLink link = nextLevel.gameObject.AddComponent<NavMeshLink>();
                link.startPoint = floorInfo.PointEast.transform.localPosition;
                link.endPoint = new Vector3(floorInfo.PointEast.transform.localPosition.x + (NEXT_TILE_OFFSET / 2), -50, floorInfo.PointEast.transform.localPosition.z);
                link.width = 20;
                link.bidirectional = false;
                link.area = 2;
                link.UpdateLink();

            }
            else if (direction == 3)
            {
                nextSpawnZ = nextSpawnZ - NEXT_TILE_OFFSET;
                floorInfo.DoorSouth.SetActive(false);

                NavMeshLink link = nextLevel.gameObject.AddComponent<NavMeshLink>();
                link.startPoint = floorInfo.PointSouth.transform.localPosition;
                link.endPoint = new Vector3(floorInfo.PointSouth.transform.localPosition.x, -50, floorInfo.PointSouth.transform.localPosition.z - (NEXT_TILE_OFFSET / 2));
                link.width = 20;
                link.bidirectional = false;
                link.area = 2;
                link.UpdateLink();

            }
            else if (direction == 4)
            {
                nextSpawnX = nextSpawnX - NEXT_TILE_OFFSET;
                floorInfo.DoorWest.SetActive(false);

                NavMeshLink link = nextLevel.gameObject.AddComponent<NavMeshLink>();
                link.startPoint = floorInfo.PointWest.transform.localPosition;
                link.endPoint = new Vector3(floorInfo.PointWest.transform.localPosition.x - (NEXT_TILE_OFFSET / 2), -50, floorInfo.PointWest.transform.localPosition.z);
                link.width = 20;
                link.bidirectional = false;
                link.area = 2;
                link.UpdateLink();

            }
        }

        nextLevel = Instantiate(prefab, new Vector3(nextSpawnX, -YDISTANCE * (i + 1), nextSpawnZ), prefab.transform.rotation);
    }

    void Awake()
    {
        if (this.random == null)
        {
            this.random = new System.Random();
        }
    }
}