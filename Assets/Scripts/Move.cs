using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public float spd = 5f;
    public float jumpPower = 3f;
    public float dashPower = 500f;
    public float distance = 1f;
    public bool isRigidbody = true;

    bool isJumping = false;
    
    float horizontal;
    float vertical;

    private CharacterController characterCtrl = null;
    private Rigidbody myRigidbody = null;
    private CollisionFlags collisionFlags = CollisionFlags.None;


    void Start()
    {
        characterCtrl = GetComponent<CharacterController>();
        myRigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        if (isRigidbody)
        {
            characterCtrl.enabled = false;

            Vector3 inputVec = new Vector3(horizontal, 0, vertical);
            myRigidbody.MovePosition(transform.position + inputVec * Time.deltaTime * spd);
            if (Input.GetButtonDown("Jump") && !isJumping)
            {
                isJumping = true;
                myRigidbody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            }

            RaycastHit hit;

            if(Physics.Raycast(transform.position, Vector3.down, out hit, distance))
            {
                if (hit.transform.CompareTag("Ground"))
                {
                    isJumping = false;
                }
                else
                {
                    isJumping = true;
                }
            }

            if (Input.GetButtonDown("Fire1")) myRigidbody.AddForce(transform.forward * dashPower, ForceMode.Impulse);
                //myRigidbody.AddExplosionForce(dashPower, -transform.forward, 5f, 3f);
        }
        else
        {
            characterCtrl.enabled = true;

            Transform cameraTransform = Camera.main.transform;

            Vector3 forward = cameraTransform.TransformDirection(Vector3.forward);
            forward.y = 0.0f;

            Vector3 right = new Vector3(forward.z, 0.0f, -forward.x);

            Vector3 targetDirection = vertical * forward + horizontal * right;

            Vector3 moveAmount = targetDirection * spd * Time.deltaTime;

            collisionFlags = characterCtrl.Move(moveAmount);
        }
    }
}
