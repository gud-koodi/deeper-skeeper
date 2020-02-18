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
    /// Strategy for casting from camera.
    /// </summary>
    private CastStrategy castStrategy = () => { };

    private List<MeshRenderer> disabledRenderers = new List<MeshRenderer>();

    /// <summary>
    /// Strategy for updating the camera position.
    /// </summary>
    private delegate void UpdateStrategy();

    /// <summary>
    /// Strategy for casting from camera and what to do.
    /// </summary>
    private delegate void CastStrategy();

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

        this.castStrategy = () =>
        {
            RaycastHit hit;
            if (Physics.Linecast(transform.position, player.transform.position, out hit))
            {
                if (hit.collider.gameObject.tag == "Wall")
                {
                    MeshRenderer meshRenderer = hit.collider.gameObject.GetComponent<MeshRenderer>();
                    meshRenderer.enabled = false;
                    disabledRenderers.Add(meshRenderer);
                    MeshRenderer edgeMeshRenderer = GetMeshRenderFromChild(meshRenderer.gameObject);
                    edgeMeshRenderer.enabled = true;
                }
            }
        };
    }

    void LateUpdate()
    {
        updateStrategy();
    }

    void Update()
    {
        ReEnableDisabledMeshRenderers();
        castStrategy();
    }

    private void ReEnableDisabledMeshRenderers()
    {
        disabledRenderers.ForEach(p =>
        {
            GetMeshRenderFromChild(p.gameObject).enabled = false;
            p.enabled = true;
        });
        disabledRenderers.Clear();
    }

    private MeshRenderer GetMeshRenderFromChild(GameObject gameObject)
    {
        MeshRenderer[] childMeshRenderers = gameObject.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer renderer in childMeshRenderers)
        {
            if (!renderer.gameObject.Equals(gameObject))
            {
                return renderer;
            }
        }
        return null;
    }
}
