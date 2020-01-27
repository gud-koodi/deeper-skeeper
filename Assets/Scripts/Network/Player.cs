using DarkRift;
using UnityEngine;

namespace Network {

    public class Player : INetworkSendable, IDarkRiftSerializable {
        /// <summary>
        /// Sets the Unity GameObject prefab that the extended class should represent.
        /// </summary>
        /// <para>
        /// By convention, set this value to a private static field in the extended class.
        /// This solution is a bit iffy, but should make accessing references from the Unity editor easier.
        /// </para>
        /// <value>Reference to the represented prefab</value>
        internal static GameObject Prefab { get; set; }

        public ushort ID { get; set; }
        public ushort clientID { get; set; }
        public Vector3 Position { get; set; }

        /// <summary>
        /// Parameterless constructor for DarkRift serialization.
        /// </summary>
        public Player() { }

        public Player(ushort id, ushort clientID, Vector3 position) {
            this.ID = id;
            this.clientID = clientID;
            this.Position = position;
        }

        public GameObject toGameObject() {
            GameObject go = GameObject.Instantiate(Prefab, Position, Quaternion.identity);
            return go;
        }

        /// <summary>
        /// Deserializes data to this instance. Called by DarkRift.
        /// </summary>
        /// <param name="e">DarkRift deserialization event</param>
        public void Deserialize(DeserializeEvent e) {
            DarkRiftReader reader = e.Reader;
            this.ID = reader.ReadUInt16();
            this.Position = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
        }

        /// <summary>
        /// Serializes data from this instance. Called by DarkRift.
        /// </summary>
        /// <param name="e">DarkRift serialization event</param>
        public virtual void Serialize(SerializeEvent e) {
            DarkRiftWriter writer = e.Writer;
            writer.Write(ID);
            writer.Write(Position.x);
            writer.Write(Position.y);
            writer.Write(Position.z);
        }
    }
}