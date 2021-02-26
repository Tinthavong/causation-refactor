using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    /*
    [SerializeField]
    private int damage = 1;

    [SerializeField]
    public float knockBackAmount;

    [SerializeField]
    public float bulletSpeed;
    
    public bool isExplosive;
    public bool isPlayerBullet;

    EnemyBase enemy;

    //For now this simply assumes that every collision is with an enemy and will take away the appropriate amount of damage
    //we may want to eventually change this to accomodate collisions with destructible objects (if added)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Enemy":
                //Need to check if the collision is the final boss to allow for its reflecting phase to take effect
                if (isPlayerBullet)
                {
                    enemy = collision.GetComponent<EnemyBase>();

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
                    FindObjectOfType<SFXManager>().PlayAudio("Damage");
                }
                Destroy(gameObject);
                break;
            case "Player":
                PlayerStateManager player = collision.gameObject.GetComponent<PlayerStateManager>();
                player.DamageCalculation(damage);

                FindObjectOfType<SFXManager>().PlayAudio("Damage");
                Destroy(gameObject);
                break;
            case "Object":
                FindObjectOfType<SFXManager>().PlayAudio("Damage");
                Destroy(gameObject);
                break;
            case "FinalBoss":
                FindObjectOfType<SFXManager>().PlayAudio("Damage");
                Destroy(gameObject);
                break;
        }

        
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    //Need this for final boss to have its own collision detection in its reflection phase
    public int GetDamage()
    {
        return damage;
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
    */
}
