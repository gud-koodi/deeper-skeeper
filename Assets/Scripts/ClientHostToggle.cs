using DarkRift.Client.Unity;
using DarkRift.Server.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClientHostToggle : MonoBehaviour {
    public bool isHost = true;

    // Start is called before the first frame update
    void Start() {
        if (isHost) {
            print("Host Server");
            SceneManager.LoadScene("ServerScene", LoadSceneMode.Single);
        } else {
            print("Load Scene");
            SceneManager.LoadScene("DragDemo", LoadSceneMode.Single);
        }
    }

    // Update is called once per frame
    void Update() {
        
    }
}
