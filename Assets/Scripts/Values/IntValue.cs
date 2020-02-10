namespace GudKoodi.DeeperSkeeper.Value
{
    using System.Collections.Generic;
    using UnityEngine;

    [CreateAssetMenu(fileName = "IntValue", menuName = "Value/IntValue")]

    /// <summary>
    /// IntValue class.
    /// </summary>
    public class IntValue : ScriptableObject
    {
        private readonly HashSet<ICallable<int>> listeners = new HashSet<ICallable<int>>();
        private int internalValue = 0;

        /// <summary>
        /// Gets or sets value.
        /// </summary>
        /// <value>Integer value.</value>
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
