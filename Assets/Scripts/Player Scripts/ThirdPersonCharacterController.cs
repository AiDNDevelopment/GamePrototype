using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCharacterController : MonoBehaviour
{
    //Super basic movement script, its literally wasd with no jumping
    public float speed; 
    public GameObject projectile;
    public GameObject Enemy;
    public float timeBetweenAttacks;
    bool hasAttacked;

    void Update(){
        playerMovement();
        playerAttack();
    }

    void playerMovement(){//Simple af player movement
        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");
        Vector3 playerMovement = new Vector3(hor, 0f, ver) * speed * Time.deltaTime;
        transform.Translate(playerMovement, Space.Self);
    }


    void playerAttack(){
        if(Input.GetMouseButton(0) && !hasAttacked){

                      
            Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            rb.AddForce(transform.up * 8f, ForceMode.Impulse);

            hasAttacked = true;
            Debug.Log("You shot");
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }       
    }

     private void ResetAttack()
    {
        hasAttacked = false;
    }
}
