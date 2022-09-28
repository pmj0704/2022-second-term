using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class ClickToMove : MonoBehaviour
{
    private CharacterController characterController; //CharacterController Cashing Ready
    private float spd = 0.2f;

    private float jumpValue = 2.0f;
    private float dashValue = 5.0f;

    private Vector3 directionValue = Vector3.zero;

    private float gravity = -9.81f;
    public Vector3 drags;
    private Vector3 calcVelocity = Vector3.zero;

    public LayerMask layerGround;
    private bool flagOnGrounded;
    private float defaultGroundDistance = 0.2f; //땅에 있는지 판단 기준값

    void CheckGroundStatus()
    {
        RaycastHit hitInfo;

        Vector3 posOrigin = transform.position + (Vector3.up * 0.1f);

        Debug.DrawLine(posOrigin, transform.position + (Vector3.up * 0.1f) + (Vector3.down * defaultGroundDistance));

        if (Physics.Raycast(posOrigin, Vector3.down, out hitInfo, defaultGroundDistance, layerGround)) flagOnGrounded = true;
        else flagOnGrounded = false;
    }

    void Start()
    {
        characterController = GetComponent<CharacterController>();//Cashing
    }

    void Update() //로직을 위한 Update
    {
        //CheckGroundStatus();
        
        //사용자 입력 값 받기, 좌우, 앞뒤
        //directionValue = Vector3.zero;
        //directionValue.x = Input.GetAxis("Horizontal");
        //directionValue.z = Input.GetAxis("Vertical");
        //directionValue = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")); //다를 거 없음 그냥 1줄


        flagOnGrounded = characterController.isGrounded;

        if (flagOnGrounded && calcVelocity.y < 0)
        {
            calcVelocity.y = 0.0f;
        }


        if (Input.GetMouseButton(1))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
            {
                directionValue = new Vector3(hit.point.x, 0, hit.point.z);
            }
        }

        var _dirction = directionValue - transform.position;

        //사용자 입력이  있다면, 현재 방향을 입력 값 방향으로 변경하라
        if (directionValue != Vector3.zero)
        {
            transform.forward = _dirction;
        }
        characterController.Move(directionValue * Time.deltaTime * spd);

        if (Input.GetButtonDown("Jump") && flagOnGrounded)
        {
            calcVelocity.y += Mathf.Sqrt(jumpValue - 2f * gravity);
            //Mathf.Sqrt(jumpValue - 2f * Physics.gravity.y) 점프 공식, gravity = -9.81, 포물선처럼 보이게
        }

        if (Input.GetButtonDown("Fire1"))
        {
            //점점 빨라지다 점점 원래대로

            float posDashEndX = Mathf.Log(1f / (Time.deltaTime * drags.x + 1)) / -Time.deltaTime;
            float posDashEndZ = Mathf.Log(1f / (Time.deltaTime * drags.z + 1)) / -Time.deltaTime;
            //Mathf.Log(1f / (Time.deltaTime * rigidbody.drag + 1)) / -Time.deltaTime; 자연스러운 지연

            Vector3 dashVelocity = Vector3.Scale(transform.forward, dashValue * new Vector3(posDashEndX, 0, posDashEndZ));

            calcVelocity += dashVelocity;
        }

        calcVelocity.y += gravity * Time.deltaTime;

        calcVelocity.x /= 1 + drags.x * Time.deltaTime;
        calcVelocity.y /= 1 + drags.y * Time.deltaTime;
        calcVelocity.z /= 1 + drags.z * Time.deltaTime;

        characterController.Move(calcVelocity * Time.deltaTime * spd);

        if (Vector3.Distance(transform.position, directionValue) > 0.5f)
        {
            characterController.Move(_dirction.normalized * Time.deltaTime * spd);
        }
    }
}
