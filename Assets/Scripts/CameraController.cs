using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject Player;
    private Vector3 offset;
    
    public void SetFollowedPlayer(GameObject player)
    {
        Debug.Log("pls");
        this.Player = player;
        this.offset = transform.position - player.transform.position;
    }
    
    void Awake()
    {
        this.SetFollowedPlayer(gameObject);
    }

    // Update is called after frame
    void LateUpdate()
    {
        transform.position = Player.transform.position + offset;
    }
}