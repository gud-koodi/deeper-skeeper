namespace Network
{
    using System;
    using System.Collections.Generic;
    using DarkRift;
    using UnityEngine;

    /// <summary>
    /// Collection for managing Player network objects.
    /// </summary>
    internal class PlayerList
    {
        /// <summary>
        /// Instantiator used for creating new Player-GameObjects.
        /// </summary>
        private readonly NetworkInstantiator instantiator;

        /// <summary>
        /// List of managed GameObjects.
        /// </summary>
        private readonly NetworkObjectList playerObjects;

        /// <summary>
        /// Serializable data for managed GameObjects.
        /// </summary>
        private readonly List<Player> players;

        /// <summary>
        /// Creates new instance of PlayerList with given NetworkInstantiator.
        /// </summary>
        /// <param name="instantiator">Instantiator used for creating new Player-GameObjects.</param>
        public PlayerList(NetworkInstantiator instantiator)
        {
            this.instantiator = instantiator;
            this.playerObjects = new NetworkObjectList();
            this.players = new List<Player>();
        }

        /// <summary>
        /// Creates and starts managing a new GameObject using given serialization data.
        /// </summary>
        /// <param name="player">Unserialized data used in creation.</param>
        /// <param name="isMasterObject">Whether or not this object is controlled locally.</param>
        /// <returns>The created GameObject.</returns>
        public GameObject Create(Player player, bool isMasterObject = false)
        {
            GameObject go = null;
            if (this.playerObjects.IsVacant(player.NetworkID))
            {
                go = instantiator.InstantiatePlayer(player, isMasterObject);
                this.playerObjects[player.NetworkID] = go;
                this.players.Add(player);
                Debug.Log($"Assigned network ID {player.NetworkID}  to new Player instance.");
            }
            else
            {
                Debug.LogError($"Network ID {player.NetworkID} was already present.");
            }
            return go;
        }

        /// <summary>
        /// Interprets the given message as Player data and forwards it to the existing representative GameObject.
        /// </summary>
        /// <param name="message">Message to deserialize.</param>
        public void DeserializeUpdate(Message message)
        {
            Player player = message.Deserialize<Player>();
            GameObject go = this.playerObjects[player.NetworkID];
            NetworkSlave slave = go.GetComponent<NetworkSlave>();
            slave.targetPosition = player.Position;
            int index = this.players.FindIndex(p => p.NetworkID == player.NetworkID);
            this.players[index] = player;
        }

        /// <summary>
        /// Removes the GameObject with specified network id from this list and destroys it.
        /// </summary>
        /// <param name="playerId"></param>
        public void Destroy(ushort playerId)
        {
            if (!this.playerObjects.IsVacant(playerId))
            {
                GameObject go = this.playerObjects.RemoveAt(playerId);
                Debug.Log($"Removing {go} at network id {playerId}");
                GameObject.Destroy(go);
            }
            else
            {
                Debug.LogError($"Trying to remove player ID {playerId} but was not found");
            }
            int count = this.players.RemoveAll(p => p.NetworkID == playerId);
            if (count != 1)
            {
                throw new Exception("TODO: Write error message");
            }
        }

        /// <summary>
        /// Updates the serialized data of the given object and writes it to a Message with given tag.
        /// </summary>
        /// <param name="playerObject">Object for serialization.</param>
        /// <param name="tag">Tag for the message.</param>
        /// <returns>Message with given tag and serialized data for object.</returns>
        public Message SerializeUpdate(GameObject playerObject, ushort tag)
        {
            ushort id = this.playerObjects.LookUpNetworkID(playerObject);
            Player player = this.players.Find(p => p.NetworkID == id);
            player.Position = playerObject.transform.localPosition;
            return Message.Create(tag, player);
        }

        /// <summary>
        /// Returns an array presentation of all serializable player data.
        /// </summary>
        /// <returns>Array of serializable players.</returns>
        public Player[] ToArray()
        {
            // TODO: Handle serialization of multiple objects and replace this method with that.
            return players.ToArray();
        }
    }
}
