using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class BulletPhysics : MonoBehaviour
{
    
    [Header("Assignable")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GameObject explosion;
    [SerializeField] private LayerMask whatIsEnemies;
    
    [Header("Stats")]
    [Range(0f, 1f)] [SerializeField] private float bounciness;
    [SerializeField] private bool useGravity;

    [Header("Damage")] 
    [SerializeField] private int damage;
    
    [Header("Explosion")]
    [SerializeField] private int explosionDamage;
    [SerializeField] private float explosionRange;
    [SerializeField] private float explosionForce;
    [SerializeField] private bool doExplode;
    
    
    [Header("Lifetime")]
    [SerializeField] private int maxCollisions;
    [SerializeField] private float maxLifetime;
    [SerializeField] private bool explodeOnImpact;
    
    [Header("Impact")]
    [SerializeField] private GameObject fleshImpact;
    [SerializeField] private GameObject sandImpact;
    [SerializeField] private GameObject stoneImpact;
    [SerializeField] private GameObject metalImpact;
    [SerializeField] private GameObject woodImpact;

    private int collisions;
    private PhysicMaterial physicMaterial;

    private void Start()
    {
        Setup();
    }

    private void Update()
    {
        //When to explode
        if(collisions > maxCollisions && doExplode)
            Explode();
        
        //Count down lifetime
        maxLifetime -= Time.deltaTime;
        if(maxLifetime <= 0 && doExplode)
            Explode();
    }

    private void Explode()
    {
        //Instate explosion
        if (explosion != null)
            Instantiate(explosion, transform.position, Quaternion.identity);
        
        //Check for enemies
        Collider[] enemies = Physics.OverlapSphere(transform.position, explosionRange, whatIsEnemies);
        foreach (var t in enemies)
        {
            //Get component of enemy and call TakeDamage
            t.GetComponent<EnemyAi>().TakeDamage(explosionDamage);
        }
        
        //Check for other objects
        Collider[] otherObjects = Physics.OverlapSphere(transform.position, explosionRange);
        foreach (var t in otherObjects)
        {
            //Check if object can be destroyed and if so, destroy it
            Destructible dest = t.GetComponent<Destructible>();
            if(dest != null)
                dest.Destroy();
            
            //Add explosion force
            if (t.GetComponent<Rigidbody>())
                t.GetComponent<Rigidbody>()
                    .AddExplosionForce(explosionForce, transform.position, explosionRange);
        }
        
        //Add a little delay to avoid bugs
        Invoke(nameof(Delay), 0.01f);
    }

    private void Delay()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision other)
    {
        //Count up collisions
        collisions++;
        
        //Explode if bullet hits an enemy directly and explodeOnImpact is true
        if(other.collider.CompareTag("Enemy") && explodeOnImpact && doExplode)
            Explode();
        
        if(other.collider.CompareTag("Enemy") && !doExplode)
            other.gameObject.GetComponent<EnemyAi>().TakeDamage(damage);
        
        //Create particle effect on collision
        switch (other.collider.tag)
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
    }

    private void Setup()
    {
        //Create new physics material
        physicMaterial = new PhysicMaterial
        {
            bounciness = bounciness,
            frictionCombine = PhysicMaterialCombine.Minimum,
            bounceCombine = PhysicMaterialCombine.Maximum,
            dynamicFriction = 1f,
            staticFriction = 1f
        };

        //Assign material
        GetComponent<MeshCollider>().material = physicMaterial;
        
        //Set Gravity
        rb.useGravity = useGravity;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRange);
    }
}
