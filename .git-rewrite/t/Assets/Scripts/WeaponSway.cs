using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    
    [Header("Sway Settings")]
    [SerializeField] private float smooth;
    [SerializeField] private float swayMultiplier;

    [Header("Extra")] 
    public bool rotateGun;

    private void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * swayMultiplier;
        float mouseY = Input.GetAxisRaw("Mouse Y") * swayMultiplier;
        
        Quaternion rotationX = Quaternion.AngleAxis(-mouseY, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);

        Quaternion targetRoatation = rotationX * rotationY;

        if (rotateGun)
        {
            transform.localRotation =
                Quaternion.Slerp(transform.localRotation, targetRoatation, smooth * Time.deltaTime);
        }
    }
}
