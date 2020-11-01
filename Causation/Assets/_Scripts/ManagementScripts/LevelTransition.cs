using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelTransition : MonoBehaviour
{
    public Animator fade;
    public Canvas fadeCanvas;
    private int playOnce = 1;
    PlayerController pc;


    private void Start()
    {
        pc = FindObjectOfType<PlayerController>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) && Interactables.transitionFlag && playOnce != 0)
        {
            pc.GetComponent<Animator>().SetFloat("Speed", 0);
            pc.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX;
            //StartCoroutine(Loading());
            playOnce = 0;
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
