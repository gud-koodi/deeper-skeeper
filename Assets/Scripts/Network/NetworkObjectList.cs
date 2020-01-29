using System;
using System.Collections.Generic;
using UnityEngine;

namespace Network {
    public class NetworkObjectList {
        public List<GameObject> list;

        private readonly Dictionary<int, ushort> networkIDLookUp;

        public NetworkObjectList() {
            this.list = new List<GameObject>();
            this.networkIDLookUp = new Dictionary<int, ushort>();
        }

        public GameObject this [ushort index] {
            get => GetAt(index);
            set => SetAt(index, value);
        }

        private void SetAt(ushort networkID, GameObject gameObject) {
            if (IsVacant(networkID)) {
                // networkIDLookUp.Remove(list[networkID].GetInstanceID());
                list.Insert(networkID, gameObject);
                networkIDLookUp[gameObject.GetInstanceID()] = networkID;
            } else {
                Debug.LogError("Attempted to insert a gameobject to existing id");
            }
        }

        private GameObject GetAt(ushort networkID) {
            return list[networkID];
            // return (networkID >= list.Count) ? null : list[networkID];
        }

        public GameObject RemoveAt(ushort networkID) {
            GameObject go = GetAt(networkID);
            if (go != null) {
                networkIDLookUp.Remove(go.GetInstanceID());
                list[networkID] = null;
            }
            return go;
        }

        public bool IsVacant(ushort networkID) {
            return (networkID >= list.Count || list[networkID] == null);
        }

        public uint LookUpNetworkID(GameObject gameObject) {
            return networkIDLookUp[gameObject.GetInstanceID()];
        }
    }
}
