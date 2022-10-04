using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomWalk : BT_Node
{
    protected Vector3 NextDestination { get; set; }
    public float spd = 3.0f;

    public RandomWalk(BehaviourTree t) : base(t)
    {
        NextDestination = Vector3.zero;
        FindNextDestination();
    }

    public bool FindNextDestination()
    {
        object obj;
        bool found = false;
        found = Tree.BlackBoard.TryGetValue("WorldBounds", out obj);
        if(found)
        {
            Rect bounds = (Rect)obj;
            float x = Random.value * bounds.width;
            float y = Random.value * bounds.height;
            NextDestination = new Vector3(x, y, NextDestination.z);
        }
        return found;
    }

    public override Result Execute()
    {
        if(Tree.gameObject.transform.position == NextDestination)
        {
            if(!FindNextDestination())
            {
                return Result.Fail;
            }
            else
            {
                return Result.Success;
            }
        }
        else
        {
            Tree.gameObject.transform.position = Vector3.MoveTowards(Tree.gameObject.transform.position, NextDestination
                , Time.deltaTime * spd);
            return Result.Running;
        }
    }
}
