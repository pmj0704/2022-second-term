using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stateIdle : State<MonsterFSM> 
{
    private Animator animator;
    private CharacterController characterController;

    protected int hashMove = Animator.StringToHash("Move");
    protected int hashMoveSpd = Animator.StringToHash("MoveSpd");

    public bool flagRoaming = false;
    private float roamingStateMinIdleTime = 0.0f;
    private float roamingStateMaxIdleTime = 3.0f;
    private float roamingStateIdleTime = 0.0f;

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

        if(flagRoaming)
        {
            roamingStateIdleTime = Random.Range(roamingStateMinIdleTime, roamingStateMaxIdleTime);
        }
    }

    public override void OnUpdate(float deltaTime)
    {
        Transform target = stateMachineClass.SearchMonster();
        if (target.tag == "Player"){
            if (stateMachineClass.getFlagAtk){
                stateMachine.ChangeState<stateAtk>();
            } else {
                stateMachine.ChangeState<stateMove>();
            }
        }
        else if(flagRoaming && stateMachine.getStateDurationTime > roamingStateIdleTime)
        {
            stateMachine.ChangeState<stateRoaming>();
        }
    }

    public override void OnEnd(){}

}
