using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 1;
    public int currentHealth;

    public GameObject enemy;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            TakeDamage(1);
        }

        if (currentHealth == 0)
        {
            Death();
        }

    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
    }

    void Death()
    {
        Destroy(enemy);
    }
}
