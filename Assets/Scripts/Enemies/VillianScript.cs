using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class VillianScript : MonoBehaviour

{public NavMeshAgent agent;
    

    public Transform player;
    public Transform defense;

    public LayerMask whatIsGround, whatIsPlayer, whatIsDefense;

    public float health;

    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;
    public int damage;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange, defenseInSightRange, defesneInAttackRange;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        defense = GameObject.Find("Defense_Point").transform;       
        agent = GetComponent<NavMeshAgent>();
        
    }

    private void Update()
    {
        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        defenseInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsDefense);
        defesneInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsDefense);


        if (!playerInSightRange && !playerInAttackRange || !defenseInSightRange && !defesneInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange || defenseInSightRange && !defesneInAttackRange) ChasePlayer();
        if (playerInAttackRange && playerInSightRange || defesneInAttackRange && defenseInSightRange) AttackPlayer();

        if(health <= 0){
                Debug.Log("Dead Enemy"); // this should work but like projectiles im not sure how to delete just the clones
                Destroy(gameObject); //this turns the enemies into suicide bombers for some reason
            }
    }

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }
    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void ChasePlayer()//This is broken cus i dno how to set it to equally chase both so atm it sorta does both if the player is close enough but it prios the defense point.
    {
        agent.SetDestination(player.position);
        agent.SetDestination(defense.position);

    }

    private void AttackPlayer()
    {
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);

        transform.LookAt(defense);

        if (!alreadyAttacked)
        {
            ///Attack code here
            Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            rb.AddForce(transform.up * 0.5f, ForceMode.Impulse);
            Destroy(rb, 1f); // doesnt quite work yet, freezes them in place but doesnt delete them :/ setting this to projectile deletes the object thats being cloned.
            
            ///End of attack code

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    void OnCollisionEnter(Collision collision){ // should handle the defense point health and the image thing
        if(collision.gameObject.tag=="Bullet")
        {
            health = health - damage;
            return;    
            /* This block is the key to getting enemies to suicide bomb themselves when placed here.
            if(health <= 0){
                Debug.Log("Dead Enemy"); 
                gameObject.SetActive(false); 
            }*/       
        }
    } 

    
    
}
