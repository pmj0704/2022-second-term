using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class stateRoaming : State<MonsterFSM>
{
    private Animator animator;
    private CharacterController controller;
    private NavMeshAgent agent;                 //Move to move

    private MonsterFSM monsterFSM;

    private int hashMove = Animator.StringToHash("Move");
    private int hashMoveSpd = Animator.StringToHash("MoveSpd");

    public override void OnAwake()
    {
        animator = stateMachineClass.GetComponent<Animator>();
        controller = stateMachineClass.GetComponent<CharacterController>();
        agent = stateMachineClass.GetComponent<NavMeshAgent>();

        monsterFSM = stateMachineClass as MonsterFSM;
    }

    public override void OnStart()
    {
        if(stateMachineClass?.posTarget == null)
        {
            stateMachineClass.SearchNextTargetPosition();
        }
        if(stateMachineClass?.posTarget)
        {
            agent.SetDestination(stateMachineClass.posTarget.position);
        }
        //A���� B�̵��� ���� �̵� �� ������ B �״���� ������ �ִ�.
        //Transform posRoaming = stateMachineClass.getPositionNextRoaming();

        //if(posRoaming)
        //{
        //    agent?.SetDestination(stateMachineClass.posRoaming.position);
        //    animator?.SetBool(hashMove, true);
        //}

        if (stateMachineClass?.posTarget == null)
        {
            stateMachineClass.SearchNextTargetPosition();
        }

        //else�� �ȵȴ�. null�� ���� ã�Ƽ� �����ؾ� �ȴ�.
        if (stateMachineClass?.posTarget)
        {
            agent?.SetDestination(stateMachineClass.posTarget.position);
            animator?.SetBool(hashMove, true);
        }
    }

    public override void OnUpdate(float deltaTime)
    {
        Transform target = stateMachineClass.SearchMonster();
        if (target.tag == "Player")
        {
            Debug.Log("a");
            if(stateMachineClass.getFlagAtk)
            {
                stateMachine.ChangeState<stateAtk>();
            }
            else
            {
                stateMachine.ChangeState<stateMove>();
            }
        }
        else
        {
            //agent.pathPending : �� �̵��� ������ �ִ°� ���°�
            if(!agent.pathPending && (agent.remainingDistance <= agent.stoppingDistance))
            {
                Transform nextRoamingPosition = stateMachineClass.SearchNextTargetPosition();
                if(nextRoamingPosition)
                {
                    agent.SetDestination(nextRoamingPosition.position);
                }

                stateMachine.ChangeState<stateIdle>();
            }
            else
            {
                controller.Move(agent.velocity * Time.deltaTime);
                animator.SetFloat(hashMoveSpd, agent.velocity.magnitude / agent.speed, .1f, Time.deltaTime);
            }
        }
    }

    

    public override void OnEnd()
    {
        agent.stoppingDistance = stateMachineClass.atkRange;
        animator?.SetBool(hashMove, false);
        agent.ResetPath();
    }

}
