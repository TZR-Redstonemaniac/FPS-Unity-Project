using UnityEngine;

public class WallRunningScript : MonoBehaviour
{
    public Transform orientation;
    public float wallDistance = 0.5f;
    public float minimumJumpHeight = 1.5f;
    public float wallRunUpFactor = 10f;
    public float wallRunJumpForce;
    public float camTilt;
    public float camTiltTime;
    public float wallRunSpeed;
    public float Tilt { get; private set; }
    public LayerMask whatIsWall;

    private bool _wallLeft, _wallRight;
    private Rigidbody _rb;
    private RaycastHit _leftWallHit, _rightWallHit;

    private void Awake()
    {
        // grab the rigidbody
        _rb = GetComponent<Rigidbody>();
    }

    bool CanWallRun()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minimumJumpHeight);
    }

    void CheckWall()
    {
        // check if there is a wall
        _wallLeft = Physics.Raycast(transform.position, -orientation.right, out _leftWallHit, wallDistance, whatIsWall);
        _wallRight = Physics.Raycast(transform.position, orientation.right, out _rightWallHit, wallDistance,
            whatIsWall);
    }

    private void Update()
    {
        CheckWall();

        if (CanWallRun())
        {
            if (_wallLeft)
            {
                StartWallRun();
            }
            else if (_wallRight)
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
        // add forwards force
        _rb.AddForce(orientation.forward * wallRunSpeed, ForceMode.Acceleration);

        // camera tilt
        if (_wallLeft)
            Tilt = Mathf.Lerp(Tilt, -camTilt, camTiltTime * Time.deltaTime);
        else if (_wallRight)
            Tilt = Mathf.Lerp(Tilt, camTilt, camTiltTime * Time.deltaTime);

        // wall jumping
        if (Input.GetKey(KeyCode.Space))
        {
            // some super complicated stuff no one will understand
            if (_wallLeft)
            {
                Vector3 wallRunJumpDirection = transform.up * wallRunUpFactor + _leftWallHit.normal;
                _rb.velocity = new Vector3(_rb.velocity.x, 0, _rb.velocity.z);
                _rb.AddForce(wallRunJumpDirection * wallRunJumpForce * 70, ForceMode.Force);
            }
            else if (_wallRight)
            {
                Vector3 wallRunJumpDirection = transform.up * wallRunUpFactor + _rightWallHit.normal;
                _rb.velocity = new Vector3(_rb.velocity.x, 0, _rb.velocity.z);
                _rb.AddForce(wallRunJumpDirection * wallRunJumpForce * 70, ForceMode.Force);
            }
        }
    }

    void StopWallRun()
    {
        // reset camera tilt
        Tilt = Mathf.Lerp(Tilt, 0, camTiltTime * Time.deltaTime);
    }
}