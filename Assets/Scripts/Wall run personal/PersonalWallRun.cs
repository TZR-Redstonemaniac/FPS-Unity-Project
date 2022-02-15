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

    public PlayerMovement playerMovement;

    private bool wallLeft;
    private bool wallRight;
    
    private void Update()
    {
        RaycastHit leftHit;
        RaycastHit rightHit;

        wallRight = Physics.Raycast(transform.position, orientation.right, out rightHit, wallDistance);
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftHit, wallDistance);

        if (wallRight && !playerMovement.grounded)
        {
            rb.useGravity = false;
            rb.AddForce(Vector3.right * wallRunPush * Time.deltaTime, ForceMode.Force);
            rb.AddForce(Vector3.down * wallRunGravity * Time.deltaTime);
        }
        else if (wallLeft && !playerMovement.grounded)
        {
            rb.useGravity = false;
            rb.AddForce(Vector3.left * wallRunPush * Time.deltaTime, ForceMode.Force);
            rb.AddForce(Vector3.down * wallRunGravity * Time.deltaTime);
        }
        else
        {
            rb.useGravity = true;
        }
    }
}