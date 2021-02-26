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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (LM.checkpointIndex <= 0 || LM.checkpointIndex == 1) LM.checkpointIndex++;
            LM.checkpoints[LM.checkpointIndex].GetComponent<SpriteRenderer>().color = Color.green;
            LM.flaggedCheckpoints[LM.checkpointIndex] = true;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
        else
        {
            //Nothing! 
        }
    }
}
