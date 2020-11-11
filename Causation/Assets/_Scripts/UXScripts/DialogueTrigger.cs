using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    private bool hasPlayed = false;
    public bool bossDialogueTrigger = false;
    public Bossman1 bossReference; //the boss object that will be activated after dialogue is finished

    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueController>().StartDialogue(dialogue);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasPlayed && collision.CompareTag("Player") && !bossDialogueTrigger)
        {
            TriggerDialogue();
            Time.timeScale = 0f;
            hasPlayed = true;
        }

        if (!hasPlayed && collision.CompareTag("Player") && bossDialogueTrigger)
        {
            Debug.Log("Boss");
            TriggerDialogue();
            bossReference.isAwake = true;
            Time.timeScale = 0f;
            hasPlayed = true;
        }
    }

}
