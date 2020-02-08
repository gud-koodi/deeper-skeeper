namespace Network
{
    using DarkRift;

    public interface INetworkSerializable : IDarkRiftSerializable
    {
        ushort NetworkID { get; set; }
    }
}
