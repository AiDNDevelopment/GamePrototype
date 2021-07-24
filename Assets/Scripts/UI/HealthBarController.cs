using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{

    public Image healthBar;
    public float health;
    public float startHealth;
    // Start is called before the first frame update
   public void onTakeDamage(int damage) //whenever the defense point takes damage the left healthbar should decrease
   {
       health = health - damage;
       healthBar.fillAmount = health / startHealth;
   }
}
