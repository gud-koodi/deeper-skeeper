namespace GudKoodi.DeeperSkeeper.Network
{
    /// <summary>
    /// Tag container for messages sent by the server.
    /// </summary>
    public static class ServerMessage
    {
        /// <summary>
        /// Sent to a new client when it connects. Can be deserialized into <see cref="ConnectionData" />.
        /// </summary>
        public const ushort ConnectionData = 0x000_0;

        /// <summary>
        /// Sent when a level is started. Doesn't contain any serialized data.
        /// </summary>
        public const ushort LevelStart = 0x010_0;

        /// <summary>
        /// This code is used by clients to request level start.
        /// </summary>
        public const ushort LevelStartRequest = 0x010_1;

        /// <summary>
        /// Sent when client should create a new player object. Can be deserialized into <see cref="Player" />.
        /// </summary>
        public const ushort CreatePlayer = 0x011_0;

        /// <summary>
        /// Sent when client should update a slave player's data. Can be deserialized into <see cref="Player" />.
        /// </summary>
        public const ushort UpdatePlayer = 0x011_1;

        /// <summary>
        /// Sent when client should remove a player object. Contains <c>ushort</c> of the player id to remove.
        /// </summary>
        public const ushort DeletePlayer = 0x011_F;

        /// <summary>Sent when new enemy should be created.</summary>
        public const ushort CreateEnemy = 0x012_0;

        /// <summary>Sent when existing enemy should be updated.</summary>
        public const ushort UpdateEnemy = 0x012_1;

        /// <summary>Sent when existing enemy should be deleted.</summary>
        public const ushort DeleteEnemy = 0x012_F;
    }
}
