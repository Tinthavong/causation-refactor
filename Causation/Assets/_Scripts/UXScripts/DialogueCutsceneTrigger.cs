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
       
        if (!hasPlayed && collision.CompareTag("Player"))
        {
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
