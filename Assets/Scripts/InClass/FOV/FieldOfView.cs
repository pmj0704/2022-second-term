using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    [Header("Sight Elements")]
    public float eyeRadius = 5f;

    [Range(0, 360)]
    public float eyeAngle = 90f;

    [Header("Search Elements")]
    public float delayFindTime = 0.2f;

    public LayerMask targetLayerMask;
    public LayerMask blockLayerMask; //장애물ㅌ

    private List<Transform> targetList = new List<Transform>();
    private Transform firstTraget;
    private float distanceTarget = 0.0f;

    public List<Transform> TargetLists => targetList;
    public Transform FirstTarget => firstTraget;
    public float DistanceTarget => distanceTarget;

    void FindTargets()
    {
        distanceTarget = 0.0f;
        firstTraget = null;
        targetList.Clear();

        Collider[] overlapSphereTargets = Physics.OverlapSphere(transform.position, eyeRadius, targetLayerMask);

        //각도 상상
        for (int i = 0; i < overlapSphereTargets.Length; i++)
        {
            Transform target = overlapSphereTargets[i].transform;

            Vector3 LookAtTarget = (target.position - transform.position).normalized; //타겟에 대한 몬스터 방향이 나옴

            if (Vector3.Angle(transform.forward, LookAtTarget) < eyeAngle / 2)
            {
                float firstTargetDistance = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, LookAtTarget, firstTargetDistance, blockLayerMask))
                {
                    targetList.Add(target);

                    if (firstTraget == null || (distanceTarget) > firstTargetDistance)
                    {
                        firstTraget = target;
                        distanceTarget = firstTargetDistance;
                    }
                }
            }
        }
    }

    private void Start()
    {
        StartCoroutine(updateFindTarget(delayFindTime));
    }

    IEnumerator updateFindTarget(float delay)
    {
        while(true)
        {
            yield return new WaitForSeconds(delay);
            FindTargets();
        }
    }

    public Vector3 getVecByAngle(float degrees, bool flagGlobalAngle)
    {
        if(!flagGlobalAngle)
        {
            degrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(degrees * Mathf.Deg2Rad), 0, Mathf.Cos(degrees * Mathf.Deg2Rad));
    }
}
