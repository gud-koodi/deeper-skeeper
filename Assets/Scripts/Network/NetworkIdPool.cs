using System;
using System.Collections.Generic;

namespace Network
{
    /// <summary>
    /// Class for handling out sequential ids for objects and help with cache locality by recycling old ids.
    /// </summary>
    public class NetworkIdPool
    {
        private ushort counter;
        private Queue<ushort> recyclables;

        /// <summary>
        /// Creates new instance of NetworkIdPool, that will start to hand out ids starting from zero.
        /// </summary>
        public NetworkIdPool()
        {
            this.counter = 0;
            this.recyclables = new Queue<ushort>();
        }

        /// <summary>
        /// Returns next, possibly recycled network id.
        /// </summary>
        /// <returns>The next usable id in this pool</returns>
        public ushort Next()
        {
            return (recyclables.Count > 0) ? recyclables.Dequeue() : counter++;
        }

        /// <summary>
        /// Releases given id back to the pool of usable ids.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Release(ushort id)
        {
            if (id < counter && !recyclables.Contains(id)) // TODO: implement a structure with faster search
            {
                recyclables.Enqueue(id);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
