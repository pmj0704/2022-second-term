using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(NavMeshAgent))]
public class NavClickToMove : MonoBehaviour
{
    private CharacterController characterController; //CharacterController Cashing Ready

    //Navmesh 속성들
    private NavMeshAgent agent;

    //Navmesh에 있는 속성들
    //private float spd = 0.2f;
    //private Vector3 directionValue = Vector3.zero;
    //private float gravity = -9.81f;

    private Vector3 calcVelocity = Vector3.zero;

    public LayerMask layerGround;
    private bool flagOnGrounded = true;
    private float defaultGroundDistance = 0.2f; //땅에 있는지 판단 기준값

    public Animator animator;

    void Start()
    {
        characterController = GetComponent<CharacterController>();//Cashing
        agent = GetComponent<NavMeshAgent>();
        agent.updatePosition = false;
        agent.updateRotation = true;
    }

    void Update() //로직을 위한 Update
    {
        flagOnGrounded = characterController.isGrounded;

        if (flagOnGrounded && calcVelocity.y < 0)
        {
            calcVelocity.y = 0.0f;
        }

        if (Input.GetMouseButton(1))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
            {
                //directionValue = new Vector3(hit.point.x, 0, hit.point.z);
                agent.SetDestination(hit.point);
            }
        }

        /*
        사용자 입력이  있다면, 현재 방향을 입력 값 방향으로 변경하라
        var _dirction = directionValue - transform.position;

        if (directionValue != Vector3.zero)
        {
            transform.forward = _dirction;
        }

        characterController.Move(directionValue * Time.deltaTime * spd);


        calcVelocity.y += gravity * Time.deltaTime;
        calcVelocity.x /= 1 + drags.x * Time.deltaTime;
        calcVelocity.y /= 1 + drags.y * Time.deltaTime;
        calcVelocity.z /= 1 + drags.z * Time.deltaTime;
        if (Vector3.Distance(transform.position, directionValue) > 0.5f)
        {
            characterController.Move(_dirction.normalized * Time.deltaTime * spd);
        }
        */

        if (agent.remainingDistance > agent.stoppingDistance)
        {
            characterController.Move(agent.velocity * Time.deltaTime);
            animator.SetBool("Walk", true);
        }
        else
        {
            characterController.Move(Vector3.zero);
            animator.SetBool("Walk", false);
        }
    }

    private void LateUpdate()
    {
        transform.position = agent.nextPosition;
    }
}
