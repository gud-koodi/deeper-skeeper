using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkRift;

public class DarkriftWrapper : MonoBehaviour
{
    // Start is called before the first frame update
    public DarkRiftWriter MyDarkriftWriter { get; set; }

    public void initDarkriftWriter()
    {
    }
    void Start()
    {
        Debug.Log("AWAKE");
        using (DarkRiftWriter writer = DarkRiftWriter.Create()) {}
        

    }

    // Update is called once per frame
    void Update()
    {

    }
}
