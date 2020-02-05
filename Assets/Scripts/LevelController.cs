using UnityEngine;
using Value;

public class LevelController : MonoBehaviour, ICallable<int>
{
    public Transform spawnPosition;

    [Tooltip("Seed for level generation.")]
    public IntValue LevelSeed;

    private GameObject currentLevel;
    private GameObject nextLevel;
    private int depth = 10;
    private System.Random random;
    private const int YDISTANCE = 50;
    private const int TILESIZE = 30 * 2;
    private const int NEXT_TILE_OFFSET = TILESIZE;

    /// <summary>
    /// Gets or sets seed for the random generator that generates levels.
    /// </summary>
    /// <value>Any int</value>
    public int RandomSeed { get; set; }

    /// <summary>
    /// Sets the seed and calls level generation.
    /// </summary>
    /// <param name="seed">Random seed.</param>
    public void Call(int seed)
    {
        this.random = new System.Random(seed);
        this.GenerateNext();
    }

    /// <summary>
    /// Generates 10 more levels down.
    /// </summary>
    public void GenerateNext()
    {
        float nextSpawnZ = spawnPosition.position.z;
        float nextSpawnX = spawnPosition.position.x;
        FloorInfo floorInfo;
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
            }
            else if (direction == 2)
            {
                nextSpawnX = nextSpawnX + NEXT_TILE_OFFSET;
                floorInfo.DoorEast.SetActive(false);
            }
            else if (direction == 3)
            {
                nextSpawnZ = nextSpawnZ - NEXT_TILE_OFFSET;
                floorInfo.DoorSouth.SetActive(false);
            }
            else if (direction == 4)
            {
                nextSpawnX = nextSpawnX - NEXT_TILE_OFFSET;
                floorInfo.DoorWest.SetActive(false);
            }
        }
        
        nextLevel = Instantiate(prefab, new Vector3(nextSpawnX, -YDISTANCE * (i + 1), nextSpawnZ), prefab.transform.rotation);
    }

    void Awake()
    {
        this.LevelSeed.Subscribe(this);
    }

    void Start()
    {
        random = new System.Random(RandomSeed);
        //// GenerateNext();
    }
}