using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //TODO:Add the animations code for enemy death (not first playable)

    public int maxHealth;
    public int currentHealth;

    //public GUIController healthBar;
    //public GameObject enemy;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        //healthBar.SetMaxHealth(maxHealth);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if(currentHealth <= 0)
        {
            Death();
        }

        //healthBar.SetHealth(currentHealth);
    }

    void Death()
    {
        Destroy(gameObject);
    }
}
