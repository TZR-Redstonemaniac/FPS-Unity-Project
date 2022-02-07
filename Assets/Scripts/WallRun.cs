using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRun : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private Transform orientation;

    [Header("Detection")] 
    [SerializeField] private float wallDistance = .5f;
    [SerializeField] private float minimumJumpHeight = 1.5f;

    [Header("Wall Running")] 
    [SerializeField] private float wallRunGravity = 1f;
    [SerializeField] private float wallRunJumpForce = 6f;

    [Header("Camera")] 
    [SerializeField] private Camera cam;
    [SerializeField] private float fov;
    [SerializeField] private float wallRunFov;
    [SerializeField] private float wallRunFovTime;
    [SerializeField] private float camTilt;
    [SerializeField] private float camTiltTime;
    
    public float tilt { get; private set;  }

    private bool wallLeft = false;
    private bool wallRight = false;
    
    public bool isWallRun = false;

    public PlayerMovement playerMove;

    private RaycastHit leftWallHit;
    private RaycastHit rightWallHit;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        wallRunFov = fov + 20;
    }

    bool canWallRun()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minimumJumpHeight);
    }

    void CheckWall()
    {
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallHit, wallDistance);
        wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallHit, wallDistance);
    }

    private void Update()
    {
        CheckWall();

        if (canWallRun())
        {
            if (wallLeft)
            {
                StartWallRun();
            }
            else if (wallRight)
            {
                StartWallRun();
            }
            else
            {
                StopWallRun();
            }
        }
        else
        {
            StopWallRun();
        }
    }

    void StartWallRun()
    {
        isWallRun = true;
        
        rb.useGravity = false;
        playerMove.useGravity = false;

        rb.drag = 2;
        
        rb.AddForce(Vector3.down * wallRunGravity, ForceMode.Force);

        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, wallRunFov, wallRunFovTime * Time.deltaTime);

        if (wallLeft)
        {
            tilt = Mathf.Lerp(tilt, -camTilt, camTiltTime * Time.deltaTime);
        }
        else if (wallRight)
        {
            tilt = Mathf.Lerp(tilt, camTilt, camTiltTime * Time.deltaTime); 
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (wallLeft)
            {
                Vector3 wallRunJumpDirection = transform.up + leftWallHit.normal;
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                rb.AddForce(wallRunJumpDirection * wallRunJumpForce * 100, ForceMode.Force);
            }
            else if (wallRight)
            {
                Vector3 wallRunJumpDirection = transform.up + rightWallHit.normal;
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                rb.AddForce(wallRunJumpDirection * wallRunJumpForce * 100, ForceMode.Force);
            }
        }
    }

    void StopWallRun()
    {
        isWallRun = false;
        
        rb.drag = 0;
        
        rb.useGravity = true;
        playerMove.useGravity = true;
        
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, fov, wallRunFovTime * Time.deltaTime);
        tilt = Mathf.Lerp(tilt, -0, camTiltTime * Time.deltaTime);
    }
}
