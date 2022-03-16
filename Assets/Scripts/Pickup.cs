using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    
    ////////////////////////////////////////Public Variables////////////////////////////////////////

    [SerializeField] private float pickUpRange = 10f;
    [SerializeField] private float moveForce = 250f;
    [SerializeField] private float throwForce = 10f;
    [SerializeField] private Transform holdParent;

    ////////////////////////////////////////Private Variables////////////////////////////////////////

    private GameObject heldObject;
    private RaycastHit hit;
    private Quaternion desiredRotation;
    private const float rotationSpeed = .1f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Interact"))
        {
            if (heldObject == null)
            {
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit,
                    pickUpRange))
                    PickUpObject(hit.transform.gameObject);
            }
            else
            {
                DropObject(false );
            }
        }
        
        if(Input.GetButtonDown("Fire1"))
            if(heldObject != null)
                DropObject(true);

        if (heldObject != null)
            MoveObject();
        
        if(heldObject != null)
            heldObject.transform.rotation =
                Quaternion.Lerp(transform.rotation, desiredRotation, Time.deltaTime * rotationSpeed);
    }

    void MoveObject()
    {
        if (Vector3.Distance(heldObject.transform.position, holdParent.position) > .1f)
        {
            Vector3 moveDirection = holdParent.position - heldObject.transform.position;
            heldObject.GetComponent<Rigidbody>().AddForce(moveDirection * moveForce);
        }
    }

    void PickUpObject(GameObject pickObject)
    {
        if (pickObject.GetComponent<Rigidbody>())
        {
            Rigidbody objectRigidbody = pickObject.GetComponent<Rigidbody>();
            objectRigidbody.drag = 10f;
            objectRigidbody.useGravity = false;

            objectRigidbody.transform.parent = holdParent;
            
            desiredRotation =
                Quaternion.LookRotation(holdParent.position - transform.position, Vector3.up);

            heldObject = pickObject;
            heldObject.gameObject.layer = 13;
        }
    }

    void DropObject(bool throwObject)
    {
        Rigidbody heldRigidbody = heldObject.GetComponent<Rigidbody>();
        heldRigidbody.drag = 1f;
        heldRigidbody.useGravity = true;

        heldObject.gameObject.layer = 0;

        heldObject.transform.parent = null;
        heldObject = null; 
        
        if(throwObject) 
            heldRigidbody.AddForce(holdParent.forward * throwForce * 100, ForceMode.Force);
    }
}
