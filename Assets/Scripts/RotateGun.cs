using System;
using System.Collections;
using System.Collections.Generic;
using ShadowedSouls;
using UnityEngine;

public class RotateGun : MonoBehaviour
{

    public GrapplingGun grapple;

    private Quaternion desiredRotation;
    private float rotationSpeed = 5f;

    private void Update()
    {
        if (!grapple.grappleConnected)
        {
            desiredRotation = transform.parent.rotation;
        }
        else
        {
            desiredRotation = Quaternion.LookRotation((grapple.grapplePoint - transform.position), Vector3.up);
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, Time.deltaTime * rotationSpeed);
    }
}
