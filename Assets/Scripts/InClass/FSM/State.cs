using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State<T>
{
    protected StateMachine<T> stateMachine;
    protected T stateMachineClass;

    public State() { }

    internal void SetMachineWithClass(StateMachine<T> stateMachine, T stateMachineClass) //internal
    {
        this.stateMachine = stateMachine;
        this.stateMachineClass = stateMachineClass;

        OnAwake();
    }

    public virtual void OnAwake() { /*Ä³½Ì*/ }
    public virtual void OnStart() { }
    public abstract void OnUpdate(float deltaTime);
    public virtual void OnEnd() { }

}
