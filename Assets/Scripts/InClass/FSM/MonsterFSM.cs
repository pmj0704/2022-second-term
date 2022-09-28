using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterFSM : MonoBehaviour
{
    protected StateMachine<MonsterFSM> fsmManager;

    public StateMachine<MonsterFSM> FsmManager => fsmManager;

    protected UnityEngine.AI.NavMeshAgent agent;
    protected Animator animator;

    private FieldOfView fov;
    public Transform target => fov?.FirstTarget;

    public Transform[] posTargets;
    public Transform posTarget = null;
    private int posTargetsIdx = 0;

    protected virtual void Start()
    {
        fsmManager = new StateMachine<MonsterFSM>(this, new stateIdle());
        fsmManager.AddStateList(new stateMove());
        fsmManager.AddStateList(new stateAtk());

        fov = GetComponent<FieldOfView>();
    }

    protected virtual void Update()
    {
        fsmManager.Update(Time.deltaTime);
        Debug.Log(fsmManager.getNowState);
    }

    public Transform SearchNextTargetPosition()
    {
        posTarget = null;
        if(posTargets.Length > 0)
        {
            posTarget = posTargets[posTargetsIdx];
        }
        posTargetsIdx = (posTargetsIdx + 1) % posTargets.Length;
        return posTarget;
    }

    public virtual Transform SearchMonster()
    {
        /*target = null;

        Collider[] findTargets = Physics.OverlapSphere(transform.position, eyeSight, targetLayerMask);

        if(findTargets.Length > 0)
        {
            target = findTargets[0].transform;
        }*/

        return target;
    }

    public float atkRange;

    public virtual bool getFlagAtk
    {
        get
        {
            if (!target) return false;

            float distance = Vector3.Distance(transform.position, target.position);

            return (distance <= atkRange);
        }
    }

    /*private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, eyeSight);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, atkRange);
    }*/
}
