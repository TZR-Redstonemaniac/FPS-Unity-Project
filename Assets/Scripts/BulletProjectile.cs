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

    private void OnTriggerEnter(Collider other)
    {

        switch (other.tag)
        {
            case "Flesh":
                Instantiate(fleshImpact, transform.position, transform.rotation);
                break;
            case "Sand":
                Instantiate(sandImpact, transform.position, transform.rotation);
                break;
            case "Stone":
                Instantiate(stoneImpact, transform.position, transform.rotation);
                break;
            case "Metal":
                Instantiate(metalImpact, transform.position, transform.rotation);
                break;
            case "Wood":
                Instantiate(woodImpact, transform.position, transform.rotation);
                break;
        }

        Destroy(gameObject);
    }
}
