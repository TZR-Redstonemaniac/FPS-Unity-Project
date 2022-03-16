using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemyAi : MonoBehaviour
{

    ///////////////////////////////////////Editable variables///////////////////////////////////////
    
    [Header("Assignable")]
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform player;
    [SerializeField] private LayerMask whatIsGround, whatIsPlayer;
    
    [Header("Patrolling")]
    [SerializeField] private Vector3 walkPoint;
    [SerializeField] private float walkPointRange;

    [Header("Attacking")] 
    [SerializeField] private float timeBetweenAttacks;
    [SerializeField] private GameObject projectile;
    
    [Header("States")]
    [SerializeField] private float sightRange;
    [SerializeField] private float attackRange;
    [SerializeField] private bool playerInSightRange;
    [SerializeField] private bool playerInAttackRange;

    [Header("Health")] 
    [SerializeField] private float health;

    ///////////////////////////////////////Private variables///////////////////////////////////////
    
    //Patrolling
    private bool walkPointSet;
    
    //Attacking
    private bool alreadyAttacked;
    
    ///////////////////////////////////////Code///////////////////////////////////////

    private void Update()
    {
        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        
        //If the player is not in the attack or sight range, patrol
        if(!playerInSightRange && !playerInAttackRange)
            Patrolling();
        
        //If the player is in sight range but not attack range, chase
        if(playerInSightRange && !playerInAttackRange)
            ChasePlayer();
        
        //If the player is in sight range and in attack range, attack
        if(playerInSightRange && playerInAttackRange)
            AttackPlayer();
        
    }

    void Patrolling()
    {
        //If there is no walk point, set one
        if (!walkPointSet)
            SearchWalkPoint();
        
        //If there is a walk point, go there
        if (walkPointSet)
            agent.SetDestination(walkPoint);
        
        //Finding the distance to the walk point
        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        
        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }

    void SearchWalkPoint()
    {
        //Calculate a random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3
            (transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    void AttackPlayer()
    {
        //Make sure the enemy doe not move
        agent.SetDestination(transform.position);
        
        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            //Attack code
            Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            rb.AddForce(transform.up * 8f, ForceMode.Impulse);

            alreadyAttacked = true;
            
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        
        if(health <= 0)
            Invoke(nameof(DestroyEnemy), .5f);
    }

    void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
