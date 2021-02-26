using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineManager : MonoBehaviour
{
    //This manages the timeline and it will also contain the boxcollider trigger that will activate the cutscene

    [Header("Cutscene Components")]
    public GameObject timelineCutscene; //these cutscenes can be anywhere in the level including the beginning, the distinction is just that it's not at the end

    private PlayerStateManager pc;
    public bool hasCutscenePlayed = false; //made public so that player respawning will cause the cutscene to replay

    public float cutsceneDuration;
    
    // Start is called before the first frame update
    void Start()
    {
        pc = FindObjectOfType<PlayerStateManager>();
        //if boolcheck in cutscene mode?
        cutsceneDuration = (float)timelineCutscene.GetComponent<PlayableDirector>().duration;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasCutscenePlayed && collision.CompareTag("Player"))
        {
            //timescale doesn't have to freeze because player controls are restricted anyways
            timelineCutscene.GetComponent<PlayableDirector>().Play();
            StartCoroutine(PlayerRestriction());
            hasCutscenePlayed = true;     
        }
    }

    IEnumerator PlayerRestriction()
    {
        pc.isControlling = false; //prevents player from moving when cutscene is playing (also means they can't shoot)
        pc.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        pc.gameObject.GetComponent<PlayerMovementController>().animator.SetFloat("Speed", 0f);
        yield return new WaitForSeconds(0.1f);

        pc.isControlling = true;
        pc.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
    }
}
