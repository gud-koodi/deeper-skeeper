namespace GudKoodi.DeeperSkeeper.Enemy
{
    using System;
    using System.Collections;
    using Event;
    using GudKoodi.DeeperSkeeper.Weapon;
    using UnityEngine;
    using UnityEngine.AI;

    /// <summary>
    /// Component for controlling enemy object.
    /// </summary>
    public class EnemyController : MonoBehaviour
    {
        private const float MOVEMENT_UPDATE_TRESHOLD = 0.5f;
        private const float ROTATION_UPDATE_TRESHOD = 30f;

        public GameObject Player;
        public BotState State = BotState.IDLE;
        public Weapon Weapon;
        public ObjectUpdateRequested ObjectUpdateRequested;
        public float HitSpeed = 1f;
        public OffMeshLinkMoveMethod Method = OffMeshLinkMoveMethod.Parabola;
        public AnimationCurve AnimCurve = new AnimationCurve();
        private Animator animator;
        private UnityEngine.AI.NavMeshAgent agent;
        private int playerLayer;
        private Action idleStrategy;
        private Action updateStrategy;

        private Vector3 oldPosition;
        private Quaternion oldRotation;

        /// <summary>
        /// How to move between the link.
        /// </summary>
        public enum OffMeshLinkMoveMethod
        {
            /// <summary>
            /// Teleport between start pos and end pos. 
            /// </summary>
            Teleport,

            /// <summary>
            /// NormalSpeed movement.
            /// </summary>
            NormalSpeed,

            /// <summary>
            /// Move by Prabola.
            /// </summary>
            Parabola,

            /// <summary>
            /// Move by curve. 
            /// </summary>
            Curve
        }

        /// <summary>
        /// AI State.
        /// </summary>
        public enum BotState
        {
            /// <summary>
            /// Do nothing.
            /// </summary>
            IDLE,

            /// <summary>
            /// Chase player.
            /// </summary>
            CHASE,

            /// <summary>
            /// Attack player.
            /// </summary>
            ATTACK
        }

        /// <summary>
        /// Starts chasing the given player object.
        /// </summary>
        /// <param name="target">Player to chase.</param>
        public void StartChase(GameObject target)
        {
            this.Player = target;
            this.State = BotState.CHASE;
            this.ObjectUpdateRequested.Trigger(this.gameObject, ObjectType.Enemy);
        }

        public void SetAsMaster() {
            this.idleStrategy = () => IdleMaster();
            this.updateStrategy = () => UpdateMaster();
        }

        void Awake()
        {
            this.idleStrategy = () => { };
            this.updateStrategy = () => { };
            this.playerLayer = LayerMask.GetMask("Player");
            this.oldPosition = transform.localPosition;
            this.oldRotation = transform.localRotation;
        }

        // Start is called before the first frame update
        void Start()
        {
            agent = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
            animator = gameObject.GetComponent<Animator>();
            animator.SetFloat("hitSpeed", HitSpeed);
            Weapon.AttackDuration = Weapon.AttackDuration / HitSpeed;
            agent.autoTraverseOffMeshLink = false;
        }

        // Update is called once per frame
        void Update()
        {
            switch (State)
            {
                case BotState.CHASE:
                    Chase();
                    break;
                case BotState.IDLE:
                    idleStrategy();
                    break;
                case BotState.ATTACK:
                    Attack();
                    break;
            }

            updateStrategy();
            StartCoroutine(CheckOffMeshLinkMove());
        }

        private void Chase()
        {
            if (!Player)
            {
                State = BotState.IDLE;
                idleStrategy();
                return;
            }

            if (Vector3.Distance(Player.transform.position, transform.position) < 3)
            {
                agent.isStopped = true;
                State = BotState.ATTACK;
            }
            else
            {
                agent.isStopped = false;
                agent.destination = Player.transform.position;
                animator.SetBool("isWalking", true);
            }
        }

        private void IdleMaster()
        {
            Collider[] players = Physics.OverlapSphere(transform.position, 27.14f, playerLayer);
            if (players.Length > 0)
            {
                StartChase(players[0].gameObject);
            }
        }

        private void Attack()
        {
            Weapon.Attack();
            animator.SetBool("isAttacking", true);
            State = BotState.IDLE;
            StartCoroutine(WaitAttack());
        }

        private void UpdateMaster()
        {
            Vector3 currentPosition = transform.localPosition;
            Quaternion currentRotation = transform.localRotation;
            if ((currentPosition - this.oldPosition).magnitude > MOVEMENT_UPDATE_TRESHOLD)
                //// || Quaternion.Angle(currentRotation, this.oldRotation) > ROTATION_UPDATE_TRESHOD)
            {
                this.oldPosition = currentPosition;
                //// this.oldRotation = currentRotation;
                this.ObjectUpdateRequested.Trigger(gameObject, ObjectType.Enemy);
            }
        }

        public void UpdateState(GameObject target, Vector3 networkPosition)
        {
            // Rubberband if necessary
            if ((networkPosition - transform.position).magnitude > 10f)
            {
                transform.position = networkPosition;
            }

            if (target != Player)
            {
                StartChase(target);
            }
        }

        private IEnumerator WaitAttack()
        {
            yield return null;
            animator.SetBool("isAttacking", false);
            yield return new WaitForSeconds(Weapon.AttackDuration);
            State = BotState.CHASE;
        }

        private IEnumerator CheckOffMeshLinkMove()
        {
            if (agent.isOnOffMeshLink)
            {
                if (Method == OffMeshLinkMoveMethod.NormalSpeed)
                {
                    yield return StartCoroutine(NormalSpeed(agent));
                }
                else if (Method == OffMeshLinkMoveMethod.Parabola)
                {
                    yield return StartCoroutine(Parabola(agent, 2.0f, 0.5f));
                }
                else if (Method == OffMeshLinkMoveMethod.Curve)
                {
                    yield return StartCoroutine(Curve(agent, 0.5f));
                }

                agent.CompleteOffMeshLink();
            }

            yield return null;
        }

        private IEnumerator NormalSpeed(NavMeshAgent agent)
        {
            OffMeshLinkData data = agent.currentOffMeshLinkData;
            Vector3 endPos = data.endPos + (Vector3.up * agent.baseOffset);

            while (agent.transform.position != endPos)
            {
                agent.transform.position = Vector3.MoveTowards(agent.transform.position, endPos, agent.speed * Time.deltaTime);
                yield return null;
            }
        }

        private IEnumerator Parabola(NavMeshAgent agent, float height, float duration)
        {
            OffMeshLinkData data = agent.currentOffMeshLinkData;
            Vector3 startPos = agent.transform.position;
            Vector3 endPos = data.endPos + (Vector3.up * agent.baseOffset);
            float normalizedTime = 0.0f;

            while (normalizedTime < 1.0f)
            {
                float yOffset = height * 4.0f * (normalizedTime - (normalizedTime * normalizedTime));
                agent.transform.position = Vector3.Lerp(startPos, endPos, normalizedTime) + (yOffset * Vector3.up);
                normalizedTime += Time.deltaTime / duration;
                yield return null;
            }
        }

        private IEnumerator Curve(NavMeshAgent agent, float duration)
        {
            OffMeshLinkData data = agent.currentOffMeshLinkData;
            Vector3 startPos = agent.transform.position;
            Vector3 endPos = data.endPos + (Vector3.up * agent.baseOffset);
            float normalizedTime = 0.0f;

            while (normalizedTime < 1.0f)
            {
                float yOffset = AnimCurve.Evaluate(normalizedTime);
                agent.transform.position = Vector3.Lerp(startPos, endPos, normalizedTime) + (yOffset * Vector3.up);
                normalizedTime += Time.deltaTime / duration;
                yield return null;
            }
        }
    }
}
