using DarkRift;

/// <summary>
/// Interface for reading and writing object data over network using DarkRift.
/// </summary>
/// <remarks>
/// <para>Implementation MUST always write the exact same amount of data as will be read.</para>
/// <para>
/// All data that may change during a gameObject's lifetime should be written
/// unless derived from other values.
/// </para>
/// </remarks>
public interface INetworkSyncable{
    /// <summary>
    /// Synchronize with network using data from reader.
    /// </summary>
    /// <param name="reader"><c>DarkRiftReader</c> instance to read from.</param>
    void Read(DarkRiftReader reader);

    /// <summary>
    /// Write mutable data into writer.
    ///</summary>
    /// <param name="writer"><c>DarkRiftWriter</c> instance to write into.</param>
    void Write(DarkRiftWriter writer);
}
