using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stateDie : State<MonsterFSM>
{
    private Animator animator;

    protected int flagLive = Animator.StringToHash("flagLive");

    public override void OnAwake()
    {
        animator = stateMachineClass.GetComponent<Animator>();
    }

    public override void OnStart()
    {
        animator?.SetBool(flagLive, false);
    }

    public override void OnUpdate(float deltaTime)
    {
        if (stateMachine.getStateDurationTime > 3.0f) GameObject.Destroy(stateMachineClass.gameObject);
    }

    public override void OnEnd() {}
}
