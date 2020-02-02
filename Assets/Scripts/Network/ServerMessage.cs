namespace Network
{
    /// <summary>
    /// Tag container for messages sent by the server.
    /// </summary>
    public static class ServerMessage
    {
        /// <summary>
        /// Sent to a new client when it connects.
        /// Can be deserialized into <c>ConnectionData</c>.
        /// </summary>
        public const ushort ConnectionData = 0x000_0;

        /// <summary>
        /// Sent when client should create a new player object.
        /// Can be deserialized into <c>Player</c>.
        /// </summary>
        public const ushort CreatePlayer = 0x001_0;

        /// <summary>
        /// Sent when client should update a slave player's data.
        /// Can be deserialized into <c>Player</c>.
        /// </summary>
        public const ushort UpdatePlayer = 0x001_1;

        /// <summary>
        /// Sent when client should remove a player object.
        /// Contains <c>ushort</c> of the player id to remove.
        /// </summary>
        public const ushort DeletePlayer = 0x001_F;
    }
}
