﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{  //game over manager for both player wins and losses
    public GameObject checkpoint; //the last visited checkpoint, a flag is set that allows the player to respawn there
    public bool flaggedCheckpoint;

    public GameObject victoryPoint;
    public GameObject levelLoader;
    public GameObject GameOverPanel;


    //called in the player controller class to pause gameplay and remove player controls
    //also spawns the unity UI object/panel that shows gameover buttons like, retry, restart, quit, mainmenu etc 
    public void GameOver()
    {
        //Spawn the game over panels or UI game object here
        //The player script disables movement but if you want to pause the gameworld then i imagine it could be done right here
        //Debug.Log("Game over!");
        //SceneManager.LoadScene("Death Screen");
        GameOverPanel.SetActive(true);
    }


    public void VictoryCheck()
    {
        //Spawn the victory screen here or the transition or whatever you had in mind
        //The player script disables movement but if you want to pause the gameworld then i imagine it could be done right here
        //Debug.Log("You win!");
        SceneManager.LoadScene("Victory Screen");
    }

    public void RetryCheckpoint()//Retry from a checkpoint rather than from the beginning
    {
        //A "replenish" function for playercontroller might be best for using checkpoints
        Camera mc = FindObjectOfType<Camera>();
        PlayerController pc = FindObjectOfType<PlayerController>();
        pc.transform.position = checkpoint.transform.position;
        Vector3 camerapoint = new Vector3(pc.transform.position.x, pc.transform.position.y, -10);
        mc.transform.position = camerapoint;
        pc.Replenish();
        GameOverPanel.SetActive(false);
        //respawn enemies too?
    }

    // Start is called before the first frame update
    void Start()
    {
        GameOverPanel = GameObject.Find("GameOverPanel");
    }

    // Update is called once per frame
    void Update()
    {

    }
}