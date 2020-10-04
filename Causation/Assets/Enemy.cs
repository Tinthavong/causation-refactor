using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 5;
    public int currentHealth;

    public GUIController healthBar;

    public GameObject enemy;

    public ReticlePointer bullet;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetButtonDown("Fire1"))
        //{
        //    TakeDamage(1);
        //}

        if (currentHealth == 0)
        {
            Death();
        }

    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;

        healthBar.SetHealth(currentHealth);
    }

    void Death()
    {
        Destroy(enemy);
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == bullet)
        {
            TakeDamage(1);
        }
    }
}
