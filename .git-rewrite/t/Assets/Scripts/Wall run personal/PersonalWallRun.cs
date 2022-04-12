using System;
using System.Collections;
using System.Collections.Generic;
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
    
    public float tilt { get; private set; }
    
    public Camera cam;

    public PhysicMaterial wallRunPhysicsMaterial;
    public PhysicMaterial groundPhysicsMaterial;

    public PlayerMovement playerMovement;
    
    public Transform wallRunTransform;
    
    private bool wallLeft;
    private bool wallRight;
    private bool isWallRunning;

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

        if (wallRight && !playerMovement.grounded && Time.time <= wallRunTimeLeft)
        {
            if (!isWallRunning)
            {
                wallRunTimeLeft = Time.time + wallRunTimerDuration;
                rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y * 0.5f, rb.velocity.z);
            }
            
            if (lastWallNormal != rightHit.normal)
            {
                isWallRunning = true;

                rb.useGravity = false;
                rb.AddForce(-rightHit.normal * wallRunPush * Time.deltaTime, ForceMode.Impulse);

                wallMesh = rightHit.collider;
                wallMesh.material = wallRunPhysicsMaterial;

                currentWallNormal = rightHit.normal;

                cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, wallRunFov, 20f * Time.deltaTime);
                tilt = Mathf.Lerp(tilt, camTilt, camTiltTime * Time.deltaTime);

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    rb.AddForce(orientation.up * wallRunJumpForce, ForceMode.Impulse);
                    rb.AddForce(-orientation.right * wallRunJumpForce, ForceMode.Impulse);
                }   
            }
        }
        else if (wallLeft && !playerMovement.grounded && Time.time < wallRunTimeLeft)
        {
            
            if (!isWallRunning)
            {
                wallRunTimeLeft = Time.time + wallRunTimerDuration;
                rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            }
            
            if (lastWallNormal != leftHit.normal)
            {
                isWallRunning = true;
                
                rb.useGravity = false;
                rb.AddForce(-leftHit.normal * wallRunPush * Time.deltaTime, ForceMode.Impulse);

                wallMesh = leftHit.collider;
                wallMesh.material = wallRunPhysicsMaterial;

                currentWallNormal = leftHit.normal;
                
                cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, wallRunFov, 20f * Time.deltaTime);
                tilt = Mathf.Lerp(tilt, -camTilt, camTiltTime * Time.deltaTime);
                
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    rb.AddForce(orientation.up * wallRunJumpForce, ForceMode.Impulse);
                    rb.AddForce(orientation.right * wallRunJumpForce, ForceMode.Impulse);
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
            tilt = Mathf.Lerp(tilt, 0, camTiltTime * Time.deltaTime);

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