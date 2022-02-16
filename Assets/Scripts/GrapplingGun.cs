using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingGun : MonoBehaviour
{
    
    private LineRenderer lr;
    private Vector3 grapplePoint;
    private bool grappleActivated;
    private SpringJoint joint;

    public LayerMask canBeGrappled;
    public Transform gunTip, mainCamera, player;
    public GameObject claw;
    public float maxDistance = 100f;
    public bool grappleConnected;
    public ParticleSystem muzzleFlash;
    public PlayerMovement playerMovement;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1") && !grappleActivated)
        {
            StartGrapple();
        } else if (Input.GetButtonUp("Fire1"))
        {
            StopGrapple();
        }
    }

    private void LateUpdate()
    {
        DrawRope();
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
            claw.gameObject.SetActive(false);
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

            lr.positionCount = 2;
            
            playerMovement.move = false;
        }
    }

    void DrawRope()
    {
        if(!joint) return;
        
        lr.SetPosition(0, gunTip.position);
        lr.SetPosition(1, grapplePoint);
    }

    void StopGrapple()
    {
        claw.gameObject.SetActive(true);
        grappleActivated = false;
        grappleConnected = false;

        lr.positionCount = 0;

        playerMovement.move = true;
        
        Destroy(joint);
    }

    public bool IsGrappling()
    {
        return joint != null;
    }

    public Vector3 GetGrapplingPoint()
    {
        return grapplePoint;
    }
}
