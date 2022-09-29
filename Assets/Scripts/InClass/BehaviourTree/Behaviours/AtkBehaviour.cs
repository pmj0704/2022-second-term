using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AtkBehaviour : MonoBehaviour
{
#if UNITY_EDITOR
    [Multiline]//설명
    public string devComponent = "devCommponent";
#endif

    public int aniMotionIdx;
    public int importanceAtkNo; //우선순위

    public int atkDmg;
    public float atkRange = 3f;

    [SerializeField]
    private float atkCoolTime;
    protected float nowAtkCoolTime = 0.0f;

    public GameObject atkEffectPrefab;
    public bool flagReady => nowAtkCoolTime >= atkCoolTime;
    public LayerMask targetLayerMask;

    protected virtual void Start()
    {
        nowAtkCoolTime = atkCoolTime;
    }

    protected virtual void Update()
    {
        if(nowAtkCoolTime < atkCoolTime)
        {
            nowAtkCoolTime += Time.deltaTime;
        }
    }

    public abstract void callAtkMotion(GameObject target = null, Transform posAtkStart = null); //어떤 공격 로직?

}
