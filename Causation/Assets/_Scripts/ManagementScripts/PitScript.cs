using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitScript : MonoBehaviour
{
    [SerializeField]
    private int damage;

    PlayerController player;

    private Vector2 respawnPOS;

    private void Start()
    {
        //respawnPOS = gameObject.GetComponent<PlayerController>().transform.position;
        //respawnPOS = player.transform.position;
        //Debug.Log(respawnPOS);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        LevelManager checkpoint = FindObjectOfType<LevelManager>();

        switch(collision.tag)
        {
            case "Enemy":
                Destroy(gameObject);
                break;
            case "Player":
                player = collision.GetComponent<PlayerController>();
                if (checkpoint.flaggedCheckpoint == true)
                {
                    respawnPOS = checkpoint.transform.position;
                    player.transform.position = respawnPOS;
                }
                else if (checkpoint.flaggedCheckpoint == false)
                {
                    player.transform.position = respawnPOS;
                }

                player.DamageCalc(damage);
                break;
        }
    }
}
