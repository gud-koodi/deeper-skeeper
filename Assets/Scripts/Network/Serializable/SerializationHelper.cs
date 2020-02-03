namespace Network
{
    using DarkRift;
    using UnityEngine;

    /// <summary>
    /// Container class for helping with serialization of Unity datatypes.
    /// </summary>
    public static class SerializationHelper
    {
        /// <summary>
        /// Deserializes a <see cref="Vector3" /> from given <see cref="DarkRiftWriter" />.
        /// </summary>
        /// <param name="reader">Reader to read from.</param>
        /// <returns>Deserialized vector.</returns>
        public static Vector3 DeserializeVector3(DarkRiftReader reader)
        {
            return new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
        }

        /// <summary>
        /// Serializes <see cref="Vector3" /> into <see cref="DarkRiftWriter" />.
        /// </summary>
        /// <param name="writer">Writer to write into.</param>
        /// <param name="vector">Vector to serialize.</param>
        public static void SerializeVector3(DarkRiftWriter writer, Vector3 vector)
        {
            writer.Write(vector.x);
            writer.Write(vector.y);
            writer.Write(vector.z);
        }
    }
}