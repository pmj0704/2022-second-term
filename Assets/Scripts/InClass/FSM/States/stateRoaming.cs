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
        //A에서 B이동후 다음 이동 때 지점이 B 그대로인 문제가 있다.
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

        //else면 안된다. null일 때도 찾아서 실행해야 된다.
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
            //agent.pathPending : 더 이동할 범위가 있는가 없는가
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
