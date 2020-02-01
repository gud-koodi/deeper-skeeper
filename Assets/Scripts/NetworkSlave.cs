namespace Network
{
    using UnityEngine;

    public class NetworkSlave : MonoBehaviour
    {
        private const float RUBBERBAND_TRESHOLD = 10;
        private const float EPSILON = 0.1f;

        public float speed = 10f;

        public Vector3 targetPosition;

        public Rigidbody Rigidbody;

        void Start()
        {
            if (Rigidbody == null) { Rigidbody = GetComponent<Rigidbody>(); }
            targetPosition = transform.localPosition;
        }

        void FixedUpdate()
        {
            Vector3 localPosition = Rigidbody.position;
            Vector3 direction = targetPosition - localPosition;
            float distance = direction.magnitude;
            if (distance > RUBBERBAND_TRESHOLD)
            {
                Rigidbody.position = targetPosition;
                return;
            }
            else
            {
                if (Rigidbody.velocity.y < -10)
                {
                    Rigidbody.velocity = new Vector3(0, Rigidbody.velocity.y, 0);
                }
                else
                {
                    Vector3 planeVelocity = (distance > EPSILON)
                        ? speed * (new Vector3(direction.x, 0, direction.z).normalized)
                        : Vector3.zero;
                    Rigidbody.velocity = new Vector3(planeVelocity.x, Rigidbody.velocity.y, planeVelocity.z);
                }
            }
        }
    }
}
