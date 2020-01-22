using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/* 
    GameEvent pattern using ScriptableObject as presented by Ryan Hipple.
    Brackets are fixed because we are monsters and don't obey poor standards.
    More info at: https://github.com/roboryantron/Unite2017
 */

[CreateAssetMenu(fileName = "GameEvent", menuName = "Event/GameEvent")]
public class GameEvent : ScriptableObject {
    private HashSet<GameEventListener> listeners = new HashSet<GameEventListener>();

    public void Trigger() {
        foreach(var listener in listeners) {
            listener.OnTriggered();
        }
    }

    public void Subscribe(GameEventListener listener) {
        listeners.Add(listener);
    }

    public void UnSubscribe(GameEventListener listener) {
        listeners.Remove(listener);
    }
}
