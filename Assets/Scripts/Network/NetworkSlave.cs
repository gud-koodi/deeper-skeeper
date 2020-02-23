namespace GudKoodi.DeeperSkeeper.Network
{
    using System.Collections;
    using UnityEngine;
    using Weapon;

    public class NetworkSlave : MonoBehaviour
    {
        private const float RUBBERBAND_TRESHOLD = 10;
        private const float EPSILON = 0.1f;

        public float speed = 10f;

        public Vector3 TargetPosition;

        public Rigidbody Rigidbody;

        private Animator animator;
        private Weapon weapon;

        public bool isAttacking = false;

        /// <summary>
        /// Starts attack animation with current weapon.
        /// </summary>
        public void StartAttack()
        {
            weapon.Attack();
            animator.SetBool("isAttacking", true);
            isAttacking = true;
            StartCoroutine(WaitAttack());
        }

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

        void Start()
        {
            animator = GetComponent<Animator>();
            weapon = GetComponentInChildren<Weapon>();
            weapon.AttackDuration = 20f;
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

        private IEnumerator WaitAttack()
        {
            yield return null;
            animator.SetBool("isAttacking", false);
            yield return new WaitForSeconds(weapon.AttackDuration);
            isAttacking = false;
        }
    }
}
