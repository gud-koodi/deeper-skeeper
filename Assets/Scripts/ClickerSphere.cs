using System.Collections;
using System.Collections.Generic;
using DarkRift;
using UnityEngine;

public class ClickerSphere : MonoBehaviour, INetworkSyncable {

    public int id;

    public void Read(DarkRiftReader reader) {
        transform.localScale = reader.ReadSingle() * Vector3.one;
    }

    public void Write(DarkRiftWriter writer) {
        // All scales are assumed to be same
        writer.Write(transform.localScale.x);
    }

}
