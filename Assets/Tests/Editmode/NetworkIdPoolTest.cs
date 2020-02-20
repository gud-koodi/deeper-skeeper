namespace GudKoodi.DeeperSkeeper.Tests
{
    using NUnit.Framework;
    using Network;

    public class NetworkIdPoolTest
    {
        // A Test behaves as an ordinary method
        [Test]
        public void IdsAreGivenProperly()
        {
            NetworkIdPool pool = new NetworkIdPool();

            Assert.AreEqual(1, pool.Next());
            Assert.AreEqual(2, pool.Next());
            Assert.AreEqual(3, pool.Next());

            pool.Release(2);
            Assert.AreEqual(2, pool.Next());
            Assert.AreEqual(4, pool.Next());
        }

        [Test]
        public void NoIllegalReleases()
        {
            NetworkIdPool pool = new NetworkIdPool();

            Assert.False(pool.Release(0));
            Assert.False(pool.Release(1));
            Assert.False(pool.Release(99));
            Assert.AreEqual(1, pool.Next());
            Assert.AreEqual(2, pool.Next());

            pool = new NetworkIdPool();

            Assert.AreEqual(1, pool.Next());
            Assert.True(pool.Release(1));
            Assert.False(pool.Release(1));
            Assert.AreEqual(1, pool.Next());
            Assert.AreNotEqual(1, pool.Next());
        }

        [Test]
        public void LargeTest()
        {
            NetworkIdPool pool = new NetworkIdPool();

            for (int i = 0; i < 30000; i++) // Add 30000
            {
                pool.Next();
            }

            ushort k = 13;
            for (int i = 0; i < 16000; i++) // Remove 16000
            {
                k = (ushort)((k + 53) % 30000);
                pool.Release(k);
            }

            for (int i = 0; i < 20000; i++) // Add back 20000
            {
                pool.Next();
            }
            Assert.AreEqual(34001, pool.Next()); // Next should be 34001
        }
    }
}
