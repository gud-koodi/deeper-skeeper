namespace GudKoodi.DeeperSkeeper.Network
{
    using DarkRift;
    using UnityEngine;

    /// <summary>
    /// Serialization data class for player GameObjects.
    /// </summary>
    public class Player : INetworkSerializable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Player"/> class.
        /// This parameterless constructor is called by DarkRift.
        /// </summary>
        public Player()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Player"/> class.
        /// Target position is set to be the same as current position.
        /// </summary>
        /// <param name="networkID">Network identifier.</param>
        /// <param name="currentPosition">This object's current position.</param>
        /// <param name="rotation">Y-plane rotation of this objet in radians.</param>
        public Player(ushort networkID, Vector3 currentPosition, float rotation)
            : this(networkID, currentPosition, rotation, currentPosition)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Player"/> class.
        /// </summary>
        /// <param name="networkID">Network identifier.</param>
        /// <param name="currentPosition">This object's current position.</param>
        /// <param name="rotation">Direction this object is looking at in radians.</param>
        /// <param name="targetPosition">Position this object is aiming towards.</param>
        public Player(ushort networkID, Vector3 currentPosition, float rotation, Vector3 targetPosition)
        {
            this.NetworkID = networkID;
            this.CurrentPosition = currentPosition;
            this.Rotation = rotation;
            this.TargetPosition = targetPosition;
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
        /// Gets or sets the rotation for this object.
        /// </summary>
        /// <value>Y-plane rotation in radians.</value>
        public float Rotation { get; set; }

        /// <summary>
        /// Gets or sets the target position for this object.
        /// </summary>
        /// <value>Target position.</value>
        public Vector3 TargetPosition { get; set; }

        /// <summary>
        /// /// Deserializes data to this instance. Called by DarkRift.
        /// </summary>
        /// <param name="e">DarkRift deserialization event</param>
        public void Deserialize(DeserializeEvent e)
        {
            this.NetworkID = e.Reader.ReadUInt16();
            this.CurrentPosition = SerializationHelper.DeserializeVector3(e.Reader);
            this.Rotation = e.Reader.ReadSingle();
            this.TargetPosition = SerializationHelper.DeserializeVector3(e.Reader);
        }

        /// <summary>
        /// Serializes data from this instance. Called by DarkRift.
        /// </summary>
        /// <param name="e">DarkRift serialization event</param>
        public virtual void Serialize(SerializeEvent e)
        {
            e.Writer.Write(this.NetworkID);
            SerializationHelper.SerializeVector3(e.Writer, this.CurrentPosition);
            e.Writer.Write(this.Rotation);
            SerializationHelper.SerializeVector3(e.Writer, this.TargetPosition);
        }
    }
}
