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
            //getIsMeshUp �� �� ������
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
            //currentOffMeshLinkData ? ���� : �ڵ�
            OffMeshLinkData offMeshLinkData = navMeshAgent.currentOffMeshLinkData;

            if (offMeshLinkData.offMeshLink != null)
            {
                Debug.Log("Mesh Cost" + offMeshLinkData.offMeshLink.area);
                Debug.Log("Has OffMeshLink");
                //Mesh cost�� �� 6���� �𸣰ڽ��ϴ�.
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

        //time setting : �Ÿ� = �ð� * �ӵ� => �ð� = �Ÿ� / �ӵ�
        float upTime = Mathf.Abs(posEnd.y - posStart.y) / spd;
        float settingTime = 0.0f;
        float rateLength = 0.0f;

        while(rateLength < 1)
        {
            //deltaTime Ÿ���� �Ҽ� ���� ��� �p �ö� 1�� == 1, 1���� 100%
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
