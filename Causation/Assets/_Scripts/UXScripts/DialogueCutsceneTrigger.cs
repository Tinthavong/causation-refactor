using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueCutsceneTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    private bool hasPlayed = false;

    [Header("Boss Components")]
    public bool bossDialogueTrigger = false;
    public Bossman1 bossReference; //the boss object that will be activated after dialogue is finished
    public GameObject bossHP; //only necessary for boss triggers

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
            TriggerDialogue();
            bossReference.isAwake = true;
            bossHP.SetActive(true);
            Time.timeScale = 0f;
            hasPlayed = true;
        }
    }

    private void Start()
    {
        //if boolcheck in cutscene mode?
        TriggerDialogue();
        Time.timeScale = 0f;
    }

}
