using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{

    [SerializeField] private float delay = 3f;

    [SerializeField] private GameObject explosionEffect;

    private float countdown;
    private bool hasExploded = false;
    
    // Start is called before the first frame update
    void Start()
    {
        countdown = delay;
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
        //Add forces
        //Damage

        //Remove Grenade
        Destroy(gameObject);
    }
}
