using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitScript : MonoBehaviour
{
    [SerializeField]
    private int damage = 3;

    PlayerBaseStats player;
    PlayerStateManager psm;


    private Vector2 respawnPOS;
    private Vector2 startingPos; //absolute beginning of the level before checkpoints are triggered

    private void Start()
    {
        player = FindObjectOfType<PlayerBaseStats>();
        psm = FindObjectOfType<PlayerStateManager>();
        startingPos = player.GetComponent<Transform>().position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        LevelManager LM = FindObjectOfType<LevelManager>();

        switch (collision.tag)
        {
            case "Enemy":
                Destroy(collision.gameObject);
                break;
            case "Player":
                //store pit "respawn points" by hand instead of referencing checkpoints?
                //make an array of pitRestore points then assign those positions equal to each of the checkpoints
                player = collision.GetComponent<PlayerBaseStats>();

                //Sent to the start of the level instead of at a checkpoint because the index is negative one by default (no checkpoints registered)
                if (LM.checkpointIndex == -1)
                {
                    player.transform.position = startingPos;
                }

                else
                {
                    if (LM.flaggedCheckpoints[LM.checkpointIndex])
                    {
                        //   respawnPOS = LM.checkpoint.transform.position;
                        respawnPOS = LM.checkpoints[LM.checkpointIndex].transform.position;
                    }               
                }

                player.transform.position = respawnPOS;

                Camera camera = FindObjectOfType<Camera>();
                camera.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10);

                psm.DamageCalculation(damage);
                break;
        }

        Debug.Log(respawnPOS);
    }
}
