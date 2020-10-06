using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoMan : MonoBehaviour
{
    private int health = 5;
    private int currentHealth;
    public HealthBar healthBar;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = health;
        //healthBar.SetMaxHealth(health);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        healthBar.SetHealth(currentHealth);
    }


}
