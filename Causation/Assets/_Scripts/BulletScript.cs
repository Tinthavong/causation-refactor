using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float speed = 15f;
    public int damage = 10;
    public Rigidbody2D rb;

    private void Start()
    {
        //This is what makes the bullet go forward on the X axis and with the appropriate speed
        //Speed can be adjusted later if it feels too slow now
        rb.velocity = transform.right * speed;
    }

    //For now this simply assumes that every collision is with an enemy and will take away the appropriate amount of damage
    //we may want to eventually change this to accomodate collisions with destructible objects (if added)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();

        if(enemy != null)
        {
            enemy.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}
