using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Network;

namespace Tests {
    public class NetworkObjectListTest {
        // A Test behaves as an ordinary method
        [Test]
        public void NetworkObjectListWorksProperly() {
            GameObject go_a = new GameObject();
            GameObject go_b = new GameObject();
            GameObject go_c = new GameObject();

            NetworkObjectList list = new NetworkObjectList();

            Assert.True(list.IsVacant(0));
            list[0] = go_a;
            Assert.False(list.IsVacant(0));
            Assert.AreSame(go_a, list[0]);

            GameObject go_d = list.RemoveAt(0);
            Assert.AreSame(go_a, go_d);
            Assert.True(list.IsVacant(0));


        }
    }
}
