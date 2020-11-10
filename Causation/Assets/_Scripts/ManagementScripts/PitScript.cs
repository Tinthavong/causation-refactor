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

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        LevelManager checkpoint = FindObjectOfType<LevelManager>();

        switch(collision.tag)
        {
            case "Enemy":
                Destroy(collision.gameObject);
                break;
            case "Player":
                player = collision.GetComponent<PlayerController>();

                if (checkpoint.flaggedCheckpoint == true)
                {
                    respawnPOS = checkpoint.checkpoint.transform.position;
                }

                player.transform.position = respawnPOS;

                Camera camera = FindObjectOfType<Camera>();
                camera.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10);

                player.DamageCalc(damage);
                break;
        }

        Debug.Log(respawnPOS);
    }
}
