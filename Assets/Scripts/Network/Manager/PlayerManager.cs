namespace GudKoodi.DeeperSkeeper.Network
{
    using UnityEngine;
    using Event;
    using DeeperSkeeper.Player;
    using Weapon;

    internal class PlayerManager : ObjectManager<Player>
    {
        /// <summary>
        /// Event called after a master object has been created.
        /// </summary>
        private readonly ObjectCreated masterPlayerCreated;

        /// <summary>
        /// Event called when a master object requests an update to network.
        /// </summary>
        private readonly ObjectUpdateRequested objectUpdateRequested;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerManager"/> class.
        /// </summary>
        /// <param name="masterPlayerCreated">Event called after a master object has been created.</param>
        /// <param name="objectUpdateRequested">Event called when a master object requests an update to network.</param>
        public PlayerManager(ObjectCreated masterPlayerCreated, ObjectUpdateRequested objectUpdateRequested) : base()
        {
            this.masterPlayerCreated = masterPlayerCreated;
            this.objectUpdateRequested = objectUpdateRequested;
        }

        /// <summary>
        /// Instantiates a new GameObject and gives it proper components for local controlling.
        /// </summary>
        /// <param name="prefab">Prefab used as a base for the new GameObject.</param>
        /// <param name="player">Serialization data used in creation.</param>
        /// <returns>Instantiated GameObject.</returns>
        protected override GameObject InstantiateMaster(GameObject prefab, Player player)
        {
            GameObject go = GameObject.Instantiate(prefab, player.CurrentPosition, Quaternion.identity);

            go.AddComponent<PlayerController>();
            PlayerController pc = go.GetComponent<PlayerController>();
            pc.playerRigidbody = go.GetComponent<Rigidbody>();
            pc.weapon = go.GetComponentInChildren<Weapon>();

            NetworkMaster master = go.AddComponent<NetworkMaster>();
            master.ObjectUpdateRequested = this.objectUpdateRequested;

            // TODO: Do this call elsewhere, but for now having a direct access to its reference is sooo niice.
            this.masterPlayerCreated.Trigger(go);
            return go;
        }

        /// <summary>
        /// Instantiates a new GameObject and gives it proper components for remote controlling.
        /// </summary>
        /// <param name="prefab">Prefab used as a base for the new GameObject.</param>
        /// <param name="player">Serialization data used in creation.</param>
        /// <returns>Instantiated GameObject.</returns>
        protected override GameObject InstantiateSlave(GameObject prefab, Player player)
        {
            GameObject go = GameObject.Instantiate(prefab, player.CurrentPosition, Quaternion.identity);

            NetworkSlave slave = go.AddComponent<NetworkSlave>();
            slave.Rigidbody = go.GetComponent<Rigidbody>();
            slave.UpdateState(player);
            return go;
        }

        /// <summary>
        /// Deserializes the state to the GameObject from <see cref="Player" /> object.
        /// </summary>
        /// <param name="data">Data to deserialize from.</param>
        /// <param name="gameObject">GameObject to update.</param>
        protected override void DeserializeState(Player player, GameObject gameObject)
        {
            NetworkSlave slave = gameObject.GetComponent<NetworkSlave>();
            slave.UpdateState(player);
        }

        /// <summary>
        /// Serializes the GameObject to a <see cref="Player" /> object.
        /// </summary>
        /// <param name="data">Data to serialize into.</param>
        /// <param name="gameObject">GameObject to serialize from.</param>
        protected override void SerializeState(Player player, GameObject gameObject)
        {
            player.CurrentPosition = gameObject.transform.localPosition;
            player.Rotation = gameObject.GetComponent<Rigidbody>().rotation.eulerAngles.y;
            player.TargetPosition = gameObject.transform.localPosition;
        }
    }
}
