namespace GudKoodi.DeeperSkeeper.Network
{
    /// <summary>
    /// Tag container for messages sent by a client.
    /// </summary>
    public static class ClientMessage
    {
        // Don't tell anyone, but we cheat a bit here and use some shared codes.
        // This allows us to simply forward the same message also to other clients.

        /// <summary>
        /// Sent when the level should start.
        /// </summary>
        public const ushort LevelStartRequest = ServerMessage.LevelStartRequest;

        /// <summary>
        /// Sent when server should update player controlled by client. Can be deserialized into <see cref="Player" />.
        /// </summary>
        public const ushort UpdatePlayer = ServerMessage.UpdatePlayer;

        /// <summary>Sent when client should remove a player object. Contains <c>ushort</c> of the player id to remove.</summary>
        public const ushort DeletePlayer = ServerMessage.DeletePlayer;

        /// <summary>Sent when existing enemy should be deleted.</summary>
        public const ushort DeleteEnemy = ServerMessage.DeleteEnemy;
    }
}
