using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public int damage = 10;

    //For now this simply assumes that every collision is with an enemy and will take away the appropriate amount of damage
    //we may want to eventually change this to accomodate collisions with destructible objects (if added)
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if(collision.tag == "Enemy")
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            enemy.TakeDamage(damage);
            AudioSource.PlayClipAtPoint(this.GetComponent<AudioSource>().clip, this.transform.position);
            Debug.Log("Enemy has been hit");
        }

        if (collision.tag == "Player")
        {
            DemoMan player = collision.gameObject.GetComponent<DemoMan>();
            player.TakeDamage(1);
            AudioSource.PlayClipAtPoint(this.GetComponent<AudioSource>().clip, this.transform.position);
        }

        Destroy(gameObject);
    }
}
