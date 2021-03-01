using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileProperties : MonoBehaviour
{
    [SerializeField]
    private int damage = 1;

    [SerializeField]
    public float projectileSpeed;

    [Header("Explosive Ammo Values")]
    [SerializeField]
    public float knockBackAmount;
    public bool isExplosive;
  
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Enemy":
                //Need to check if the collision is the final boss to allow for its reflecting phase to take effect
                ICharacterStatesCalculations enemy = collision.GetComponent<ICharacterStatesCalculations>();
                if (isExplosive)
                {
                    enemy.DamageCalculation(damage);
                    Debug.Log("Enemy has been hit");
                    //Knockback();
                }
                else
                {
                    enemy.DamageCalculation(damage);
                    Debug.Log("Enemy has been hit");
                }
                Destroy(gameObject);
                break;

            case "Boss":
                ICharacterStatesCalculations boss = collision.GetComponent<ICharacterStatesCalculations>();
                boss.DamageCalculation(damage);
                Destroy(gameObject);
                break;

            case "Player":
                ICharacterStatesCalculations player = collision.gameObject.GetComponent<ICharacterStatesCalculations>();
                player.DamageCalculation(damage);
                Destroy(gameObject);
                break;

            case "EnvironmentObject":
                Destroy(gameObject);
                break;

            case "PlatformObject":
                Destroy(gameObject);
                break;

            case "Bounds":
                Destroy(gameObject);
                break;

            default: //Or create an Bounds tag
                Destroy(gameObject);
                break;
        }
    }
}
