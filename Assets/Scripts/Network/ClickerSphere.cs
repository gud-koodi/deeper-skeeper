using DarkRift;
using UnityEngine;

namespace Network {

    public class ClickerSphere : IDarkRiftSerializable {

        public int ID { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float Scale { get; set; }

        public ClickerSphere() { }

        public void Deserialize(DeserializeEvent e) {
            X = e.Reader.ReadSingle();
            Y = e.Reader.ReadSingle();
            Z = e.Reader.ReadSingle();
            Scale = e.Reader.ReadSingle();
        }

        public void Serialize(SerializeEvent e) {
            e.Writer.Write(X);
            e.Writer.Write(Y);
            e.Writer.Write(Z);
            e.Writer.Write(Scale);
        }

    }

}
