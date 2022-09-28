using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlyMove : MonoBehaviour
{
    private Rigidbody myRigidbody = null;

    float horizontal;
    float vertical;

    public float spd = 5f;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        Vector3 inputVec = new Vector3(horizontal, 0, vertical);
        myRigidbody.MovePosition(transform.position + inputVec * Time.deltaTime * spd);
    }
}
