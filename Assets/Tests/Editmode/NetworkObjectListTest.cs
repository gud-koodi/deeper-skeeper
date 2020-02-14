namespace GudKoodi.DeeperSkeeper.Tests
{
    using Network;
    using NUnit.Framework;
    using UnityEngine;

    public class NetworkObjectListTest
    {
        [Test]
        public void NetworkObjectListWorksProperly()
        {
            NetworkObjectList list = new NetworkObjectList();

            GameObject go_a = new GameObject();
            GameObject go_b = new GameObject();
            GameObject go_c = new GameObject();
            GameObject go_d = new GameObject();

            list[0] = go_a;
            Assert.AreSame(go_a, list[0]);
            list[1] = go_b;
            Assert.AreSame(go_b, list[1]);
            list[2] = go_c;
            Assert.AreSame(go_b, list[1]);
            Assert.AreSame(go_b, list.RemoveAt(1));
            Assert.Null(list[1]);
            Assert.Null(list.RemoveAt(1));
            Assert.NotNull(list[2]);
            list[1] = go_d;
            Assert.NotNull(list[2]);
            Assert.AreSame(go_c, list[2]);
            Assert.AreSame(go_c, list.RemoveAt(2));
        }

        [Test]
        public void NetworkObjectListWorksProperly2()
        {
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
