using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class GrapplingGun : MonoBehaviour
{
    
    [Header("Transform")]
    public Transform gunTip;
    public Transform player;
    public Transform mainCamera;
    
    
    [Header("Assignable")]
    public LayerMask canBeGrappled;
    public ParticleSystem muzzleFlash;
    public PlayerMovement playerMovement;

    [Header("Values")]
    public float maxDistance = 100f;

    [HideInInspector] public bool grappleConnected;
    
    [HideInInspector]public Vector3 grapplePoint;
    private SpringJoint joint;
    
    private bool grappleActivated;

    private void Update()
    {
        if (Input.GetButtonDown("Ability 1") && !grappleActivated)
        {
            StartGrapple();
        } else if (Input.GetButtonUp("Ability 1"))
        {
            StopGrapple();
        }
    }

    void StartGrapple()
    {
        grappleActivated = true;
        
        RaycastHit hit;

        if (Physics.Raycast(mainCamera.position, mainCamera.forward, out hit, maxDistance, canBeGrappled))
        {
            if (!grappleConnected)
            {
                muzzleFlash.Play();
            }
            
            grappleConnected = true;
            grapplePoint = hit.point;
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            float distanceFromPoint = Vector3.Distance(player.position, grapplePoint);

            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = distanceFromPoint * 0.25f;

            joint.spring = 4.5f;
            joint.damper = 7f;
            joint.massScale = 4.5f;

            joint.connectedBody = hit.rigidbody;

            playerMovement.move = false;
        }
    }

    void StopGrapple()
    {
        grappleActivated = false;
        grappleConnected = false;

        playerMovement.move = true;

        Destroy(joint);
    }

    public bool IsGrappling()
    {
        return joint != null;
    }

    public Vector3 GetGrapplePoint()
    {
        return grapplePoint;
    }
}
