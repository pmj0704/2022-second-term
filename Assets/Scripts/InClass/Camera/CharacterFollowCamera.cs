using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterFollowCamera : MonoBehaviour
{
    public float camHeight = 5f;
    public float camDistance = 10f;
    public float camAngle = 45f;
    public float camFollowTime = 0.5f;

    public float targetHeight = 2f;

    private Vector3 refVelocity;

    public Transform camTarget;

    private void LateUpdate()
    {
        TopDownFollowCamera();
    }

    public void TopDownFollowCamera()
    {
        if (!camTarget) return; 

        Vector3 posCam = (Vector3.forward * -camDistance) + (Vector3.up * camHeight); 
        Debug.DrawLine(camTarget.position, posCam, Color.red); 

        Vector3 posRotated = Quaternion.AngleAxis(camAngle, Vector3.up) * posCam; 
        Debug.DrawLine(camTarget.position, posRotated, Color.green);

        Vector3 posCamTarget = camTarget.position;
        posCamTarget.y += targetHeight;

        Vector3 lastPosCam = posCamTarget + posRotated;
        Debug.DrawLine(camTarget.position, lastPosCam, Color.blue);

        transform.position = Vector3.SmoothDamp(transform.position, lastPosCam, ref refVelocity, camFollowTime);
        transform.LookAt(camTarget.position);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if(camTarget)
        {
            Vector3 posEnd = camTarget.position;
            posEnd.y += targetHeight;

            Gizmos.DrawLine(transform.position, posEnd);
            Gizmos.DrawSphere(posEnd, 0.25f);
        }

        Gizmos.DrawSphere(transform.position, 0.25f);
    }
}
