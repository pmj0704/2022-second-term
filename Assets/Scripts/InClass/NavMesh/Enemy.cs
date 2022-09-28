using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    private NavMeshAgent agent;
    private CharacterController characterController;
    public Transform playerTransform;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        agent = GetComponent<NavMeshAgent>();
        agent.updatePosition = false;
        agent.updateRotation = true;
    }

    void Update()
    {
        agent.SetDestination(playerTransform.position);

        if (agent.remainingDistance > agent.stoppingDistance)
        {
            characterController.Move(agent.velocity * Time.deltaTime);
        }
        else
        {
            characterController.Move(Vector3.zero);
        }
    }

    private void LateUpdate()
    {
        transform.position = agent.nextPosition;
    }
}
