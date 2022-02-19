using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.ProBuilder;

public class PersonalWallRun : MonoBehaviour
{

    public Transform orientation;

    public Rigidbody rb;

    public float wallDistance;
    public float wallRunPush;
    public float wallRunJumpForce;
    public float wallRunTimerDuration;
    public float wallRunCooldown;
    public float wallRunFov;
    public float camTilt;
    public float camTiltTime;
    
    public float tilt { get; set; }
    
    public Camera cam;

    public PhysicMaterial wallRunPhysicsMaterial;
    public PhysicMaterial groundPhysicsMaterial;

    public PlayerMovement playerMovement;
    
    public Transform wallRunTransform;
    
    public bool wallLeft;
    public bool wallRight;
    public bool isWallRunning;

    public Collider[] ignoreCollider;

    private Collider wallMesh = null;
    
    private float wallRunTimeLeft;
    private float currentCooldown;

    private Vector3 lastWallNormal;
    private Vector3 currentWallNormal;
    
    private void Update()
    {
        RaycastHit leftHit;
        RaycastHit rightHit;

        wallRight = Physics.Raycast(transform.position, orientation.right, out rightHit, wallDistance);
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftHit, wallDistance);

        if (!isWallRunning)
        {
            wallRunTimeLeft = Time.time + wallRunTimerDuration;
        }

        if (wallRight && !playerMovement.IsGrounded() && Time.time <= wallRunTimeLeft)
        {
            if (!isWallRunning)
            {
                wallRunTimeLeft = Time.time + wallRunTimerDuration;
                Vector3 vel = rb.velocity;
                if (rb.velocity.y < 0.5f)
                    rb.velocity = new Vector3(vel.x, 0, vel.z);
                else if (rb.velocity.y > 0) 
                    rb.velocity = new Vector3(vel.x, vel.y / 2, vel.z);
            }

            foreach (var t in ignoreCollider)
            {
                if (lastWallNormal != rightHit.normal && rightHit.collider != ignoreCollider[0])
                {
                    isWallRunning = true;

                    rb.useGravity = false;
                    rb.AddForce(-rightHit.normal * wallRunPush * Time.deltaTime, ForceMode.Impulse);

                    wallMesh = rightHit.collider;
                    wallMesh.material = wallRunPhysicsMaterial;

                    currentWallNormal = rightHit.normal;

                    cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, wallRunFov, 20f * Time.deltaTime);

                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        rb.AddForce(orientation.up * wallRunJumpForce, ForceMode.Impulse);
                        rb.AddForce(-orientation.right * wallRunJumpForce, ForceMode.Impulse);
                    }
                }
            }
            
            
        }
        else if (wallLeft && !playerMovement.IsGrounded() && Time.time < wallRunTimeLeft)
        {
            if (!isWallRunning)
            {
                wallRunTimeLeft = Time.time + wallRunTimerDuration;
                Vector3 vel = rb.velocity;
                if (rb.velocity.y < 0.5f)
                    rb.velocity = new Vector3(vel.x, 0, vel.z);
                else if (rb.velocity.y > 0) 
                    rb.velocity = new Vector3(vel.x, vel.y / 2, vel.z);
            }

            foreach (var t in ignoreCollider)
            {
                if (lastWallNormal != leftHit.normal && leftHit.collider != t)
                {
                    isWallRunning = true;

                    rb.useGravity = false;
                    rb.AddForce(-leftHit.normal * wallRunPush * Time.deltaTime, ForceMode.Impulse);

                    wallMesh = leftHit.collider;
                    wallMesh.material = wallRunPhysicsMaterial;

                    currentWallNormal = leftHit.normal;

                    cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, wallRunFov, 20f * Time.deltaTime);

                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        rb.AddForce(orientation.up * wallRunJumpForce, ForceMode.Impulse);
                        rb.AddForce(orientation.right * wallRunJumpForce, ForceMode.Impulse);
                    }
                }
            }
        }
        else
        {
            rb.useGravity = true;
            
            if (isWallRunning)
            {
                lastWallNormal = currentWallNormal;
                currentWallNormal = Vector3.zero;
                currentCooldown = Time.time + wallRunCooldown;
            }
            
            isWallRunning = false;
            
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, 90, 20f * Time.deltaTime);

            if (wallMesh != null)
            {
                wallMesh.material = groundPhysicsMaterial;
            }
        }

        if (Time.time >= currentCooldown)
        {
            lastWallNormal = Vector3.zero;
        }
    }
}