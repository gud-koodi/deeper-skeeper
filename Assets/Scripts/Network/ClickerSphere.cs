using DarkRift;
using UnityEngine;

namespace Network {

    public class ClickerSphere : IDarkRiftSerializable {

        public int ID { get; set; }
        public Vector3 Position { get; set; }
        public float Scale { get; set; }

        public ClickerSphere() : this(-1, Vector3.zero) { }

        public ClickerSphere(int ID, Vector3 position) : this(ID, position, 1f) { }

        public ClickerSphere(int ID, Vector3 position, float scale) {
            this.ID = ID;
            this.Position = position;
            this.Scale = scale;
        }

        public void Deserialize(DeserializeEvent e) {
            DarkRiftReader reader = e.Reader;
            ID = reader.ReadInt32();
            Position = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            Scale = reader.ReadSingle();
        }

        public void Serialize(SerializeEvent e) {
            e.Writer.Write(ID);
            e.Writer.Write(Position.x);
            e.Writer.Write(Position.y);
            e.Writer.Write(Position.z);
            e.Writer.Write(Scale);
        }

    }

}
