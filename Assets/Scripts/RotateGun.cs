using System;
using System.Collections;
using System.Collections.Generic;
using ShadowedSouls;
using UnityEngine;

public class RotateGun : MonoBehaviour
{

    public GrapplingGun grapple;
    public WeaponSway sway;

    private Quaternion desiredRotation;
    private float rotationSpeed = 5f;

    private void Update()
    {
        if (!grapple.grappleConnected)
        {
            desiredRotation = transform.parent.rotation;
            sway.rotateGun = true;
        }
        else
        {
            desiredRotation = Quaternion.LookRotation(grapple.GetGrapplingPoint() - transform.position);
            sway.rotateGun = false;
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, Time.deltaTime * rotationSpeed);
    }
}
