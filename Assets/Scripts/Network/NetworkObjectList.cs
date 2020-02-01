namespace Network
{
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Collection for storing and accessing objects by their network ids.
    /// </summary>
    public class NetworkObjectList
    {
        /// <summary>
        /// List where all gameobjects are stored by their network ids.
        /// </summary>
        private readonly List<GameObject> list;

        /// <summary>
        /// Instance ids of stored objects mapped to their network ids.
        /// </summary>
        private readonly Dictionary<int, ushort> networkIDLookUp;

        /// <summary>
        /// Creates new instance of <c>NetworkObjectList</c>.
        /// </summary>
        public NetworkObjectList()
        {
            this.list = new List<GameObject>();
            this.networkIDLookUp = new Dictionary<int, ushort>();
        }

        /// <summary>
        /// Gets or sets the gameObject at specified network id.
        /// Getter returns null if no object is present.
        /// </summary>
        /// <value>GameObject the network id should point to.</value>
        public GameObject this[ushort index]
        {
            get => this.GetAt(index);
            set => this.SetAt(index, value);
        }

        /// <summary>
        /// Checks whether or not there are GameObject at the specified index.
        /// </summary>
        /// <param name="networkID">Network id to check vacancy of.</param>
        /// <returns>True if and only if there is an object at the specified index.</returns>
        public bool IsVacant(ushort networkID)
        {
            return networkID >= list.Count || list[networkID] == null;
        }

        /// <summary>
        /// Finds the network id of the specified stored object.
        /// </summary>
        /// <param name="gameObject">GameObject to check the network id for.</param>
        /// <returns>Network id of the object.</returns>
        public ushort LookUpNetworkID(GameObject gameObject)
        {
            // TODO: Handle exception if not found.
            return this.networkIDLookUp[gameObject.GetInstanceID()];
        }

        /// <summary>
        /// Removes the GameObject with given network id from this list.
        /// </summary>
        /// <param name="networkID">Network id of the object to remove.</param>
        /// <returns>GameObject that was in given position, or null if the position was empty.</returns>
        public GameObject RemoveAt(ushort networkID)
        {
            GameObject go = this.list[networkID]; // GetAt(networkID);
            if (go != null)
            {
                this.networkIDLookUp.Remove(go.GetInstanceID());
                this.list[networkID] = null;
            }
            return go;
        }

        private void SetAt(ushort networkID, GameObject gameObject)
        {
            if (networkID >= this.list.Count)
            {
                this.list.Insert(networkID, gameObject);
                this.networkIDLookUp[gameObject.GetInstanceID()] = networkID;
                Debug.Log($"Stored instance id {gameObject.GetInstanceID()}");
            }
            else if (list[networkID] == null)
            {
                this.list[networkID] = gameObject;
                this.networkIDLookUp[gameObject.GetInstanceID()] = networkID;
                Debug.Log($"Stored instance id {gameObject.GetInstanceID()}");
            }
            else
            {
                Debug.LogError("Attempted to insert a gameobject to existing id");
            }
        }

        private GameObject GetAt(ushort networkID)
        {
            return (networkID >= this.list.Count) ? null : this.list[networkID];
        }
    }
}
