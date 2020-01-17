using System.Collections;
using System.Collections.Generic;
using Network;
using UnityEngine;

public class ClickerMouse : MonoBehaviour {

    public ClickerSphereSpawner spawner;

    // Update is called once per frame
    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (!Physics.Raycast(ray, float.PositiveInfinity)) {
                Vector3 position = ray.GetPoint(10);
                Network.ClickerSphere sphere = new Network.ClickerSphere();
                sphere.ID = -1;
                sphere.X = position.x;
                sphere.Y = position.y;
                sphere.Z = position.z;
                sphere.Scale = 1;

                spawner.CreateSphere(sphere);
            }
        }
    }
}
