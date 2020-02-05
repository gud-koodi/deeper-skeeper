using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Component for setting the position of camera during game
/// </summary>
public class CameraController : MonoBehaviour
{
    /// <summary>
    /// Distance from followed object.
    /// </summary>
    public float Distance = 25f;

    /// <summary>
    /// Strategy for updating the camera position.
    /// </summary>
    private UpdateStrategy updateStrategy = () => { };

    /// <summary>
    /// Strategy for updating the camera position.
    /// </summary>
    private delegate void UpdateStrategy();

    /// <summary>
    /// Sets the followed <see cref="GameObject" />
    /// </summary>
    /// <param name="player">Player GameObject</param>
    public void SetFollowedPlayer(GameObject player)
    {
        Vector3 direction = transform.rotation * Vector3.forward;
        this.updateStrategy = () =>
        {
            transform.position = player.transform.position - (Distance * direction);
        };
    }
    
    void LateUpdate()
    {
        updateStrategy();
    }
}
