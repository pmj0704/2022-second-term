using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequencer : Composite
{
    private int nowNode = 0;

    public Sequencer(BehaviourTree t, BT_Node[] children) : base(t, children) { }

    public override Result Execute() 
    { 
        if(nowNode < Children.Count)
        {
            Result result = Children[nowNode].Execute();
            if(result == Result.Running)
            {
                return Result.Running;
            }
            else if(result == Result.Fail)
            {
                nowNode = 0;
                return Result.Fail;
            }
            else
            {
                nowNode++;
                if(nowNode < Children.Count) { return Result.Running; }
                else
                {
                    nowNode = 0;
                    return Result.Success;
                }
            }
        }

        return Result.Success;
    }
}
