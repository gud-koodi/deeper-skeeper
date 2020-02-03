namespace Network
{
    using UnityEngine;

    public class NetworkSlave : MonoBehaviour
    {
        private const float RUBBERBAND_TRESHOLD = 10;
        private const float EPSILON = 0.1f;

        public float speed = 10f;

        public Vector3 TargetPosition;

        public Rigidbody Rigidbody;

        /// <summary>
        /// Update the state of this slave controller with given player data.
        /// </summary>
        /// <param name="player">Player data.</param>
        internal void UpdateState(Player player)
        {
            Rigidbody.rotation = Quaternion.Euler(0, player.Rotation, 0);
            this.TargetPosition = player.TargetPosition;

            if ((player.CurrentPosition - Rigidbody.position).magnitude > RUBBERBAND_TRESHOLD)
            {
                Rigidbody.position = player.CurrentPosition;
            }
        }

        void FixedUpdate()
        {
            Vector3 localPosition = Rigidbody.position;
            Vector3 direction = TargetPosition - localPosition;
            float distance = direction.magnitude;
            
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
