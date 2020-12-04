using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueCutsceneTrigger : MonoBehaviour
{
    [Header("Dialogue Components")]
    public Dialogue dialogue;
    private bool hasPlayed = false;

    [Header("Boss Components")]
    public bool bossDialogueTrigger = false;
    public Enemy bossReference; //the boss object that will be activated after dialogue is finished
    public GameObject bossHP; //only necessary for boss triggers

    private PlayerController pc;

    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueController>().StartDialogue(dialogue);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //trigger cutscene here instead?
        /*
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
        */
        if (!hasPlayed && collision.CompareTag("Player"))
        {
            //pc.canMove = false; //prevents player from moving when cutscene is playing (also means they can't shoot)
            //timescale doesn't have to freeze because player controls are restricted anyways
            TriggerDialogue();
            hasPlayed = true;
        }
    }

    private void Start()
    {
        TriggerDialogue();
        Time.timeScale = 0f;
    }

    // Update is called once per frame
    void Update()
    {
     
    }
}
