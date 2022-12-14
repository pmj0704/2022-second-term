using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class stateMove : State<MonsterFSM>
{
    private Animator animator;
    private CharacterController characterController;
    private NavMeshAgent agent;

    private int hashMove = Animator.StringToHash("Move");
    private int hashMoveSpd = Animator.StringToHash("MoveSpd");

    public override void OnAwake()
    {   
        agent = stateMachineClass.GetComponent<NavMeshAgent>();
    }

    public override void OnStart()
    {
        agent?.SetDestination(stateMachineClass.target.position);
    }

    public override void OnUpdate(float deltaTime)
    {
        Transform target = stateMachineClass.SearchMonster();
        if (target)
        {
            agent.SetDestination(stateMachineClass.target.position);
            if(agent.remainingDistance > agent.stoppingDistance)
            {
                characterController.Move(agent.velocity * Time.deltaTime);
                animator.SetFloat(hashMoveSpd, agent.velocity.magnitude / agent.speed, 0.1f, Time.deltaTime);
                return;
            }
        }

        stateMachine.ChangeState<stateIdle>();
    }

    public override void OnEnd()
    {
        agent.ResetPath();
    }
}
