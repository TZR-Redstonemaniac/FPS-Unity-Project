using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonalWallRun : MonoBehaviour
{

    public Transform orientation;

    public Rigidbody rb;

    public float wallDistance;
    public float wallRunGravity;
    public float wallRunPush;
    public float wallRunJumpForce;

    public PhysicMaterial wallRunPhysicsMaterial;
    public PhysicMaterial groundPhysicsMaterial;

    public PlayerMovement playerMovement;

    public Transform wallRunTransform;
    
    private bool wallLeft;
    private bool wallRight;

    private Collider wallMesh = null;

    
    private void Update()
    {
        RaycastHit leftHit;
        RaycastHit rightHit;

        wallRight = Physics.Raycast(transform.position, orientation.right, out rightHit, wallDistance);
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftHit, wallDistance);

        if (wallRight && !playerMovement.grounded)
        {
            rb.useGravity = false;
            rb.AddForce(orientation.right * wallRunPush * Time.deltaTime, ForceMode.Force);
            rb.AddForce(-orientation.up * wallRunGravity * Time.deltaTime, ForceMode.Acceleration);

            wallMesh = rightHit.collider;
            wallMesh.material = wallRunPhysicsMaterial;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb.AddForce(orientation.up * wallRunJumpForce, ForceMode.Impulse);
                rb.AddForce(orientation.up * wallRunJumpForce, ForceMode.Impulse);
                rb.AddForce(-orientation.right * wallRunJumpForce, ForceMode.Impulse);
            }
        }
        else if (wallLeft && !playerMovement.grounded)
        {
            rb.useGravity = false;
            rb.AddForce(-orientation.right * wallRunPush * Time.deltaTime, ForceMode.Force);
            rb.AddForce(-orientation.up * wallRunGravity * Time.deltaTime, ForceMode.Acceleration);

            wallMesh = leftHit.collider;
            wallMesh.material = wallRunPhysicsMaterial;

            wallRunTransform.right = leftHit.normal;
            wallRunTransform.right = -wallRunTransform.right;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb.AddForce(orientation.up * wallRunJumpForce, ForceMode.Impulse);
                rb.AddForce(orientation.up * wallRunJumpForce, ForceMode.Impulse);
                rb.AddForce(orientation.right * wallRunJumpForce, ForceMode.Impulse);
            }
        }
        else
        {
            rb.useGravity = true;

            if (wallMesh != null)
            {
                wallMesh.material = groundPhysicsMaterial;
            }
        }
    }
}