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

    // Update is called once per frame
    private void Update()
    {
        if(followObject == null)
            Destroy(gameObject);
    }

    void LateUpdate()
    {
        transform.position = new Vector3(followObject.position.x, height, followObject.position.z);
        transform.rotation =
            Quaternion.Euler(transform.eulerAngles.x, followObject.eulerAngles.y + yOffset, transform.eulerAngles.z);
    }
}
