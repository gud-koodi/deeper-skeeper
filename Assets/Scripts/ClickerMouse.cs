using UnityEngine;

public class ClickerMouse : MonoBehaviour
{

    public ClickerSphereSpawner spawner;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, float.PositiveInfinity))
            {
                GameObject go = hit.transform.gameObject;
                go.transform.localScale += 0.1f * Vector3.one;
                spawner.SendUpdate(go); // Unchecked object type
            }
            else
            {
                Vector3 position = ray.GetPoint(10);
                spawner.Spawn(position);
            }
        }
    }
}
