using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public int damage;
    public bool playerBullet;

    //For now this simply assumes that every collision is with an enemy and will take away the appropriate amount of damage
    //we may want to eventually change this to accomodate collisions with destructible objects (if added)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Enemy":
                if (playerBullet)
                {
                    Enemy enemy = collision.GetComponent<Enemy>();
                    enemy.DamageCalc(damage);
                    Debug.Log("Enemy has been hit");
                }

                Destroy(gameObject);
                break;
            case "Player":
                if (!playerBullet)
                {
                    PlayerController player = collision.gameObject.GetComponent<PlayerController>();
                    player.DamageCalc(damage);
                    Debug.Log("player has been hit");

                }

                Destroy(gameObject);
                break;
            case "Object":
                Destroy(gameObject);
                break;
        }

        FindObjectOfType<SFXManager>().PlayAudio("Damage");
    }
}
