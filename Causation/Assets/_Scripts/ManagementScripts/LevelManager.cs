using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{  //game over manager for both player wins and losses
    public GameObject flaggedCheckPoint; //the last visited checkpoint, a flag is set that allows the player to respawn there
    private bool checkpointFlag;

    public GameObject victoryPoint;

    public GameObject levelLoader;


    //called in the player controller class to pause gameplay and remove player controls
    //also spawns the unity UI object/panel that shows gameover buttons like, retry, restart, quit, mainmenu etc 
    public void GameOver()
    {
        //Spawn the game over panels or UI game object here
        //The player script disables movement but if you want to pause the gameworld then i imagine it could be done right here
        //Debug.Log("Game over!");
        SceneManager.LoadScene("Death Screen");
    }


    public void VictoryCheck()
    {
        //Spawn the victory screen here or the transition or whatever you had in mind
        //The player script disables movement but if you want to pause the gameworld then i imagine it could be done right here
        //Debug.Log("You win!");
        SceneManager.LoadScene("Victory Screen");
    }

    void Checkpoint()
    {
        if(checkpointFlag)
        {

        }
    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
