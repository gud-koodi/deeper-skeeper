namespace GudKoodi.DeeperSkeeper.Network
{
    using DarkRift;

    /// <summary>
    /// Serialization data class for sending initial data when a new client connects.
    /// </summary>
    public class ConnectionData : IDarkRiftSerializable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionData"/> class.
        /// This parameterless constructor is provided for DarkRift.
        /// </summary>
        public ConnectionData()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionData"/> class.
        /// </summary>
        /// <param name="clientID">ID of the client.</param>
        /// <param name="playerObjectID">ID oif the player object connected by the client.</param>
        /// <param name="levelSeed">Random seed used when generating the level.</param>
        /// <param name="players">Array of all present player GameObjects.</param>
        public ConnectionData(ushort clientID, Enemy[] enemies, ushort playerObjectID, int levelSeed, Player[] players)
        {
            this.ClientID = clientID;
            this.Enemies = enemies;
            this.PlayerObjectID = playerObjectID;
            this.LevelSeed = levelSeed;
            this.Players = players;
        }

        /// <summary>
        /// Gets or sets the client identifier provided by this object.
        /// </summary>
        /// <value>Client identifier.</value>
        public ushort ClientID { get; set; }

        /// <summary>
        /// Gets or sets an array containing all player GameObjects on the server.
        /// </summary>
        /// <value></value>
        public Enemy[] Enemies { get; set; }

        /// <summary>
        /// Gets or sets the network identifier for player GameObject controlled by this client.
        /// </summary>
        /// <value>Player GameObject network identifier.</value>
        public ushort PlayerObjectID { get; set; }

        /// <summary>
        /// Gets or sets an array containing all player GameObjects on the server.
        /// </summary>
        /// <value></value>
        public Player[] Players { get; set; }

        /// <summary>
        /// Gets or sets the random seed used for generating levels.
        /// </summary>
        /// <value>Random seed.</value>
        public int LevelSeed { get; set; }

        /// <summary>
        /// /// Deserializes data to this instance. Called by DarkRift.
        /// </summary>
        /// <param name="e">DarkRift deserialization event.</param>
        public void Deserialize(DeserializeEvent e)
        {
            this.ClientID = e.Reader.ReadUInt16();
            this.Enemies = e.Reader.ReadSerializables<Enemy>();
            this.PlayerObjectID = e.Reader.ReadUInt16();
            this.LevelSeed = e.Reader.ReadInt32();
            this.Players = e.Reader.ReadSerializables<Player>();
        }

        /// <summary>
        /// Serializes data from this instance. Called by DarkRift.
        /// </summary>
        /// <param name="e">DarkRift serialization event</param>
        public void Serialize(SerializeEvent e)
        {
            e.Writer.Write(this.ClientID);
            e.Writer.Write(this.Enemies);
            e.Writer.Write(this.PlayerObjectID);
            e.Writer.Write(this.LevelSeed);
            e.Writer.Write(this.Players);
        }
    }
}
