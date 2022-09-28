using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleStateScript : StateMachineBehaviour
{
    public int idleIDCount = 2; // 0, 1, 2

    public float minBaseIdleTime = 0f;
    public float maxBaseIdleTime = 5f;
    protected float rndBaseIdleTime;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rndBaseIdleTime = Random.Range(minBaseIdleTime, maxBaseIdleTime);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(animator.IsInTransition(0) && animator.GetCurrentAnimatorStateInfo(0).fullPathHash == stateInfo.fullPathHash)
        {
            animator.SetInteger("Idle_ID", -1);
        }

        if(stateInfo.normalizedTime > rndBaseIdleTime && !animator.IsInTransition(0))
        {
            animator.SetInteger("Idle_ID", Random.Range(0, idleIDCount));
        }
    }

    //Animator.IsInTransition(0) 화살표 시작에 있는 스테이트 (베이스 레이어를 뜻함) 
    //(0)fullPathHash 베이스 레이어의 주소값
    //true 베이스 레이어에 있다.
    //-1은 하지말라 뜻
}
