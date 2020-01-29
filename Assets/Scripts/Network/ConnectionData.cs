using DarkRift;
using UnityEngine;

namespace Network
{
    public class ConnectionData : IDarkRiftSerializable
    {

        public ushort ID { get; set; }

        public Player[] Players { get; set; }

        public ConnectionData() { }

        public ConnectionData(ushort id, Player[] players)
        {
            this.ID = id;
            this.Players = players;
        }

        public GameObject toGameObject()
        {
            throw new System.NotImplementedException();
        }

        public void Deserialize(DeserializeEvent e)
        {
            this.ID = e.Reader.ReadUInt16();
            this.Players = e.Reader.ReadSerializables<Player>();
        }

        public void Serialize(SerializeEvent e)
        {
            e.Writer.Write(ID);
            e.Writer.Write(Players);
        }
    }
}
