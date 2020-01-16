using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    // Start is called before the first frame update
    public Rigidbody playerRigidbody;
    public float speed = 10.0f;
    void Start () {

    }

    // Update is called once per frame
    void Update () {

        float mH = Input.GetAxis ("Horizontal");
        float mV = Input.GetAxis ("Vertical");
        playerRigidbody.velocity = new Vector3 (mH * speed, playerRigidbody.velocity.y, mV * speed);

    }
}