using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject Player;
    private Vector3 offset;
    
    /// <summary>
    /// Sets wich player to follow.
    /// </summary>
    /// <param name="player">Player GameObject</param>
    public void SetFollowedPlayer(GameObject player)
    {
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