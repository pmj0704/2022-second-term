using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourTree : MonoBehaviour
{
    private BT_Node mRoot;
    private bool startBehaviour;
    private Coroutine behaviour;

    public Dictionary<string, object> BlackBoard { get; set; }
    public BT_Node Root { get { return mRoot; } }

    void Start()
    {
        BlackBoard = new Dictionary<string, object>();
        BlackBoard.Add("WorldBounds", new Rect(0, 0, 5, 5));
        startBehaviour = false;

        //mRoot = new BT_Node(this);
        mRoot = new Repeater(this, new Sequencer(this, new BT_Node[] { new RandomWalk(this) }));
    }

    void Update()
    {
        if(!startBehaviour)
        {
            behaviour = StartCoroutine(RunBehaviour());
            startBehaviour = true;
        }
    }

    private IEnumerator RunBehaviour()
    {
        BT_Node.Result result = Root.Execute();
        while(result == BT_Node.Result.Running)
        {
            Debug.Log("Root result : " + result);
            yield return null;
            result = Root.Execute();
        }
        Debug.Log("Behaviour has finished with : " + result);
    }
}
