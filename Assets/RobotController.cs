using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobotController : MonoBehaviour
{

    [SerializeField] private Camera cam;

    [SerializeField] private NavMeshAgent agent;

    [SerializeField] private Transform playerTransform;
    [SerializeField] private bool followPlayer;
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && !followPlayer)
        {
            //Get where we clicked
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            //Move the agent if we clicked something
            if (Physics.Raycast(ray, out hit))
                agent.SetDestination(hit.point);
        }
        
        if(followPlayer)
            agent.SetDestination(playerTransform.position);
    }
}
