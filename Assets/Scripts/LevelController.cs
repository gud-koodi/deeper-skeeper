using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour {
    public int LevelCounter { get; set; }
    public int randomSeed;

    public Transform spawnPosition;

    private GameObject currentLevel;
    private GameObject nextLevel;
    // Start is called before the first frame update
    void Start() {
        LevelCounter = 1;
        Random.InitState(randomSeed);
        GameObject prefab = (GameObject) Resources.Load("Prefabs/Floor1", typeof(GameObject));
        nextLevel = Instantiate(prefab, new Vector3(spawnPosition.position.x, -50, spawnPosition.position.z), prefab.transform.rotation);
    }

    // Update is called once per frame
    void Update() {

    }
}