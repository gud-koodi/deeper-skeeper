using System;
using System.Collections.Generic;

namespace Network {
    public class NetworkIdPool {
        private ushort counter;
        private Queue<ushort> recyclables;

        public NetworkIdPool() {
            this.counter = 0;
            this.recyclables = new Queue<ushort>();
        }

        public ushort Next() {
            return (recyclables.Count > 0) ? recyclables.Dequeue() : counter++;
        }

        public void Release(ushort id) {
            if (id < counter) {
                recyclables.Enqueue(id);
            } else {
                throw new IndexOutOfRangeException();
            }
        }
    }

}
