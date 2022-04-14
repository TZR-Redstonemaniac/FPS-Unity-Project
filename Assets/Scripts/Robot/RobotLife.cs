using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;
using UnityStandardAssets.Characters.ThirdPerson;

public class RobotLife : MonoBehaviour
{

    ////////////////////////////////////////Public Variables////////////////////////////////////////

    [SerializeField] private Material deathMat;
    [SerializeField] private float health;
    [SerializeField] private float deathSpeed;
    [SerializeField] private float deathDelay;
    [SerializeField] private float deathLunge;
    [SerializeField] private Renderer[] renderers;

    ////////////////////////////////////////Private Variables////////////////////////////////////////

    private Rigidbody[] ragdollRigidbodies;
    private Collider[] ragdollColliders;
    private float dissolveAmount;

    // Start is called before the first frame update
    void Start()
    {
        ragdollRigidbodies = gameObject.GetComponentsInChildren<Rigidbody>();
        ragdollColliders = gameObject.GetComponentsInChildren<Collider>();
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
        gameObject.GetComponent<RobotMovement>().enabled = true;
        dissolveAmount = -1;
        DisableRagdoll();
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
            Die();
        
        dissolveAmount = Mathf.Clamp(dissolveAmount, -1, 1);
        deathMat.SetFloat("_Dissolve", dissolveAmount);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
    }

    void Die()
    {
        gameObject.GetComponent<RobotMovement>().enabled = false;
        gameObject.GetComponent<ThirdPersonCharacter>().enabled = false;
        gameObject.GetComponent<Animator>().enabled = false;
        gameObject.GetComponent<NavMeshAgent>().enabled = false;
        gameObject.GetComponent<RigBuilder>().enabled = false;
        EnableRagdoll();
        Invoke(nameof(EnableMat), deathDelay);
    }

    void EnableMat()
    {
        foreach (Renderer r in renderers)
        {
            r.material = deathMat;
        }
        dissolveAmount += deathSpeed * Time.deltaTime;
        
        if(dissolveAmount >= 1)
            Invoke(nameof(RestoreMat), 2);
    }

    void RestoreMat()
    {
        dissolveAmount = -1;
        Destroy(transform.gameObject);
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

        foreach (Rigidbody rb in ragdollRigidbodies)
        {
            rb.AddRelativeForce(Vector3.up * deathLunge, ForceMode.VelocityChange);
        }
    }
}
