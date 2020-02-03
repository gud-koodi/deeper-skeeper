using System.Collections;
using System.Collections.Generic;
using Network;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class NetworkIdPoolTest
    {
        // A Test behaves as an ordinary method
        [Test]
        public void IdsAreGivenProperly()
        {
            NetworkIdPool pool = new NetworkIdPool();

            Assert.AreEqual(0, pool.Next());
            Assert.AreEqual(1, pool.Next());
            Assert.AreEqual(2, pool.Next());

            pool.Release(1);
            Assert.AreEqual(1, pool.Next());
            Assert.AreEqual(3, pool.Next());
        }

        [Test]
        public void NoIllegalReleases()
        {
            NetworkIdPool pool = new NetworkIdPool();

            Assert.False(pool.Release(0));
            Assert.False(pool.Release(99));
            Assert.AreEqual(0, pool.Next());
            Assert.AreEqual(1, pool.Next());

            pool = new NetworkIdPool();

            Assert.AreEqual(0, pool.Next());
            Assert.True(pool.Release(0));
            Assert.False(pool.Release(0));
            Assert.AreEqual(0, pool.Next());
            Assert.AreNotEqual(0, pool.Next());
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
            Assert.AreEqual(34000, pool.Next()); // Next should be 34000
        }
    }
}
