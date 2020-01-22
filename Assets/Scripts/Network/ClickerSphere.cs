using DarkRift;
using UnityEngine;

namespace Network {

    /// <summary>
    /// Network-serializable class representing ClickerSphere GameObject.
    /// </summary>
    public class ClickerSphere : IDarkRiftSerializable {

        public int ID { get; set; }
        public Vector3 Position { get; set; }
        public float Scale { get; set; }

        /// <summary>
        /// Creates a new instance with <c>ID = -1</c>, <c>Scale = 1</c>, and origin position.
        /// </summary>
        /// <returns></returns>
        public ClickerSphere() : this(-1, Vector3.zero, 1f) { }

        /// <summary>
        /// Creates a new instance with given values and <c>Scale = 1</c>.
        /// </summary>
        /// <param name="ID">id of this object</param>
        /// <param name="position">local position in gameworld</param>
        /// <returns></returns>
        public ClickerSphere(int ID, Vector3 position) : this(ID, position, 1f) { }

        /// <summary>
        /// Creates a new instance with given values.
        /// </summary>
        /// <param name="ID">id of this object</param>
        /// <param name="position">local position in gameworld</param>
        /// <param name="scale">scale of this object</param>
        public ClickerSphere(int ID, Vector3 position, float scale) {
            this.ID = ID;
            this.Position = position;
            this.Scale = scale;
        }

        /// <summary>
        /// Deserializes data to this instance. Called by DarkRift.
        /// </summary>
        /// <param name="e">DarkRift deserialization event</param>
        public void Deserialize(DeserializeEvent e) {
            DarkRiftReader reader = e.Reader;
            ID = reader.ReadInt32();
            Position = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            Scale = reader.ReadSingle();
        }

        /// <summary>
        /// Serializes data from this instance.null Called by DarkRift.
        /// </summary>
        /// <param name="e">DarkRift serialization event</param>
        public void Serialize(SerializeEvent e) {
            e.Writer.Write(ID);
            e.Writer.Write(Position.x);
            e.Writer.Write(Position.y);
            e.Writer.Write(Position.z);
            e.Writer.Write(Scale);
        }

    }

}
