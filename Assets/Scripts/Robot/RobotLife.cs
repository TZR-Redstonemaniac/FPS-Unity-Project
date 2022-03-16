using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;
using UnityStandardAssets.Characters.ThirdPerson;

public class RobotLife : MonoBehaviour
{

    ////////////////////////////////////////Public Variables////////////////////////////////////////

    [SerializeField] private float health;

    ////////////////////////////////////////Private Variables////////////////////////////////////////

    private Rigidbody[] ragdollRigidbodies;
    private Collider[] ragdollColliders;

    // Start is called before the first frame update
    void Start()
    {
        ragdollRigidbodies = gameObject.GetComponentsInChildren<Rigidbody>();
        ragdollColliders = gameObject.GetComponentsInChildren<Collider>();
        gameObject.GetComponent<CapsuleCollider>().enabled = true;
        DisableRagdoll();
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
            Die();
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
    }

    void Die()
    {
        EnableRagdoll();
        gameObject.GetComponent<ThirdPersonCharacter>().enabled = false;
        gameObject.GetComponent<Animator>().enabled = false;
        gameObject.GetComponent<NavMeshAgent>().enabled = false;
        gameObject.GetComponent<RigBuilder>().enabled = false;
    }

    void DisableRagdoll()
    {
        foreach (Rigidbody rb in ragdollRigidbodies)
        {
            rb.isKinematic = true;
        }

        foreach (Collider collider in ragdollColliders)
        {
            collider.enabled = true;
        }
    }
    
    void EnableRagdoll()
    {
        foreach (Rigidbody rb in ragdollRigidbodies)
        {
            rb.isKinematic = false;
        }
        
        foreach (Collider collider in ragdollColliders)
        {
            collider.enabled = true;
        }
    }
}
