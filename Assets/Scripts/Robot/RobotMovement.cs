using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class RobotMovement : MonoBehaviour
{

    [SerializeField] private Camera cam;
    [SerializeField] private ThirdPersonCharacter character;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform playerHeadTransform;
    [SerializeField] private Transform headTransform;
    [SerializeField] private Transform lookTarget;
    [SerializeField] private Transform[] points;
    [SerializeField] private float findRadius;
    [SerializeField] private float playerStoppingDistance;
    [SerializeField] private LayerMask playerLayerMask;
    
    private bool followPlayer;
    private int destPoint = 0;
    private float originalStoppingDistance;
    private Vector3 originalTargetLocation;

    private void Start()
    {
        agent.updateRotation = false;
        character = GetComponent<ThirdPersonCharacter>();

        originalStoppingDistance = agent.stoppingDistance;
        originalTargetLocation = lookTarget.localPosition;
        
        GotoNextPoint();
    }

    // Update is called once per frame
    void Update()
    {
        if (followPlayer)
        {
            agent.SetDestination(playerTransform.position);
            agent.stoppingDistance = playerStoppingDistance;
            lookTarget.position = playerHeadTransform.position;
        }
        else
        {
            agent.stoppingDistance = originalStoppingDistance;
            lookTarget.localPosition = originalTargetLocation;
        }
        
        if (!agent.pathPending && agent.remainingDistance < agent.stoppingDistance && !followPlayer)
            GotoNextPoint();

        Move();
        CheckForPlayer();
    }
    
    void GotoNextPoint() {
        // Returns if no points have been set up
        if (points.Length == 0)
            return;

        // Set the agent to go to the currently selected destination.
        agent.destination = points[destPoint].position;

        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        destPoint = (destPoint + 1) % points.Length;
    }

    void CheckForPlayer()
    {
        var colliders = Physics.OverlapSphere(headTransform.position, findRadius, playerLayerMask);

        if (colliders.Length >= 1)
            followPlayer = true;
        else
            followPlayer = false;
    }

    void Move()
    {
        //Move
        if (agent.remainingDistance > agent.stoppingDistance)
            character.Move(agent.desiredVelocity, false, false);
        else
            character.Move(Vector3.zero, false, false);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(headTransform.position, findRadius);
    }
}
