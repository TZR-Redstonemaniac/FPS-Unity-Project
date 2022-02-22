using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeThrower : MonoBehaviour
{
    
    [SerializeField] private float throwForce = 40f;
    [SerializeField] private GameObject grenadePrefab;
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Grenade"))
            ThrowGrenade();
    }

    void ThrowGrenade()
    {
        //Create grenade
        GameObject grenade = Instantiate(grenadePrefab, transform.position + (transform.forward * 0.6f),
            transform.rotation);
        Rigidbody rb = grenade.GetComponent<Rigidbody>();
        
        //Apply force
        rb.AddForce(transform.forward * throwForce, ForceMode.VelocityChange);
    }
}
