using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    public NetworkConfig networkConfig;

    /// <summary>
    /// Start the game without host configuration.
    /// </summary>
    public void Connect() {
        networkConfig.isHost = false;
        LoadScene();
    }

    /// <summary>
    /// Start the game with host configuration.
    /// </summary>
    public void Host() {
        networkConfig.isHost = true;
        LoadScene();
    }

    /// <summary>
    /// Quits the game.
    /// </summary>
    public void Quit() {
        Application.Quit();
    }

    private void LoadScene() {
        Debug.Log("Starting the game as " + ((networkConfig.isHost) ? "host" : "client"));
        SceneManager.LoadScene("ClickerTest", LoadSceneMode.Single);
    }

}
