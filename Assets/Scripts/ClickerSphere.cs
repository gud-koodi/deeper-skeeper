using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickerSphere : MonoBehaviour
{
    public int id;
    public ClickerSceneManager manager = null;

    void OnMouseDown() {
        if (manager != null) {
            manager.GetComponent<ClickerSceneManager>().GrowBall(this);
        }
    }
}
