namespace Network
{
    using UnityEngine;

    public class NetworkMaster : MonoBehaviour
    {
        private const float MOVEMENT_UPDATE_TRESHOLD = 0.5f;

        public GameEvent UpdateEvent;

        private Vector3 oldPosition;

        void OnEnable()
        {
            oldPosition = transform.localPosition;
        }

        void LateUpdate()
        {
            Vector3 currentPosition = transform.localPosition;
            if ((currentPosition - oldPosition).magnitude > MOVEMENT_UPDATE_TRESHOLD)
            {
                oldPosition = currentPosition;
                UpdateEvent.Trigger(gameObject);
            }
        }
    }
}
