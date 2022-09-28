using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stateIdle : State<MonsterFSM> 
{
    private Animator animator;
    private CharacterController characterController;

    protected int hashMove = Animator.StringToHash("Move");
    protected int hashMoveSpd = Animator.StringToHash("MoveSpd");

    public override void OnAwake()
    {
        animator = stateMachineClass.GetComponent<Animator>();
        characterController = stateMachineClass.GetComponent<CharacterController>();
    }

    public override void OnStart()
    {
        animator?.SetBool(hashMove, false);
        animator?.SetFloat(hashMoveSpd, 0);
        characterController?.Move(Vector3.zero);
    }

    public override void OnUpdate(float deltaTime)
    {
        Transform target = stateMachineClass.SearchMonster();
        if (target){
            if (stateMachineClass.getFlagAtk){
                stateMachine.ChangeState<stateAtk>();
            } else {
                stateMachine.ChangeState<stateMove>();
            }
        }
    }

    public override void OnEnd(){}

}
