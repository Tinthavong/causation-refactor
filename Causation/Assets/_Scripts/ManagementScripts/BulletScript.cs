using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    //rename buleltscript to be more generic and use it for other things as well
    //combine and simplify with contact damage?
    public int damage = 10;

    //For now this simply assumes that every collision is with an enemy and will take away the appropriate amount of damage
    //we may want to eventually change this to accomodate collisions with destructible objects (if added)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            //why is the bullet tagged with enemy instead of projectile
            //the enemy object is null
            Enemy enemy = collision.GetComponent<Enemy>();
            enemy.DamageCalc(damage);
            AudioSource.PlayClipAtPoint(this.GetComponent<AudioSource>().clip, this.transform.position);
            Debug.Log("Enemy has been hit");
        }
        else if (collision.tag == "Player")
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            player.DamageCalc(1); //arbitrarily 1 but let's figure that out later
            AudioSource.PlayClipAtPoint(this.GetComponent<AudioSource>().clip, this.transform.position);
        }
        Destroy(this.gameObject);
    }
}
