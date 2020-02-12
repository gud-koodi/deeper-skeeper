/* 
    GameEvent pattern using ScriptableObject as presented by Ryan Hipple.
    More info at: https://github.com/roboryantron/Unite2017

    Extended to support maximum amount of unity event's arguments.
 */

namespace GudKoodi.DeeperSkeeper.Event
{
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Base class for all events.
    /// </summary>
    /// <typeparam name="T0">Type of first argument.</typeparam>
    /// <typeparam name="T1">Type of second argument.</typeparam>
    /// <typeparam name="T2">Type of third argument.</typeparam>
    /// <typeparam name="T3">Type of fourth argument.</typeparam>
    public abstract class BaseEvent<T0, T1, T2, T3> : ScriptableObject
    {
        /// <summary>
        /// Set of events listening this event.
        /// </summary>
        private HashSet<IListener<T0, T1, T2, T3>> listeners = new HashSet<IListener<T0, T1, T2, T3>>();

        /// <summary>
        /// Subscribes the given listener to this event's triggers, if not already subscribed.
        /// </summary>
        /// <param name="listener">Subscribing listener</param>
        public void Subscribe(IListener<T0, T1, T2, T3> listener)
        {
            listeners.Add(listener);
        }

        /// <summary>
        /// Unsubscribes the given listener from this event's triggers, if subscribed.
        /// </summary>
        /// <param name="listener">Unsubscribing listener</param>
        public void UnSubscribe(IListener<T0, T1, T2, T3> listener)
        {
            listeners.Remove(listener);
        }

        /// <summary>
        /// Triggers the event and notifies all listeners.
        /// </summary>
        /// <param name="p0">First parameter.</param>
        /// <param name="p1">Second parameter.</param>
        /// <param name="p2">Third parameter.</param>
        /// <param name="p3">Fourth parameter.</param>
        protected void Trigger(T0 p0, T1 p1, T2 p2, T3 p3)
        {
            foreach (var listener in listeners)
            {
                listener.OnTriggered(p0, p1, p2, p3);
            }
        }
    }
}
