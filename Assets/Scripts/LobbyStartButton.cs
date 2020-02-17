using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyStartButton : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject floor;
    void ApplyDamage() {
        Destroy(floor);
    }
}
