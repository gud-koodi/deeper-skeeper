using UnityEngine;
using Network;


namespace Network
{
    /// <summary>
    /// Utility object for creating new gameobjects from assigned prefabs
    /// </summary>
    [CreateAssetMenu(fileName = "NetworkInstantiator", menuName = "Config/NetworkInstantiator")]
    public class NetworkInstantiator : ScriptableObject
    {
        public GameObject PlayerPrefab;

        internal GameObject InstantiatePlayer(Player player, bool masterObject = false)
        {
            GameObject go = Instantiate(PlayerPrefab, player.Position, Quaternion.identity);
            if (masterObject)
            {
                go.AddComponent<PlayerController>();
                PlayerController pc = go.GetComponent<PlayerController>();
                pc.playerRigidbody = go.GetComponent<Rigidbody>();
                pc.speed = 10f;
            }
            return go;
        }
    }
}
