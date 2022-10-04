using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Composite : BT_Node
{
    public List<BT_Node> Children { get; set; }

    public Composite(BehaviourTree t, BT_Node[] nodes) : base(t)
    {
        Children = new List<BT_Node>(nodes);
    }
}
