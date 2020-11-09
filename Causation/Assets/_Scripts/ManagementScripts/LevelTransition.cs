using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelTransition : MonoBehaviour
{
    public Animator fade;
    public Canvas fadeCanvas;
    private int playSize = 0;
    private int i;

    private GameObject[] screenTransitions;

    PlayerController pc;

    private void Start()
    {
        pc = FindObjectOfType<PlayerController>();

        //Grab the # of transitions in the level and set the number to i
        screenTransitions = GameObject.FindGameObjectsWithTag("Transition");
        i = screenTransitions.Length;
    }
    private void Update()
    {
        //So long as playSize != i the animation will play
        //This is a dirty fix and we might want to think about making it cleaner later
        if (Input.GetKeyDown(KeyCode.W) && Interactables.transitionFlag && playSize != i)
        {
            pc.GetComponent<Animator>().SetFloat("Speed", 0);
            pc.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX;
            //StartCoroutine(Loading());
            playSize++;
            StartCoroutine(LoadNextScene());
        }
    }

    //public void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Player"))
    //    {
    //        StartCoroutine(LoadNextScene());
    //    }
    //}

    IEnumerator LoadNextScene()
    {
        fadeCanvas.gameObject.SetActive(true);
        //fadeCanvas.enabled = true;
        //Time.timeScale = 0f;
        fade.SetBool("Fade", false);
        yield return new WaitForSeconds(1f);

        pc.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        fade.SetBool("Fade", true);
        yield return new WaitForSeconds(3);

        //fadeCanvas.enabled = false;
        fadeCanvas.gameObject.SetActive(false);

        /*
        fade.SetBool("Fade", false);

        yield return new WaitForSeconds(3);

        fade.SetBool("Fade", true);

        yield return new WaitForSeconds(1);

        fadeCanvas.enabled = false;
        fadeCanvas.gameObject.SetActive(false);
        //Time.timeScale = 1f;
        */

        //SceneManager.LoadScene(levelIndex);
    }

    IEnumerator Loading()
    {
        yield return new WaitForSeconds(1);
    }
}
