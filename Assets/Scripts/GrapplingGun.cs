using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrapplingGun : MonoBehaviour
{
    
    private LineRenderer lr;
    private Vector3 grapplePoint;
    private bool grapplePressed;
    private bool grappleActivated;
    private SpringJoint joint;
    
    public LayerMask canBeGrappled;
    public Transform gunTip, mainCamera, player;
    public GameObject claw;
    public float maxDistance = 100f;
    public PlayerInput playerInput;
    public bool grappleConnected;
    public ParticleSystem muzzleFlash;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (grapplePressed && !grappleActivated)
        {
            StartGrapple();
        } else if (!grapplePressed)
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
        Destroy(joint);
    }
    
    public void Grapple(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            grapplePressed = true;
        }

        if (context.canceled)
        {
            grapplePressed = false;
        }
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
