using System.Collections.Generic;
using UnityEngine;
using System;
using DarkRift;

namespace Network
{
    internal class PlayerList
    {
        private readonly NetworkInstantiator instantiator;
        private readonly NetworkObjectList playerObjects;
        private List<Player> players;

        public PlayerList(NetworkInstantiator instantiator)
        {
            this.instantiator = instantiator;
            this.playerObjects = new NetworkObjectList();
            this.players = new List<Player>();
        }

        public GameObject Create(Player player, bool isMasterObject = false)
        {
            GameObject go = null;
            if (playerObjects.IsVacant(player.NetworkID))
            {
                go = instantiator.InstantiatePlayer(player, isMasterObject);
                playerObjects[player.NetworkID] = go;
                players.Add(player);
                Debug.Log($"Assigned network ID {player.NetworkID}  to new Player instance.");
            }
            else
            {
                Debug.LogError($"Network ID {player.NetworkID} was already present.");
            }
            return go;
        }

        public Message SerializeUpdate(GameObject playerObject)
        {
            ushort id = playerObjects.LookUpNetworkID(playerObject);
            Player player = players.Find(p => p.NetworkID == id);
            player.Position = playerObject.transform.localPosition;
            foreach (var p in players) {
                Debug.Log(p.Position);
            }
            return Message.Create((ushort) ResponseTag.UPDATE_PLAYER, player);
        }

        public void DeserializeUpdate(Message message)
        {
            Player player = message.Deserialize<Player>();
            GameObject go = playerObjects[player.NetworkID];
            NetworkSlave slave = go.GetComponent<NetworkSlave>();
            slave.targetPosition = player.Position;
            int index = players.FindIndex(p => p.NetworkID == player.NetworkID);
            players[index] = player;
        }

        public void Remove(ushort playerId) {
            if (!playerObjects.IsVacant(playerId))
            {
                GameObject go = playerObjects.RemoveAt(playerId);
                Debug.Log($"Removing {go} at network id {playerId}");
                GameObject.Destroy(go);
            }
            else
            {
                Debug.LogError($"Trying to remove player ID {playerId} but was not found");
            }
            int count = players.RemoveAll(p => p.NetworkID == playerId);
            if (count != 1) {
                throw new Exception("TODO: Write error message");
            }
        }

        public Player[] ToArray()
        {
            return players.ToArray();
        }
    }
}
