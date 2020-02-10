namespace GudKoodi.DeeperSkeeper.Network
{
    using DarkRift;

    public interface INetworkSerializable : IDarkRiftSerializable
    {
        ushort NetworkID { get; set; }
    }
}
