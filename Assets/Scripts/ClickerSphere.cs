using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkRift;

public class ClickerSphere : MonoBehaviour, INetworkSyncable {
    public int id;
    public ClickerSceneManager manager = null;

    void OnMouseDown() {
        if (manager != null) {
            manager.GetComponent<ClickerSceneManager>().GrowBall(this);
        }
    }

    public void Read(DarkRiftReader reader) {
        transform.localScale = reader.ReadSingle() * Vector3.one;
    }

    public void Write(DarkRiftWriter writer) {
        // All scales are assumed to be same
        writer.Write(transform.localScale.x);
    }

}
