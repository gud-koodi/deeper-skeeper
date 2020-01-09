using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Server : MonoBehaviour
{
    // Start is called before the first frame update
    void Start() {
        print("Load scene");
        SceneManager.LoadScene("DragDemo", LoadSceneMode.Additive);
    }
}
