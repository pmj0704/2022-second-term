using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlyJump : MonoBehaviour
{
    private Rigidbody myRigidbody = null;

    private void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        float jump = Input.GetAxis("Jump");

        Vector3 vecJump = new Vector3(0, jump, 0);

        myRigidbody.AddForce(vecJump, ForceMode.Impulse);
    }
}
