namespace GudKoodi.DeeperSkeeper.Network
{
    using System;
    using System.Collections.Generic;
    using DarkRift;
    using UnityEngine;
    using Entity;

    /// <summary>
    /// Class for managing collections of multiple similar network objects.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ObjectManager<T> where T : INetworkSerializable, new()
    {
        /// <summary>
        /// List of managed GameObjects.
        /// </summary>
        private readonly NetworkObjectList gameObjects;

        /// <summary>
        /// Serialization data for managed gameObjects.
        /// </summary>
        private readonly List<T> serializables;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectManager"/> class.
        /// </summary>
        protected ObjectManager()
        {
            this.gameObjects = new NetworkObjectList();
            this.serializables = new List<T>();
        }

        /// <summary>
        /// Gets object at the specified network ID.
        /// </summary>
        /// <param name="index">network ID of object.</param>
        /// <returns>Object at the specified network ID.</returns>
        public GameObject this[ushort index]
        {
            get => this.gameObjects[index];
        }

        /// <summary>
        /// Creates and starts managing a new <see cref="GameObject" /> using given serialization data. 
        /// </summary>
        /// <param name="prefab">Prefab used as a base for the new GameObject.</param>
        /// <param name="data">Serialization data used in creation.</param>
        /// <param name="masterObject">Whether or not this object should be controlled locally.</param>
        /// <returns>The created GameObject.</returns>
        public GameObject Create(GameObject prefab, T data, bool masterObject = false)
        {
            GameObject go = null;
            ushort id = data.NetworkID;
            if (this.gameObjects.IsVacant(id))
            {
                go = (masterObject) ? this.InstantiateMaster(prefab, data) : this.InstantiateSlave(prefab, data);
                this.gameObjects[id] = go;
                this.serializables.Add(data);
                Debug.Log($"Assigned network ID {id} to new {data.GetType()} instance.");
            }
            else
            {
                Debug.LogError($"Network ID {id} was already present.");
            }

            return go;
        }

        /// <summary>
        /// Gets the network ID of given object if managed.
        /// </summary>
        /// <param name="gameObject">GameObject to look network ID of.</param>
        /// <returns>network ID of object.</returns>
        public ushort GetNetworkID(GameObject gameObject)
        {
            //// Debug.Log($"{typeof(T)} also {gameObject}");
            return this.gameObjects.LookUpNetworkID(gameObject);
        }

        /// <summary>
        /// Deserializes the given message and updates the state of its representative local GameObject.
        /// </summary>
        /// <param name="message">Message to deserialize, interpreted as the type defined for this instance.</param>
        public void DeserializeAndUpdate(Message message)
        {
            T data = message.Deserialize<T>();
            GameObject gameObject = this.gameObjects[data.NetworkID];
            this.DeserializeState(data, gameObject);

            // TODO: Don't use a list.
            int index = this.serializables.FindIndex(p => p.NetworkID == data.NetworkID);
            this.serializables[index] = data;
        }

        /// <summary>
        /// Updates the serialized data of the given object and writes it to a Message with given tag.
        /// </summary>
        /// <param name="gameObject">Object for serialization.</param>
        /// <param name="tag">Tag for the message.</param>
        /// <returns>Message with given tag and serialized data for object.</returns>
        public Message UpdateAndSerialize(GameObject gameObject, ushort tag)
        {
            ushort id = this.gameObjects.LookUpNetworkID(gameObject);
            T t = this.serializables.Find(s => s.NetworkID == id);
            this.SerializeState(t, gameObject);
            return Message.Create(tag, t);
        }

        /// <summary>
        /// Stops managing the GameObject with specified network id and destroys it.
        /// </summary>
        /// <param name="networkID"></param>
        public void Destroy(ushort networkID)
        {
            if (!this.gameObjects.IsVacant(networkID))
            {
                GameObject go = this.gameObjects.RemoveAt(networkID);
                Debug.Log($"Removing {go} at network id {networkID}");
                go.GetComponent<Living>().Kill();
            }
            else
            {
                Debug.LogError($"Trying to remove object ID {networkID} but was not found");
            }

            int count = this.serializables.RemoveAll(p => p.NetworkID == networkID);
            if (count != 1)
            {
                throw new Exception("TODO: Write error message");
            }
        }

        /// <summary>
        /// Returns an array representation of this object's serialization data.
        /// </summary>
        /// <returns>Serialization data array.</returns>
        public T[] ToArray()
        {
            // TODO: Handle the serialization of multiple objects as a method.
            return serializables.ToArray();
        }

        /// <summary>
        /// Instantiates a new GameObject and gives it proper components for local controlling.
        /// </summary>
        /// <param name="prefab">Prefab used as a base for the new GameObject.</param>
        /// <param name="t">Serialization data used in creation.</param>
        /// <returns>Instantiated GameObject.</returns>
        protected abstract GameObject InstantiateMaster(GameObject prefab, T t);

        /// <summary>
        /// Instantiates a new GameObject and gives it proper components for remote controlling.
        /// </summary>
        /// <param name="prefab">Prefab used as a base for the new GameObject.</param>
        /// <param name="t">Serialization data used in creation.</param>
        /// <returns>Instantiated GameObject.</returns>
        protected abstract GameObject InstantiateSlave(GameObject prefab, T t);

        /// <summary>
        /// Deserializes the state to the GameObject from data.
        /// </summary>
        /// <param name="data">Data to deserialize from.</param>
        /// <param name="gameObject">GameObject to update.</param>
        protected abstract void DeserializeState(T data, GameObject gameObject);

        /// <summary>
        /// Serializes the GameObject to data.
        /// </summary>
        /// <param name="data">Data to serialize into.</param>
        /// <param name="gameObject">GameObject to serialize from.</param>
        protected abstract void SerializeState(T data, GameObject gameObject);
    }
}
