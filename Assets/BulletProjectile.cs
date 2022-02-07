using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{

    [SerializeField] private GameObject fleshImpact;
    [SerializeField] private GameObject sandImpact;
    [SerializeField] private GameObject stoneImpact;
    [SerializeField] private GameObject metalImpact;
    [SerializeField] private GameObject woodImpact;

    private Camera cam;
    private GameObject cameraHolder;
    private RaycastHit hit;
    private Vector3 ImpactLocation;

    private void Awake()
    {
        cameraHolder = GameObject.FindWithTag("Camera");
        cam = cameraHolder.GetComponent<Camera>();
        
        if (cam != null) Debug.Log("False");
    }

    private void Update()
    {
        Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, Mathf.Infinity);

        ImpactLocation = hit.point;
    }

    private void OnTriggerEnter(Collider other)
    {

        switch (other.tag)
        {
            case "Flesh":
                Instantiate(fleshImpact, ImpactLocation, transform.rotation);
                break;
            case "Sand":
                Instantiate(sandImpact, ImpactLocation, transform.rotation);
                break;
            case "Stone":
                Instantiate(stoneImpact, ImpactLocation, transform.rotation);
                break;
            case "Metal":
                Instantiate(metalImpact, ImpactLocation, transform.rotation);
                break;
            case "Wood":
                Instantiate(woodImpact, ImpactLocation, transform.rotation);
                break;
        }

        Destroy(gameObject);
    }
}
