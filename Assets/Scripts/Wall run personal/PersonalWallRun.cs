using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.ProBuilder;

public class PersonalWallRun : MonoBehaviour
{
    
    [Header("Transform")]
    [SerializeField] private Transform orientation;
    [SerializeField] private Transform wallRunTransform;

    [Header("Assignable")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Camera cam;
    [SerializeField] private PhysicMaterial wallRunPhysicsMaterial;
    [SerializeField] private PhysicMaterial groundPhysicsMaterial;
    [SerializeField] private Collider[] ignoreCollider;
    
    [Header("Scripts")]
    [SerializeField] private PlayerMovement playerMovement;

    [Header("Wall Run Values")]
    [SerializeField] private float wallDistance;
    [SerializeField] private float wallRunPush;
    [SerializeField] private float wallRunJumpForce;
    [SerializeField] private float wallRunTimerDuration;
    [SerializeField] private float wallRunCooldown;
    [SerializeField] private float wallRunFov;
    
    public float camTilt;
    public float camTiltTime;
    
    public float tilt { get; set; }
    
    [HideInInspector] public bool wallLeft;
    [HideInInspector] public bool wallRight;
    [HideInInspector] public bool isWallRunning;

    private Collider wallMesh = null;
    
    private float wallRunTimeLeft;
    private float currentCooldown;

    private Vector3 lastWallNormal;
    private Vector3 currentWallNormal;

    private void Start()
    {
        foreach (Collider c in gameObject.transform.parent.GetComponentsInChildren<Collider>())
        {
            ignoreCollider.Append(c);
        }
    }

    private void LateUpdate()
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
            if (!isWallRunning && Time.time < currentCooldown)
            {
                wallRunTimeLeft = Time.time + wallRunTimerDuration;
            }

            foreach (Collider collider in ignoreCollider)
            {
                if (lastWallNormal != rightHit.normal && rightHit.collider != collider)
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
            if (!isWallRunning  && Time.time < currentCooldown)
            {
                wallRunTimeLeft = Time.time + wallRunTimerDuration;
            }

            foreach (Collider collider in ignoreCollider)
            {
                if (lastWallNormal != leftHit.normal && leftHit.collider != collider)
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