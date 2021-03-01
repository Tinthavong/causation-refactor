using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointBehavior : MonoBehaviour
{
    LevelManager LM;
    private void Start()
    {
        LM = FindObjectOfType<LevelManager>();
    }

    //Flags the checkpoints as enabled and the color changes to green to offer affordance to the player
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //LM <= 1 LM++;
            if (LM.checkpointIndex <= 0 || LM.checkpointIndex == 1) LM.checkpointIndex++;
            LM.checkpoints[LM.checkpointIndex].GetComponent<SpriteRenderer>().color = Color.green;
            LM.flaggedCheckpoints[LM.checkpointIndex] = true;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
