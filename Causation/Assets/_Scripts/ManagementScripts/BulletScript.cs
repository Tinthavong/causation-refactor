using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [SerializeField]
    private int damage;

    [SerializeField]
    public float knockBackAmount;

    [SerializeField]
    public float bulletSpeed;
    
    public bool isExplosive;
    public bool isPlayerBullet;

    Enemy enemy;

    //For now this simply assumes that every collision is with an enemy and will take away the appropriate amount of damage
    //we may want to eventually change this to accomodate collisions with destructible objects (if added)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Enemy":
                if (isPlayerBullet)
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
                PlayerController player = collision.gameObject.GetComponent<PlayerController>();
                player.DamageCalc(damage);
                Debug.Log("player has been hit");

                Destroy(gameObject);
                break;
            case "Object":
                Destroy(gameObject);
                break;
        }

        FindObjectOfType<SFXManager>().PlayAudio("Damage");
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void Knockback()
    {
        //Vector2 knockback = new Vector2(knockBackAmount, 1f);
        //enemy = enemy.GetComponent<>();
        if(enemy.facingRight)
        {
            enemy.GetComponent<Rigidbody2D>().velocity = new Vector2(knockBackAmount, 3f);
        }
        else
        {
            enemy.GetComponent<Rigidbody2D>().velocity = new Vector2(-knockBackAmount, 3f);
        }
    }
}
