using DarkRift;
using UnityEngine;

namespace Network
{
    public class ConnectionData : IDarkRiftSerializable
    {

        public ushort ClientID { get; set; }
        public ushort PlayerObjectID { get; set; }

        public Player[] Players { get; set; }

        public ConnectionData() { }

        public ConnectionData(ushort clientID, ushort playerObjectID, Player[] players)
        {
            this.ClientID = clientID;
            this.PlayerObjectID = playerObjectID;
            this.Players = players;
        }

        public void Deserialize(DeserializeEvent e)
        {
            this.ClientID = e.Reader.ReadUInt16();
            this.PlayerObjectID = e.Reader.ReadUInt16();
            this.Players = e.Reader.ReadSerializables<Player>();
        }

        public void Serialize(SerializeEvent e)
        {
            e.Writer.Write(ClientID);
            e.Writer.Write(PlayerObjectID);
            e.Writer.Write(Players);
        }
    }
}
