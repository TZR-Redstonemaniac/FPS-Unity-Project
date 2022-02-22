using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{

    [SerializeField] private float delay = 3f;
    [SerializeField] private float radius = 5f;
    [SerializeField] private float explosionForce = 700f;

    [SerializeField] private GameObject explosionEffect;
    
    private TimeManager timeManager;
    private GameObject timeManagerGameObject;

    private float countdown;
    private bool hasExploded = false;
    
    // Start is called before the first frame update
    void Start()
    {
        countdown = delay;
        timeManagerGameObject = GameObject.FindGameObjectWithTag("TimeManager");
        timeManager = timeManagerGameObject.GetComponent<TimeManager>();
    }

    // Update is called once per frame
    void Update()
    {
        countdown -= Time.deltaTime;

        if (countdown <= 0 && !hasExploded)
        {
            Explode();
            hasExploded = true;
        }
    }

    void Explode()
    {
        //Show explosion
        Instantiate(explosionEffect, transform.position, transform.rotation);

        //Get nearby objects
        Collider[] collidersToDestroy = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider nearbyObject in collidersToDestroy)
        {
            //Damage
            Destructible dest = nearbyObject.GetComponent<Destructible>();
            if(dest != null)
                dest.Destroy();
        }
        
        //Slow down time
        timeManager.DoSlowMotion();
        
        Collider[] collidersToMove = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider nearbyObject in collidersToMove)
        {
            //Add forces
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            
            if(rb != null)
                rb.AddExplosionForce(explosionForce, transform.position, radius);
        }

        //Remove Grenade
        Destroy(gameObject);
    }
}
