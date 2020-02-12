namespace GudKoodi.DeeperSkeeper.Network
{
    using UnityEngine;
    using DeeperSkeeper.Event;

    public class NetworkMaster : MonoBehaviour
    {
        private const float MOVEMENT_UPDATE_TRESHOLD = 0.5f;
        private const float ROTATION_UPDATE_TRESHOD = 30f;

        ////public GameEvent UpdateEvent;
        public ObjectUpdateRequested ObjectUpdateRequested;

        private Vector3 oldPosition;

        private Quaternion oldRotation;

        void OnEnable()
        {
            oldPosition = transform.localPosition;
            oldRotation = transform.localRotation;
        }

        void LateUpdate()
        {
            Vector3 currentPosition = transform.localPosition;
            Quaternion currentRotation = transform.localRotation;
            if ((currentPosition - this.oldPosition).magnitude > MOVEMENT_UPDATE_TRESHOLD
                || Quaternion.Angle(currentRotation, this.oldRotation) > ROTATION_UPDATE_TRESHOD)
            {
                this.oldPosition = currentPosition;
                this.oldRotation = currentRotation;
                this.ObjectUpdateRequested.Trigger(gameObject);
            }
        }
    }
}
