using System.Collections;
using GudKoodi.DeeperSkeeper.Weapon;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public enum State
    {
        IDLE,
        CHASE,
        ATTACK
    }

    public GameObject player;
    public State state;
    public Weapon weapon;
    public float hitSpeed = 1f;
    private Animator animator;
    private UnityEngine.AI.NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        agent = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
        state = State.CHASE;
        animator = gameObject.GetComponent<Animator>();
        animator.SetFloat("hitSpeed", hitSpeed);
        weapon.AttackDuration = weapon.AttackDuration / hitSpeed;
        agent.autoTraverseOffMeshLink = false;

        // next line is for debugging, TODO: DELETE
        player = GameObject.FindObjectOfType<Camera>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        switch (state)
        {
            case State.CHASE:
                Chase();
                break;
            case State.IDLE:
                break;
            case State.ATTACK:
                Attack();
                break;
        }
        StartCoroutine(CheckOffMeshLinkMove());
    }

    private void Chase()
    {
        if (Vector3.Distance(player.transform.position, transform.position) < 3)
        {

            agent.isStopped = true;
            state = State.ATTACK;
            return;
        }
        agent.isStopped = false;
        agent.destination = player.transform.position;
        animator.SetBool("isWalking", true);
    }

    private void Attack()
    {
        weapon.Attack();
        animator.SetBool("isAttacking", true);
        state = State.IDLE;
        StartCoroutine(WaitAttack());
    }

    private IEnumerator WaitAttack()
    {
        yield return null;
        animator.SetBool("isAttacking", false);
        yield return new WaitForSeconds(weapon.AttackDuration);
        state = State.CHASE;
    }

    public OffMeshLinkMoveMethod method = OffMeshLinkMoveMethod.Parabola;
    public AnimationCurve curve = new AnimationCurve();
    private IEnumerator CheckOffMeshLinkMove()
    {
        Debug.Log("CHECK OFFMESH");
        if (agent.isOnOffMeshLink)
        {
            Debug.Log("ON OFFMESH LINK");
            if (method == OffMeshLinkMoveMethod.NormalSpeed)
                yield return StartCoroutine(NormalSpeed(agent));
            else if (method == OffMeshLinkMoveMethod.Parabola)
                yield return StartCoroutine(Parabola(agent, 2.0f, 0.5f));
            else if (method == OffMeshLinkMoveMethod.Curve)
                yield return StartCoroutine(Curve(agent, 0.5f));
            agent.CompleteOffMeshLink();
        }
        yield return null;
    }
    private IEnumerator NormalSpeed(NavMeshAgent agent)
    {
        OffMeshLinkData data = agent.currentOffMeshLinkData;
        Vector3 endPos = data.endPos + Vector3.up * agent.baseOffset;
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
        Vector3 endPos = data.endPos + Vector3.up * agent.baseOffset;
        float normalizedTime = 0.0f;
        while (normalizedTime < 1.0f)
        {
            float yOffset = height * 4.0f * (normalizedTime - normalizedTime * normalizedTime);
            agent.transform.position = Vector3.Lerp(startPos, endPos, normalizedTime) + yOffset * Vector3.up;
            normalizedTime += Time.deltaTime / duration;
            yield return null;
        }
    }
    private IEnumerator Curve(NavMeshAgent agent, float duration)
    {
        OffMeshLinkData data = agent.currentOffMeshLinkData;
        Vector3 startPos = agent.transform.position;
        Vector3 endPos = data.endPos + Vector3.up * agent.baseOffset;
        float normalizedTime = 0.0f;
        while (normalizedTime < 1.0f)
        {
            float yOffset = curve.Evaluate(normalizedTime);
            agent.transform.position = Vector3.Lerp(startPos, endPos, normalizedTime) + yOffset * Vector3.up;
            normalizedTime += Time.deltaTime / duration;
            yield return null;
        }
    }
}

public enum OffMeshLinkMoveMethod
{
    Teleport,
    NormalSpeed,
    Parabola,
    Curve
}