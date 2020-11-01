using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [SerializeField]
    private int damage;

    public bool playerBullet;
    public bool isExplosive;

    Enemy enemy;

    //For now this simply assumes that every collision is with an enemy and will take away the appropriate amount of damage
    //we may want to eventually change this to accomodate collisions with destructible objects (if added)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Enemy":
                if (playerBullet)
                {
                    enemy = collision.GetComponent<Enemy>();

                    if (isExplosive)
                    {
                        enemy.DamageCalc(damage);
                        Debug.Log("Enemy has been hit");
                        Knockback();
                    }
                    else
                    {
                        enemy.DamageCalc(damage);
                        Debug.Log("Enemy has been hit");
                    }
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

    private void Knockback()
    {
        if(enemy.facingRight)
        {
            //enemy.GetComponent<Rigidbody2D>().AddForce();
        }
    }
}
