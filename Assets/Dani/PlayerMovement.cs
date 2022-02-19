// Some stupid rigidbody based movement by Dani

using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    //Assignable
    public Transform playerCam;
    public Transform orientation;
    public Collider playerCollider;
    
    //Other
    private Rigidbody rb;
    public PersonalWallRun WallRun;

    //Rotation and look
    private float xRotation;
    private float sensitivity = 50f;
    public float sensMultiplier = 1f;
    public Transform head;
    
    //Movement
    public float moveSpeed = 4500;
    public float maxSpeed = 20;
    public LayerMask whatIsGround;
    public bool move;
    
    public float counterMovement = 0.175f;
    private float threshold = 0.01f;
    private float moveMult = 0.5f;
    public float maxSlopeAngle = 35f;
    public bool useGravity;

    //Crouch & Slide
    private readonly Vector3 crouchScale = new Vector3(1, 0.5f, 1);
    private Vector3 playerScale;
    public float slideForce = 400;
    public float slideCounterMovement = 0.2f;

    //Jumping
    private bool readyToJump = true;
    private bool hasDoubleJump;
    private float jumpCooldown = 0f;
    public float jumpForce = 550f;
    private float distToGround;

    //Sprinting
    private bool readyToSprint = true;
    
    //Input
    float x, y;
    bool jumping, sprinting, crouching;
    
    //Sliding
    private readonly Vector3 normalVector = Vector3.up;
    
    public bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f, whatIsGround);
    }

    void Awake() {
        rb = GetComponent<Rigidbody>();
    }
    
    void Start() {
        playerScale =  transform.localScale;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        distToGround = 1.6f;
    }

    
    private void FixedUpdate() {
        Movement();
    }

    private void Update() {
        MyInput();
        Look();
        
        if (IsGrounded()) hasDoubleJump = true;
        if (WallRun.isWallRunning) hasDoubleJump = true;
        
        if (IsGrounded() && Input.GetButtonDown("Jump"))
        {
            //Add jump forces
            rb.AddForce(Vector2.up * jumpForce * 1.5f);
            rb.AddForce(normalVector * jumpForce * 0.5f);
            
            //If jumping while falling, reset y velocity.
            Vector3 vel = rb.velocity;
            if (rb.velocity.y < 0.5f)
                rb.velocity = new Vector3(vel.x, 0, vel.z);
            else if (rb.velocity.y > 0) 
                rb.velocity = new Vector3(vel.x, vel.y / 2, vel.z); 
        } 
        else if (!IsGrounded() && hasDoubleJump && Input.GetButtonDown("Jump") && !WallRun.isWallRunning)
        {
            //Add jump forces
            rb.AddForce(Vector2.up * jumpForce * 1.5f);
            rb.AddForce(normalVector * jumpForce * 0.5f);
            
            //If jumping while falling, reset y velocity.
            Vector3 vel = rb.velocity;
            if (rb.velocity.y < 0.5f)
                rb.velocity = new Vector3(vel.x, 0, vel.z);
            else if (rb.velocity.y > 0) 
                rb.velocity = new Vector3(vel.x, vel.y / 2, vel.z);

            hasDoubleJump = false;
        }
    }

    /// <summary>
    /// Find user input. Should put this in its own class but im lazy
    /// </summary>
    private void MyInput() {
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");
        jumping = Input.GetButtonDown("Jump");
        crouching = Input.GetButton("Crouch");
        sprinting = Input.GetButton("Sprint");
        
        //Sprinting
        if (Input.GetButtonDown("Sprint"))
            Sprint();
        if (Input.GetButtonUp("Sprint"))
            StopSprint();
      
        //Crouching
        if (Input.GetButtonDown("Crouch"))
            StartCrouch();
        if (Input.GetButtonUp("Crouch"))
            StopCrouch();
    }

    private void StartCrouch() {
        transform.localScale = crouchScale;
        transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
        if (rb.velocity.magnitude > 0.5f) {
            if (IsGrounded()) {
                rb.AddForce(orientation.transform.forward * slideForce);
            }
        }
    }

    private void StopCrouch() {
        transform.localScale = playerScale;
        transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
    }

    private void Movement() {
        //Extra gravity
        rb.AddForce(Vector3.down * Time.deltaTime * 10);

        //Find actual velocity relative to where player is looking
        Vector2 mag = FindVelRelativeToLook();
        float xMag = mag.x, yMag = mag.y;

        //Counteract sliding and sloppy movement
        CounterMovement(x, y, mag);

        //Set max speed
        float speed = this.maxSpeed;
        
        //If sliding down a ramp, add force down so player stays grounded and also builds speed
        if (crouching && IsGrounded() && readyToJump) {
            rb.AddForce(Vector3.down * Time.deltaTime * 3000);
            return;
        }
        
        //If speed is larger than maximum speed, cancel out the input so you don't go over max speed
        if (x > 0 && xMag > speed) x = 0;
        if (x < 0 && xMag < -speed) x = 0;
        if (y > 0 && yMag > speed) y = 0;
        if (y < 0 && yMag < -speed) y = 0;

        //Some multipliers
        float multiplier = 1f, multiplierV = 1f;
        
        // Movement in air
        if (!IsGrounded()) {
            multiplier = 0.5f;
            multiplierV = 0.5f;
        }
        
        // Movement while sliding
        if (IsGrounded() && crouching) multiplierV = 0.2f;

        //Apply forces to move player
        if (move)
        {
            moveMult = 0.5f;
        }
        else
        {
            moveMult = 1f;
        }

        rb.AddForce(
            orientation.transform.forward * y * moveSpeed * Time.deltaTime * multiplier * multiplierV * moveMult);
        rb.AddForce(orientation.transform.right * x * moveSpeed * Time.deltaTime * multiplier * moveMult);
    }

    private void Sprint()
    {
        if (IsGrounded() && readyToSprint)
        {
            readyToSprint = false;
            //Apply sprint to player
            maxSpeed = 15;
        }
    }
    private void StopSprint()
    {
        maxSpeed = 10;
        readyToSprint = true;
    }
    
    private float desiredX;
    private void Look() {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.fixedDeltaTime * sensMultiplier;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.fixedDeltaTime * sensMultiplier;

        //Find current look rotation
        Vector3 rot = playerCam.transform.localRotation.eulerAngles;
        desiredX = rot.y + mouseX;
        
        //Rotate, and also make sure we dont over- or under-rotate.
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //Perform the rotations
        if (WallRun.isWallRunning)
        {
            if (WallRun.wallRight)
                WallRun.tilt = Mathf.Lerp(WallRun.tilt, WallRun.camTilt, WallRun.camTiltTime * Time.deltaTime);
            
            if (WallRun.wallLeft)
                WallRun.tilt = Mathf.Lerp(WallRun.tilt, -WallRun.camTilt, WallRun.camTiltTime * Time.deltaTime);
        }
        else
        {
            WallRun.tilt = Mathf.Lerp(WallRun.tilt, 0, WallRun.camTiltTime * Time.deltaTime);
        }
            
        playerCam.transform.localRotation = Quaternion.Euler(xRotation, desiredX, WallRun.tilt);
        
        orientation.transform.localRotation = Quaternion.Euler(0, desiredX, 0);
    }

    private void CounterMovement(float CounterX, float CounterY, Vector2 mag) {
        if (!IsGrounded() || jumping) return;

        //Slow down sliding
        if (crouching) {
            rb.AddForce(moveSpeed * Time.deltaTime * -rb.velocity.normalized * slideCounterMovement);
            return;
        }

        //Counter movement
        if (Math.Abs(mag.x) > threshold && Math.Abs(CounterX) < 0.05f || (mag.x < -threshold && CounterX > 0) || (mag.x > threshold && CounterX < 0)) {
            rb.AddForce(moveSpeed * orientation.transform.right * Time.deltaTime * -mag.x * counterMovement);
        }
        if (Math.Abs(mag.y) > threshold && Math.Abs(CounterY) < 0.05f || (mag.y < -threshold && CounterY > 0) || (mag.y > threshold && CounterY < 0)) {
            rb.AddForce(moveSpeed * orientation.transform.forward * Time.deltaTime * -mag.y * counterMovement);
        }
        
        //Limit diagonal running. This will also cause a full stop if sliding fast and un-crouching, so not optimal.
        if (Mathf.Sqrt((Mathf.Pow(rb.velocity.x, 2) + Mathf.Pow(rb.velocity.z, 2))) > maxSpeed) {
            float fallspeed = rb.velocity.y;
            Vector3 n = rb.velocity.normalized * maxSpeed;
            rb.velocity = new Vector3(n.x, fallspeed, n.z);
        }
    }

    /// <summary>
    /// Find the velocity relative to where the player is looking
    /// Useful for vectors calculations regarding movement and limiting movement
    /// </summary>
    /// <returns></returns>
    private Vector2 FindVelRelativeToLook() {
        float lookAngle = orientation.transform.eulerAngles.y;
        float moveAngle = Mathf.Atan2(rb.velocity.x, rb.velocity.z) * Mathf.Rad2Deg;

        float u = Mathf.DeltaAngle(lookAngle, moveAngle);
        float v = 90 - u;

        float magnitue = rb.velocity.magnitude;
        float yMag = magnitue * Mathf.Cos(u * Mathf.Deg2Rad);
        float xMag = magnitue * Mathf.Cos(v * Mathf.Deg2Rad);
        
        return new Vector2(xMag, yMag);
    }
}
