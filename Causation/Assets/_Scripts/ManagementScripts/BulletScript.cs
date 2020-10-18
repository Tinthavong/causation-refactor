using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public int damage = 10;
    public bool playerBullet;

    //For now this simply assumes that every collision is with an enemy and will take away the appropriate amount of damage
    //we may want to eventually change this to accomodate collisions with destructible objects (if added)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" && playerBullet)
        {
            //why is the bullet tagged with enemy instead of projectile
            //the enemy object is null
            Enemy enemy = collision.GetComponent<Enemy>();
            enemy.DamageCalc(damage);
            AudioSource.PlayClipAtPoint(this.GetComponent<AudioSource>().clip, this.transform.position);
            Debug.Log("Enemy has been hit");
        }

        else if (collision.tag == "Player" && !playerBullet)
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            player.DamageCalc(1); //arbitrarily 1 but let's figure that out later, okay so the damage being set in this script means that the player and enemies should use different prefabs
            AudioSource.PlayClipAtPoint(this.GetComponent<AudioSource>().clip, this.transform.position);
        }
        Destroy(this.gameObject);
    }
}
