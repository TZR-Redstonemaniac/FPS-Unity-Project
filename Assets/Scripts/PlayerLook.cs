using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
        [Header("Refrences")]
        [SerializeField] MyWallRun wallRun;
        
        [SerializeField] private float sensX;
        [SerializeField] private float sensY;

        [SerializeField] Transform cam;
        [SerializeField] private Transform orientation;

        private float mouseX;
        private float mouseY;

        private float multiplier = 0.01f;

        private float xRotation;
        private float yRotation;

        private void Start()
        {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
        }

        private void Update()
        {
                mouseX = Input.GetAxisRaw("Mouse X");
                mouseY = Input.GetAxisRaw("Mouse Y");

                yRotation += mouseX * sensX * multiplier;
                xRotation -= mouseY * sensY * multiplier;

                xRotation = Mathf.Clamp(xRotation, -90, 90);
                
                cam.transform.localRotation = Quaternion.Euler(xRotation, yRotation, wallRun.tilt);
                orientation.transform.rotation = Quaternion.Euler(0, yRotation, 0);
        }

}
