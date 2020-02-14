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
        /// Sent when server should update player controlled by client.
        /// Can be deserialized into <c>Player</c>.
        /// </summary>
        public const ushort UpdatePlayer = ServerMessage.UpdatePlayer;
    }
}
