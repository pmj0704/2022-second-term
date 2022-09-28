using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))] //리지드 바디가 없으면 이 스크립트는 실행이 안됨
public class RigidbodyMove : MonoBehaviour
{
    private Rigidbody rigidbody; //RigidBody Cashing Ready
    private float spd = 5.0f;
    
    private float jumpValue = 2.0f;
    private float dashValue = 5.0f;

    private Vector3 directionValue = Vector3.zero;

    public LayerMask layerGround;
    private bool flagOnGrounded;
    private float defaultGroundDistance = 0.2f; //땅에 있는지 판단 기준값

    void CheckGroundStatus()
    {
        RaycastHit hitInfo;

        Vector3 posOrigin = transform.position + (Vector3.up * 0.1f);

        Debug.DrawLine(posOrigin, transform.position + (Vector3.up * 0.1f) + (Vector3.down * defaultGroundDistance));

        if(Physics.Raycast(posOrigin, Vector3.down, out hitInfo, defaultGroundDistance, layerGround)) flagOnGrounded = true;
        else flagOnGrounded = false;
    }

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();//Cashing
    }

    void Update() //로직을 위한 Update
    {
        CheckGroundStatus();

        //사용자 입력 값 받기, 좌우, 앞뒤
        directionValue = Vector3.zero;
        directionValue.x = Input.GetAxis("Horizontal");
        directionValue.z = Input.GetAxis("Vertical");

        //사용자 입력이  있다면, 현재 방향을 입력 값 방향으로 변경하라
        if(directionValue != Vector3.zero)
        {
            transform.forward = directionValue;
        }

        if(Input.GetButtonDown("Jump") && flagOnGrounded)
        {
            rigidbody.drag = 0f;

            rigidbody.AddForce(Vector3.up * Mathf.Sqrt(jumpValue * - 2f * Physics.gravity.y), ForceMode.VelocityChange);
            //Mathf.Sqrt(jumpValue - 2f * Physics.gravity.y) 점프 공식, gravity = -9.81, 포물선처럼 보이게
        }

        if (Input.GetButtonDown("Fire1"))
        {
            //점점 빨라지다 점점 원래대로
            Debug.Log("대쉬");
            rigidbody.drag = 8f;

            float posDashEnd = Mathf.Log(1f / (Time.deltaTime * rigidbody.drag + 1)) / -Time.deltaTime;
            //Mathf.Log(1f / (Time.deltaTime * rigidbody.drag + 1)) / -Time.deltaTime; 자연스러운 지연

            Vector3 dashVelocity = Vector3.Scale(transform.forward, dashValue * new Vector3(posDashEnd, 0, posDashEnd));
            rigidbody.AddForce(dashVelocity, ForceMode.VelocityChange);
        }

    }

    private void FixedUpdate() //물리를 위한 Update
    {
        rigidbody.MovePosition(rigidbody.position + directionValue * spd * Time.fixedDeltaTime);
    }
}
