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
    private GameObject projectileholder;
    private ProjectileGun projectile;
    private RaycastHit pointHit;
    private Vector3 ImpactLocation;

    private void Awake()
    {
        cameraHolder = GameObject.FindWithTag("MainCamera");
        cam = cameraHolder.GetComponent<Camera>(); 
        
        projectileholder = GameObject.FindWithTag("ProjectileGunObject");
        projectile = projectileholder.GetComponent<ProjectileGun>();

        pointHit = projectile.hit;
    }

    private void Update()
    {
        ImpactLocation = pointHit.point;
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
