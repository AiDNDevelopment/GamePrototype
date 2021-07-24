using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefensePoint : MonoBehaviour
{

    public HealthBarController healthBar;

    void Start(){
       
    }

    void Update(){
        
    }

    void OnCollisionEnter(Collision collision){ // should handle the defense point health and the image thing
        if(collision.gameObject.tag=="Enemy" || collision.gameObject.tag=="Bullet")
        {
            if (healthBar)
            {
                healthBar.onTakeDamage(10);
            }
        }
    }    
}
