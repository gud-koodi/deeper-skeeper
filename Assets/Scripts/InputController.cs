namespace GudKoodi.DeeperSkeeper
{
    using UnityEngine;
    using UnityEngine.SceneManagement;

    /// <summary>
    /// Component for global input handling.
    /// </summary>
    public class InputController : MonoBehaviour
    {
        void Update()
        {
            if (Input.GetButtonDown("Cancel"))
            {
                SceneManager.LoadScene(0);
            }
        }
    }
}
