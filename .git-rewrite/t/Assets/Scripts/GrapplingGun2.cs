using UnityEngine;

public class GrapplingGun2 : MonoBehaviour
{
    
    private bool tethered = false;
    private Rigidbody rb;
    private float tetherLength;
    private Vector3 tetherPoint;

    void Start() {
        rb = GetComponent<Rigidbody>();
    }

    void Update() {
        if (Input.GetKey(KeyCode.Mouse0)) {
            if (!tethered) {
                BeginGrapple();
            }
        } else if (!Input.GetKey(KeyCode.Mouse0)) {
            EndGrapple();
        }
    }

    void FixedUpdate() {
        if (tethered) ApplyGrapplePhysics();
    }

    void BeginGrapple() {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, Mathf.Infinity)) {
            tethered = true;
            tetherPoint = hit.point;
            tetherLength = Vector3.Distance(tetherPoint, transform.position);

            rb.isKinematic = false;
        }
    }

    void EndGrapple() {
        tethered = false;
        rb.isKinematic = true;
    }

    void ApplyGrapplePhysics() {
        Vector3 directionToGrapple = Vector3.Normalize(tetherPoint - transform.position);
        float currentDistanceToGrapple = Vector3.Distance(tetherPoint, transform.position);

        float speedTowardsGrapplePoint = Mathf.Round(Vector3.Dot(rb.velocity, directionToGrapple) * 100) / 100;

        if (speedTowardsGrapplePoint < 0) {
            if (currentDistanceToGrapple > tetherLength) {
                rb.velocity -= speedTowardsGrapplePoint * directionToGrapple;
                rb.position = tetherPoint - directionToGrapple * tetherLength;
            }
        }
    }
    
}