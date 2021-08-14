using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
public class AttackState : MonoBehaviour, IFSMState
{
    public FSMStateType StateName { get { return FSMStateType.Attack; } }
    public string AnimationAttackParamName = "Attack";
    public float EscapeDistance = 10.0f;
    public float MaxAttackDistance = 2.0f;
    public string TargetTag = "Player";
    public float DelayBetweenAttacks = 2.0f;

    private Animator ThisAnimator;
    private NavMeshAgent ThisAgent;
    private bool IsAttacking = false;
    private Transform Target;

    private void Awake()
    {
        ThisAgent = GetComponent<NavMeshAgent>();
        ThisAnimator = GetComponent<Animator>();
        Target = GameObject.FindGameObjectWithTag(TargetTag).transform;
    }

    public void DoAction()
    {
        IsAttacking = Vector3.Distance(Target.position, transform.position) < MaxAttackDistance;

        if (!IsAttacking)
        {
            ThisAgent.isStopped = false;
            ThisAgent.SetDestination(Target.position);
        }
    }

    public void OnEnter()
    {
        StartCoroutine(DoAttack());
    }

    public void OnExit()
    {
        ThisAgent.isStopped = true;
        IsAttacking = false;
        StopCoroutine(DoAttack());
    }

    private IEnumerator DoAttack()
    {
        while (true)
        {
            if (IsAttacking)
            {
                Debug.Log("Attack Player");
                ThisAnimator.SetTrigger(AnimationAttackParamName);
                ThisAgent.isStopped = true;

                yield return new WaitForSeconds(DelayBetweenAttacks);
            }

            yield return null;
        }
    }

    public FSMStateType ShouldTransitionToState()
    {
        if (Vector3.Distance(Target.position, transform.position) > MaxAttackDistance)
            return FSMStateType.Chase;

        return FSMStateType.Attack;
    }
}
