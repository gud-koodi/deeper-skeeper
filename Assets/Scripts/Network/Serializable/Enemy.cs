namespace GudKoodi.DeeperSkeeper.Network
{
    using DarkRift;
    using UnityEngine;

    /// <summary>
    /// Serialization data class for enemy objects.
    /// </summary>
    public class Enemy : INetworkSerializable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Enemy"/> class.
        /// This parameterless constructor is called by DarkRift.
        /// </summary>
        public Enemy()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Enemy"/> class.
        /// </summary>
        /// <param name="networkID">Network identifier.</param>
        /// <param name="currentPosition">This object's current position.</param>
        /// <param name="targetPosition">Position this object is aiming towards.</param>
        public Enemy(ushort networkID, Vector3 currentPosition, ushort targetPosition)
        {
            this.NetworkID = networkID;
            this.CurrentPosition = currentPosition;
            this.Target = targetPosition;
        }

        /// <summary>
        /// Gets or sets the network identifier for this object.
        /// </summary>
        /// <value>Network identifier.</value>
        public ushort NetworkID { get; set; }

        /// <summary>
        /// Gets or sets the current position for this object.
        /// </summary>
        /// <value>Current position.</value>
        public Vector3 CurrentPosition { get; set; }

        /// <summary>
        /// Gets or sets the target position for this object.
        /// </summary>
        /// <value>Target position.</value>
        public ushort Target { get; set; }

        /// <summary>
        /// /// Deserializes data to this instance. Called by DarkRift.
        /// </summary>
        /// <param name="e">DarkRift deserialization event</param>
        public void Deserialize(DeserializeEvent e)
        {
            this.NetworkID = e.Reader.ReadUInt16();
            this.CurrentPosition = SerializationHelper.DeserializeVector3(e.Reader);
            this.Target = e.Reader.ReadUInt16();
        }

        /// <summary>
        /// Serializes data from this instance. Called by DarkRift.
        /// </summary>
        /// <param name="e">DarkRift serialization event</param>
        public virtual void Serialize(SerializeEvent e)
        {
            e.Writer.Write(this.NetworkID);
            SerializationHelper.SerializeVector3(e.Writer, this.CurrentPosition);
            e.Writer.Write(this.Target);
        }
    }
}
