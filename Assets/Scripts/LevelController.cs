using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public Transform spawnPosition;

    private GameObject currentLevel;
    private GameObject nextLevel;
    private int depth = 10;
    private System.Random random;
    private const int YDISTANCE = 50;
    private const int TILESIZE = 30 * 2;
    private const int NEXT_TILE_OFFSET = TILESIZE;

    /// <summary>
    /// Gets and Sets seed for the random generator that generates levels.
    /// </summary>
    /// <value>Any int</value>
    public int RandomSeed { get; set; }

    /// <summary>
    /// Generates 10 more levels down.
    /// </summary>
    public void GenerateNext()
    {
        float nextSpawnZ = spawnPosition.position.z;
        float nextSpawnX = spawnPosition.position.x;
        FloorInfo floorInfo;
        for (int i = 0; i < depth; i++)
        {
            GameObject prefab = (GameObject)Resources.Load("Prefabs/Floor1", typeof(GameObject));
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
    }

    void Start()
    {
        random = new System.Random(RandomSeed);
        GenerateNext();
    }
}