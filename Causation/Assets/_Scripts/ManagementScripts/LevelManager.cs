using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{  //game over manager for both player wins and losses
    public GameObject checkpoint; //the last visited checkpoint, a flag is set that allows the player to respawn there
    public bool flaggedCheckpoint = false;
    public bool canRetry = false;
    public int checkpointCost;

    public GameObject victoryPoint;
    public GameObject levelLoader;
    public GameObject GameOverPanel;
    public GameObject victoryPanel;
    private GameObject hudRef;

    // Start is called before the first frame update
    void Start()
    {
        hudRef = GameObject.Find("HUDElements");
        GameOverPanel = GameObject.Find("GameOverScreen");
    }

    // Update is called once per frame
    void Update()
    {

    }

    //called in the player controller class to pause gameplay and remove player controls
    //also spawns the unity UI object/panel that shows gameover buttons like, retry, restart, quit, mainmenu etc 
    public void GameOver()
    {
        //Spawn the game over panels or UI game object here
        //The player script disables movement but if you want to pause the gameworld then i imagine it could be done right here
        SetActiveChildren(hudRef.transform, false);
        SetActiveChildren(GameOverPanel.transform, true);
    }

    private void SetActiveChildren(Transform transform, bool value)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(value);
            SetActiveChildren(child, value);
        }
    }

    public void VictoryCheck()
    {
        //Spawn the victory screen here or the transition or whatever you had in mind
        //The player script disables movement but if you want to pause the gameworld then i imagine it could be done right here
        SetActiveChildren(hudRef.transform, false);
        victoryPanel.SetActive(true);
    }

    public void RetryCheckpoint()//Retry from a checkpoint rather than from the beginning
    {
        PlayerController pc = FindObjectOfType<PlayerController>();
        //the hardcoded "1" should be replaced with a variable for the cost

        if (canRetry == true)
        {
            pc.walletValue = (pc.walletValue - checkpointCost); 
            //A "replenish" function for playercontroller might be best for using checkpoints
            Camera mc = FindObjectOfType<Camera>();
            pc.transform.position = checkpoint.transform.position;
            Vector3 camerapoint = new Vector3(pc.transform.position.x, pc.transform.position.y, -10);
            mc.transform.position = camerapoint;
            pc.Replenish();
            SetActiveChildren(hudRef.transform, true);
            SetActiveChildren(GameOverPanel.transform, false);
            //respawn enemies too?
        }
        else
        {
            canRetry = false;//can't afford to retry anymore
                             //Grey this out or tell the player that they can't afford it?
                             //will finish this logic later and change the gameover UI, for now will use the restarygame logic
            MenuController mc = new MenuController();
            mc.RestartGame();
        }
    }

    public void CheckpointCostCheck(PlayerController pc) 
    {
        if (pc.walletValue >= checkpointCost)
        {
            canRetry = true;
        }
        else if (pc.walletValue < checkpointCost)
        {
            canRetry = false;
        }

    }
}
