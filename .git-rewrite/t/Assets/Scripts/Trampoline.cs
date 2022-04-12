using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
    
    public Rigidbody playerRb;

    public float bounceForce;

    private void OnCollisionEnter(Collision other)
    {
        playerRb.AddForce(other.contacts[0].normal * bounceForce, ForceMode.VelocityChange);
    }
}
