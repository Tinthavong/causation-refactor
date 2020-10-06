using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 1;
    public int currentHealth;

    public GameObject drop;

    private bool facingRight;

    //public GameObject enemy;

    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0)
        {
            Death();
        }

        //Controls where the enemy is looking (currently looks at player at all times)
        Flip();

    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
    }

    void Death()
    {
        //Temporary death, needs animation and drops

        GameObject d = Instantiate(drop) as GameObject;
        d.transform.position = this.transform.position;

        Destroy(this.gameObject);
    }

    //Checks player position and turns to face them
    private void Flip()
    {
        if(player.transform.position.x < this.transform.position.x && !facingRight)
        {
            facingRight = true;
            Vector3 scale = transform.localScale;
            scale.x *= -1;

            transform.localScale = scale;
        }
        else if(player.transform.position.x >= this.transform.position.x && facingRight)
        {
            facingRight = false;
            Vector3 scale = transform.localScale;
            scale.x *= -1;

            transform.localScale = scale;
        }
    }
}
