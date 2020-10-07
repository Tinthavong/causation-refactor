using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactDamage : MonoBehaviour
{
    public int damage = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            player.DamageCalc(damage);
            Debug.Log("Player took 1 damage");
            Death();
        }
    }

    void Death()
    {
        Destroy(gameObject);
    }
}
