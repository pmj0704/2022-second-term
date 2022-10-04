using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repeater : Decorator
{
    public Repeater(BehaviourTree t, BT_Node c) : base(t,c)
    { }

    public override Result Execute()
    {
        Debug.Log("Child returned " + Child.Execute());
        return Result.Running;
    }
}
