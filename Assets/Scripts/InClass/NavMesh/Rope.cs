using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Rope : MonoBehaviour
{
    public int meshCost = 3;
    public float spd = 1.0f;
    private NavMeshAgent navMeshAgent;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        StartCoroutine(Upping());
    }

    IEnumerator Upping()
    {
        while(true)
        {
            //getIsMeshUp 참 일 때까지
            yield return new WaitUntil(() => getIsMeshUp());

            //action
            yield return StartCoroutine(UpAndDown());
        }
    }

    public bool getIsMeshUp()
    {
        //agent is OffMeshLink ? true : false
        if(navMeshAgent.isOnOffMeshLink)
        {
            //currentOffMeshLinkData ? 수동 : 자동
            OffMeshLinkData offMeshLinkData = navMeshAgent.currentOffMeshLinkData;

            if (offMeshLinkData.offMeshLink != null)
            {
                Debug.Log("Mesh Cost" + offMeshLinkData.offMeshLink.area);
                Debug.Log("Has OffMeshLink");
                //Mesh cost가 왜 6인지 모르겠습니다.
            }

            

            if (offMeshLinkData.offMeshLink != null && offMeshLinkData.offMeshLink.area == meshCost)
            {
                Debug.Log("A");
                return true;
            }
        }
        return false;
    }

    public IEnumerator UpAndDown()
    {
        //stop navigation
        navMeshAgent.isStopped = true;

        OffMeshLinkData offMeshLinkData = navMeshAgent.currentOffMeshLinkData;
        Vector3 posStart = offMeshLinkData.startPos;
        Vector3 posEnd = offMeshLinkData.endPos;

        //time setting : 거리 = 시간 * 속도 => 시간 = 거리 / 속도
        float upTime = Mathf.Abs(posEnd.y - posStart.y) / spd;
        float settingTime = 0.0f;
        float rateLength = 0.0f;

        while(rateLength < 1)
        {
            //deltaTime 타임은 소수 점이 없어서 쓕 올라감 1초 == 1, 1초후 100%
            settingTime += Time.deltaTime;
            rateLength = settingTime / Time.deltaTime;
            transform.position = Vector3.Lerp(posStart, posEnd, rateLength);
            yield return null;
        }

        //end move
        navMeshAgent.CompleteOffMeshLink();
        //navigation start
        navMeshAgent.isStopped = false;
    }
}
