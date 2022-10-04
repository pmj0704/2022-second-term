using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_Node
{
    public enum Result
    {
        Running, Fail, Success
    };
    public BehaviourTree Tree { get; set; }
    public BT_Node(BehaviourTree t)
    {
        Tree = t;
    }
    public virtual Result Execute()
    {
        return Result.Fail;
    }
}
