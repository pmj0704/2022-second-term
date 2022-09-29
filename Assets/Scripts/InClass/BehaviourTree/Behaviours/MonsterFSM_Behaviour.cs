using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterFSM_Behaviour : MonsterFSM, IAtkAble, IDmgAble
{
    public LayerMask targetLayerMask;
    public Collider atkItemCollider;
    private GameObject atkEffectPrefab = null;

    public Transform launchWeaponTransform; //¿ø
    public Transform weaponHitTransform; //±Ù

    protected override void Start()
    {
        base.Start();
        fsmManager.AddStateList(new stateMove());
        fsmManager.AddStateList(new stateAtk());
        fsmManager.AddStateList(new stateDie());

        OnAwakeAtkBehaviour();
    }

    protected override void Update()
    {
        OnCheckAtkBehaviour();
        base.Update();
    }

    public J ChangeState<J>() where J : State<MonsterFSM>
    {
        return fsmManager.ChangeState<J>();
    }

    public override bool getFlagAtk
    {
        get
        {
            if (!base.target)
            {
                return false;
            }
            float distance = Vector3.Distance(transform.position, base.target.position);
            return (distance <= atkRange);
        }
    }

    public override Transform SearchMonster()
    {
        return base.target;
    }

    public void EnableAttackCOllider()
    {
        Debug.Log("Check Attack Event");
        if(atkItemCollider)
        {
            atkItemCollider.enabled = true;
        }

        StartCoroutine(DisableAttackCollider());
    }

    IEnumerator DisableAttackCollider()
    {
        yield return new WaitForFixedUpdate();

        if(atkItemCollider)
        {
            atkItemCollider.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(((1 << other.gameObject.layer) & targetLayerMask) != 0)
        {
            Debug.Log("Attack Trigger: " + other.name);
        }

        if(((1 << other.gameObject.layer) & targetLayerMask) == 0)
        {
            //if wasn't in an ignore layer
        }
    }

    private void OnAwakeAtkBehaviour()
    {
        foreach(AtkBehaviour behaviour in attackBehaviours)
        {
            if(nowAtkBehaviour == null)
            {
                nowAtkBehaviour = behaviour;
            }
            behaviour.targetLayerMask = targetLayerMask;
        }
    }

    public AtkBehaviour nowAtkBehaviour
    {
        get;
        private set;
    }

    [SerializeField]
    private List<AtkBehaviour> attackBehaviours = new List<AtkBehaviour>();
    public int maxHp = 100;

    public int Hp
    {
        get;
        private set;
    }
    public bool getFlagLive => Hp > 0;

    private void OnCheckAtkBehaviour()
    {
        if(nowAtkBehaviour == null || !nowAtkBehaviour.flagReady)
        {
            nowAtkBehaviour = null;
            foreach(AtkBehaviour behaviour in attackBehaviours)
            {
                if (behaviour.flagReady)
                {
                    if((nowAtkBehaviour == null) || (nowAtkBehaviour.importanceAtkNo < behaviour.importanceAtkNo))
                    {
                        nowAtkBehaviour = behaviour;
                    }
                }
            }
        }
    }

    public void OnExecuteAttack(int attackIndex)
    {
        if(nowAtkBehaviour != null && base.target != null)
        {
            nowAtkBehaviour.callAtkMotion(base.target.gameObject, launchWeaponTransform);
        }
    }

    public void setDmg(int dmg, GameObject atkEffectPrefab)
    {
        if (!getFlagLive)
        {
            return;
        }
        Hp -= dmg;
        if (atkEffectPrefab)
        {
            Instantiate(atkEffectPrefab, weaponHitTransform);
        }

        if(getFlagLive)
        {
            animator?.SetTrigger("hitTriggerHash");
        }
        else
        {
            fsmManager.ChangeState<stateDie>();
        }
    }
}
