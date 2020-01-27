using DarkRift;
using UnityEngine;

namespace Network {
    public interface INetworkSendable {
        /// <summary>
        /// Identifier used to match representative instances of this object between the server and its clients.
        /// </summary>
        /// <value>identifier of this instance.</value>
        ushort ID { get; set; }

        /// <summary>
        /// Instantiates a new GameObject from this object.
        /// </summary>
        /// <returns>instantiated gameObject</returns>
        GameObject toGameObject();
    }
}
