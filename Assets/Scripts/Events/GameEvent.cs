using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/* 
    GameEvent pattern using ScriptableObject as presented by Ryan Hipple.
    Brackets are fixed because we are monsters and don't obey poor standards.
    More info at: https://github.com/roboryantron/Unite2017
 */

[CreateAssetMenu(fileName = "GameEvent", menuName = "Event/GameEvent")]
public class GameEvent : ScriptableObject
{
    private HashSet<GameEventListener> listeners = new HashSet<GameEventListener>();

    /// <summary>
    /// Triggers the event and notifies all listeners.
    /// </summary>
    public void Trigger()
    {
        foreach (var listener in listeners)
        {
            listener.OnTriggered();
        }
    }

    /// <summary>
    /// Subscribes the given listener to this event's triggers, if not already subscribed.
    /// </summary>
    /// <param name="listener">subscribing listener</param>
    public void Subscribe(GameEventListener listener)
    {
        listeners.Add(listener);
    }

    /// <summary>
    /// Unsubscribes the given listener from this event's triggers, if subscribed.
    /// </summary>
    /// <param name="listener">unsubscribing listener</param>
    public void UnSubscribe(GameEventListener listener)
    {
        listeners.Remove(listener);
    }
}
