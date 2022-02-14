using System;
using System.Collections;
using System.Collections.Generic;
using ShadowedSouls;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MyPlayerMovement : MonoBehaviour
{
    private float playerHeight = 2f;

    [SerializeField] private Transform orientation;

    [Header("Movement")] 
    public float moveSpeed = 6f;
    public float movementMultiplier = 10f;
    [SerializeField] private float airMultiplier = 0.4f;
    [SerializeField] private float dashSpeed = 20f;
    public bool canDash;

    
    [Header("Sprinting")] 
    private bool isSprint;
    [SerializeField] private float walkSpeed = 4f;
    [SerializeField] private float sprintSpeed = 6f;
    [SerializeField] private float acceleration = 10f;
    
    
    [Header("Crouching")]
    private bool isCrouch;
    [SerializeField] private float slideBoost;
    [SerializeField] private bool slid;
    [SerializeField] private float crouchHeight = 0.75f;
    [SerializeField] private GameObject capsule;
    [SerializeField] private float crouchSpeed = 3f;
    [SerializeField] private float dragAcceleration = 10f;
    

    
    [Header("Jumping")] 
    public float jumpForce = 5f;

    
    [Header("Keybindings")] 
    [SerializeField] PlayerInput playerInput;


    [Header("Gravity")] 
    [SerializeField] private float groundDrag = 6f;
    [SerializeField] private float airDrag = 2f;
    [SerializeField] private float dashDrag = 10f;
    [SerializeField] private float slideDrag = 0f;
    [SerializeField] private float gravity;
    public bool useGravity;
    private float velocityY;

    
    [Header("Cooldown")] 
    [SerializeField] private float dashDragTime;
    [SerializeField] private int lavaCooldownTime;
    [SerializeField] private float dashDragTimeLimit;
    [SerializeField] private float slideDragTime;
    [SerializeField] private float slideDragTimeLimit;


    [Header("Ground Detection")] 
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private LayerMask speedMask;
    [SerializeField] private float groundDistance = 0.4f;
    private bool isGrounded;
    public bool isSpeed;
    
    
    [Header("Double Jump")]
    public bool hasDoubleJump = true;
    private int doubleJumpAmount;
    public Text doubleJumpText;
    
    private Vector3 moveDirection;
    private Vector3 slopeMoveDirection;
    private float velocity;

    public Camera mainCam;
    public GrapplingHook grapple;

    private Rigidbody rb;

    private RaycastHit slopeHit;

    [SerializeField] MyWallRun wallRun;
    private Vector3 _rbVelocity;

    private bool loweredHeight;
    public bool fireButtonPressed;

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight / 2 + 0.5f))
        {
            if (slopeHit.normal != Vector3.up)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        return false;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.useGravity = false;
        _rbVelocity = rb.velocity;
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        isSpeed = Physics.CheckSphere(groundCheck.position, groundDistance, speedMask);

        velocity = Vector3.Dot(rb.velocity, rb.transform.forward);
        velocity = Mathf.Abs(velocity);
        velocity = Mathf.Round(velocity * 100.0f) * 0.01f;

        doubleJumpText.text = doubleJumpAmount.ToString();

        if (isGrounded || grapple.isSecured || isSpeed)
        {
            velocityY = -2f;
        }

        if (useGravity)
        {
            velocityY += gravity * 10 * Time.deltaTime;

            rb.AddRelativeForce(0f, velocityY, 0f, ForceMode.Force);
        }
        else
        {
            velocityY = 0f;
        }
        
        if (isGrounded || isSpeed)
        {
            canDash = true;
        }

        if (Time.time < dashDragTime)
        {
            canDash = false;
        }
        
        if (isGrounded && isSprint && !isCrouch)
        {
            moveSpeed = Mathf.Lerp(moveSpeed, sprintSpeed, acceleration * Time.deltaTime);
        }
        else if(!isGrounded || !isSprint)
        {
            moveSpeed = Mathf.Lerp(moveSpeed, walkSpeed, acceleration * Time.deltaTime);
        }
        
        if (isCrouch)
        {
            capsule.gameObject.transform.localScale = new Vector3(capsule.gameObject.transform.localScale.x,
                crouchHeight, capsule.gameObject.transform.localScale.z);
            if (!loweredHeight)
            {
                capsule.gameObject.transform.localPosition = new Vector3(capsule.gameObject.transform.localPosition.x,
                                capsule.gameObject.transform.localPosition.y - (1f - crouchHeight),
                                capsule.gameObject.transform.localPosition.z);
                loweredHeight = true;
            }
            
            if (velocity >= 8f && !slid)
            {
                slid = true;
                slideDragTime = Time.time + slideDragTimeLimit;
            }
            moveSpeed = Mathf.Lerp(moveSpeed, crouchSpeed, acceleration * Time.deltaTime);
        }
        else
        {
            capsule.gameObject.transform.localScale = new Vector3(capsule.gameObject.transform.localScale.x,
                1f, capsule.gameObject.transform.localScale.z);
            slid = false;
            loweredHeight = false;
        }
        
        ControlDrag();

        rb.MoveRotation(orientation.rotation);
        slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);
    }

    public void OnMovement(InputAction.CallbackContext value)
    {
        Vector2 inputMovement = value.ReadValue<Vector2>();

        moveDirection = new Vector3(inputMovement.x, 0, inputMovement.y);
    }

    public void jump(InputAction.CallbackContext context)
    {
        if (isGrounded || isSpeed && context.started)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
        else if (doubleJumpAmount >= 1 && context.started)
        {
            doubleJumpAmount--;
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            velocityY = 0f;
            canDash = true;
        }
    }

    public void Fire(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            fireButtonPressed = true;
        }

        if (context.canceled)
        {
            fireButtonPressed = false;
        }
    }
    

    public void Sprint(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isSprint = !isSprint;
        }
    }
    
    public void Crouch(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isCrouch = true;
        }

        if (context.canceled)
        {
            isCrouch = false;
        }
    }

    void ControlDrag()
    {
        if (isGrounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = airDrag;
        }

        if (Time.time < dashDragTime)
        {
            rb.drag = dashDrag;
        }

        if (Time.time < slideDragTime)
        {
            rb.drag = slideDrag;
            jumpForce = 12.5f;
        } else if (Time.time < slideDragTime + 0.25)
        {
            rb.drag = Mathf.Lerp(slideDrag, groundDrag, dragAcceleration * Time.deltaTime);
        }
        else if(isGrounded)
        {
            rb.drag = groundDrag;
            jumpForce = 20f;
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        if (isGrounded && !OnSlope())
        {
            rb.AddRelativeForce(moveDirection.normalized * moveSpeed * movementMultiplier,
                ForceMode.Acceleration);
        }
        else if (isGrounded && OnSlope())
        {
            rb.AddRelativeForce(slopeMoveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        }
        else if (!isGrounded)
        {
            rb.AddRelativeForce(moveDirection.normalized * moveSpeed * movementMultiplier * airMultiplier,
                ForceMode.Acceleration);
        }

    }

    public void Dash(InputAction.CallbackContext value)
    {
        if (canDash)
        {
            if (moveDirection == Vector3.back)
            {
                rb.AddForce(-orientation.forward * moveSpeed * dashSpeed, ForceMode.VelocityChange);
                dashDragTime = dashDragTimeLimit + Time.time;
                canDash = false;
            } else if (moveDirection == Vector3.left)
            {
                rb.AddForce(-orientation.right * moveSpeed * dashSpeed, ForceMode.VelocityChange);
                dashDragTime = dashDragTimeLimit + Time.time;
                canDash = false;
            } else if (moveDirection == Vector3.right)
            {
                rb.AddForce(orientation.right * moveSpeed * dashSpeed, ForceMode.VelocityChange);
                dashDragTime = dashDragTimeLimit + Time.time;
                canDash = false;
            }
            else
            {
                rb.AddForce(orientation.forward * moveSpeed * dashSpeed, ForceMode.VelocityChange);
                dashDragTime = dashDragTimeLimit + Time.time;
                canDash = false;   
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Double Jump"))
        {
            other.gameObject.SetActive(false);
            doubleJumpAmount++;
        }
    }
}
