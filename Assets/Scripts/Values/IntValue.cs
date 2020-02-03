namespace Value
{
    using System.Collections.Generic;
    using UnityEngine;

    [CreateAssetMenu(fileName = "IntValue", menuName = "Value/IntValue")]
    public class IntValue : ScriptableObject
    {
        private int internalValue = 0;

        private readonly HashSet<ICallable<int>> listeners = new HashSet<ICallable<int>>();

        public int Value
        {
            get
            {
                return internalValue;
            }
            set
            {
                internalValue = value;
                this.OnChange();
            }
        }

        /// <summary>
        /// Subscribes the given callable to this value's changes.
        /// </summary>
        /// <param name="listener">subscribing listener</param>
        public void Subscribe(ICallable<int> listener)
        {
            listeners.Add(listener);
        }

        /// <summary>
        /// Unsubscribes the given callable from this value's changes.
        /// </summary>
        /// <param name="listener">unsubscribing listener</param>
        public void UnSubscribe(ICallable<int> listener)
        {
            listeners.Remove(listener);
        }

        private void OnChange()
        {
            foreach (var callable in listeners)
            {
                callable.Call(this.internalValue);
            }
        }
    }
}
