using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class MinimapFollow : MonoBehaviour
{

    ////////////////////////////////////////Public Variables////////////////////////////////////////

    [SerializeField] private float height;
    [SerializeField] private float yOffset;
    [SerializeField] private Transform followObject;

    ////////////////////////////////////////Private Variables////////////////////////////////////////

    private bool exists = true;

    void LateUpdate()
    {
        while (exists)
        {
            if (followObject is null)
            {
                Destroy(gameObject);
                exists = false;
                break;
            }
            else
            {
                transform.position = new Vector3(followObject.position.x, height, followObject.position.z);
                transform.rotation =
                    Quaternion.Euler(transform.eulerAngles.x, 
                        followObject.eulerAngles.y + yOffset, transform.eulerAngles.z);
            }
        }
        
    }
}
