/* 
    GameEvent pattern using ScriptableObject as presented by Ryan Hipple.
    More info at: https://github.com/roboryantron/Unite2017
 */

namespace GudKoodi.DeeperSkeeper.Event
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;

    public class GameEventListener : MonoBehaviour
    {

        [Tooltip("GameEvent to listen to")]
        public GameEvent Event;

        [Tooltip("A list of event to call when the listened event is triggered")]
        public GameObjectEvent Response;

        void OnEnable()
        {
            Event.Subscribe(this);
        }

        void OnDisable()
        {
            Event.UnSubscribe(this);
        }

        public void OnTriggered(GameObject target)
        {
            Response.Invoke(target);
        }
    }

    [Serializable]
    public class GameObjectEvent : UnityEvent<GameObject>
    {
    }
}
