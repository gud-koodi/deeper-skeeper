﻿namespace GudKoodi.DeeperSkeeper.Network
{
    using System;
    using DarkRift;
    using DarkRift.Client;
    using DarkRift.Client.Unity;
    using GudKoodi.DeeperSkeeper.Event;
    using UnityEngine;

    using EnemyList = GudKoodi.DeeperSkeeper.Enemy.EnemyList;

    /// <summary>
    /// Component that handles all communication to server.
    /// </summary>
    public class ClientController : MonoBehaviour
    {
        [Tooltip("The server component this script will communicate with.")]
        public UnityClient Client;

        [Tooltip("Instantiator used to create objects")]
        public NetworkInstantiator NetworkInstantiator;

        [Tooltip("Network configuration to read check host status from.")]
        public NetworkConfig NetworkConfig;

        /// <summary>
        /// Level generation request event.
        /// </summary>
        [Tooltip("Level generation request event")]
        public LevelGenerationRequested LevelGenerationRequested;

        /// <summary>
        /// Network Events container.
        /// </summary>
        [Tooltip("Network Events container.")]
        public NetworkEvents NetworkEvents;

        public EnemyList EnemyList;

        private PlayerManager players;

        private EnemyManager enemies;

        /// <summary>
        /// Sends changed local data to server.
        /// </summary>
        /// <param name="gameObject">Object to send.</param>
        /// <param name="objectType">Type of object.</param>
        public void SendObject(GameObject gameObject, ObjectType objectType)
        {
            // TODO: Distinguish between different network objects
            using (Message message = this.players.UpdateAndSerialize(gameObject, ClientMessage.UpdatePlayer))
            {
                this.Client.SendMessage(message, SendMode.Unreliable);
            }
        }

        void Awake()
        {
            if (this.NetworkConfig.isHost)
            {
                Debug.Log("Client manager shutting up.");
                gameObject.SetActive(false);
                return;
            }
            this.players = new PlayerManager(this.NetworkEvents.AttackStarted, this.NetworkInstantiator.MasterPlayerCreated, this.NetworkInstantiator.PlayerUpdateRequested);
            this.enemies = new EnemyManager(players);
            if (this.Client != null)
            {
                this.Client.MessageReceived += OnResponse;
                this.NetworkEvents.LevelStartRequested.Subscribe(this.LevelStartRequested);
                this.NetworkEvents.ObjectDestructionRequested.Subscribe(DestroyObject);
                this.NetworkEvents.AttackStarted.Subscribe(AttackStarted);
            }
        }

        private void OnResponse(object sender, MessageReceivedEventArgs e)
        {
            // TODO: use command pattern
            switch (e.Tag)
            {
                case ServerMessage.ConnectionData:
                    SetupServerData(e);
                    break;
                case ServerMessage.LevelStart:
                    NetworkEvents.LevelStarted.Trigger();
                    break;
                case ServerMessage.CreatePlayer:
                    CreatePlayer(e);
                    break;
                case ServerMessage.UpdatePlayer:
                    UpdatePlayer(e);
                    break;
                case ServerMessage.PlayAttackPlayer:
                    PlayAttackPlayer(e);
                    break;
                case ServerMessage.DeletePlayer:
                    DeleteObject(e, ObjectType.Player);
                    break;
                case ServerMessage.UpdateEnemy:
                    UpdateEnemy(e);
                    break;
                case ServerMessage.DeleteEnemy:
                    DeleteObject(e, ObjectType.Enemy);
                    break;
            }
        }

        private ushort UnwrapID(MessageReceivedEventArgs e)
        {
            ushort id = 0;
            using (Message message = e.GetMessage())
            using (DarkRiftReader reader = message.GetReader())
            {
                id = reader.ReadUInt16();
            }

            return id;
        }

        private void AttackStarted(GameObject gameObject, ObjectType arg2, object arg3, object arg4)
        {
            ushort networkID = players.GetNetworkID(gameObject);
            SendNetworkID(networkID, ServerMessage.PlayAttackPlayer, SendMode.Unreliable);
        }

        private void PlayAttackPlayer(MessageReceivedEventArgs e)
        {
            players.SpaghettiAttack(UnwrapID(e));
        }

        private void SendNetworkID(ushort networkID, ushort messageTag, SendMode sendMode)
        {
            using (var writer = DarkRiftWriter.Create())
            {
                writer.Write(networkID);
                using (var message = Message.Create(messageTag, writer))
                {
                    Client.SendMessage(message, sendMode);
                }
            }
        }

        private void SetupServerData(MessageReceivedEventArgs e)
        {
            ConnectionData data;
            using (Message message = e.GetMessage())
            {
                data = message.Deserialize<ConnectionData>();
            }

            LevelGenerationRequested.Trigger(data.LevelSeed);
            ushort clientId = data.ClientID;
            Debug.Log("Client id is " + clientId);

            foreach (Player player in data.Players)
            {
                players.Create(this.NetworkInstantiator.PlayerPrefab, player, player.NetworkID == data.PlayerObjectID);
            }

            foreach (var enemy in data.Enemies)
            {
                enemies.Create(this.EnemyList.Enemies[0], enemy, false);
            }
        }

        /// <summary>
        /// Sends level start request to server.
        /// </summary>
        /// <param name="p0">The parameter is not used.</param>
        /// <param name="p1">The parameter is not used.</param>
        /// <param name="p2">The parameter is not used.</param>
        /// <param name="p3">The parameter is not used.</param>
        private void LevelStartRequested(object p0, object p1, object p2, object p3)
        {
            using (Message message = Message.CreateEmpty(ClientMessage.LevelStartRequest))
            {
                this.Client.SendMessage(message, SendMode.Unreliable);
            }
        }

        private void CreatePlayer(MessageReceivedEventArgs e)
        {
            using (Message message = e.GetMessage())
            {
                players.Create(this.NetworkInstantiator.PlayerPrefab, message.Deserialize<Player>());
            }
        }

        private void UpdatePlayer(MessageReceivedEventArgs e)
        {
            using (Message message = e.GetMessage())
            {
                players.DeserializeAndUpdate(message);
            }
        }

        private void UpdateEnemy(MessageReceivedEventArgs e)
        {
            using (Message message = e.GetMessage())
            {
                enemies.DeserializeAndUpdate(message);
            }
        }

        private void DeleteObject(MessageReceivedEventArgs e, ObjectType objectType)
        {
            ushort id = UnwrapID(e);

            switch (objectType)
            {
                case ObjectType.Enemy:
                    enemies.Destroy(id);
                    break;
                case ObjectType.Player:
                    players.Destroy(id);
                    break;
                default:
                    Debug.LogError("TODO: Write error message");
                    break;
            }
        }

        private void DestroyObject(GameObject gameObject, ObjectType objectType, object p2, object p3)
        {
            // TODO: Clean up copypaste
            ushort networkID = 0;
            ushort messageTag = 0;
            switch (objectType)
            {
                case ObjectType.Enemy:
                    networkID = enemies.GetNetworkID(gameObject);
                    enemies.Destroy(networkID);
                    messageTag = ClientMessage.DeleteEnemy;
                    break;
                case ObjectType.Player:
                    networkID = players.GetNetworkID(gameObject);
                    players.Destroy(networkID);
                    messageTag = ClientMessage.DeletePlayer;
                    break;
                default:
                    Debug.LogError("TODO: Writer error message");
                    break;
            }

            SendNetworkID(networkID, messageTag, SendMode.Reliable);
        }
    }
}

