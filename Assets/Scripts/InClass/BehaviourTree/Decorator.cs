using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decorator : BT_Node
{
    public BT_Node Child { get; set; }

    public Decorator(BehaviourTree t, BT_Node c) : base(t)
    {
        Child = c;
    }
}
