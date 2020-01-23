using DarkRift;
using UnityEngine;

namespace Network {

    public class Player : IDarkRiftSerializable {

        public int ID { get; set; }

        public Vector3 Position { get; set; }

        public Player() : this(-1, Vector3.one) { }

        public Player(int id, Vector3 position) {
            this.ID = id;
            this.Position = position;
        }

        public void Deserialize(DeserializeEvent e) {
            DarkRiftReader reader = e.Reader;
            this.ID = reader.ReadInt32();
            Position = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
        }

        public void Serialize(SerializeEvent e) {
            DarkRiftWriter writer = e.Writer;
            writer.Write(ID);
            writer.Write(Position.x);
            writer.Write(Position.y);
            writer.Write(Position.z);
        }
        
    }

}
