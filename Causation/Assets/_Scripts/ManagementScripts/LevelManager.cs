using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{  //game over manager for both player wins and losses
    public GameObject checkpoint; //the last visited checkpoint, a flag is set that allows the player to respawn there
    public bool flaggedCheckpoint = false;

    //The sloppiest and quickest way to add another checkpoint, works fine because of the small - medium sized levels
    public GameObject checkpoint2;
    public bool flaggedCheckpoint2 = false;

    public bool canRetry = false; //can afford the toll/checkpoint cost
    public int checkpointCost;

    public GameObject victoryPoint;
    public GameObject levelLoader;
    public GameObject GameOverPanel;
    public GameObject victoryPanel;
    private GameObject hudRef;
    private Currency cy;

    //Enemy mangement
    Enemy[] enemiesInLevel; //an array that stores all of the enemies inside of them for the specific scene

   

    void Start()
    {
        enemiesInLevel = FindObjectsOfType<Enemy>();
        cy = FindObjectOfType<Currency>();
        hudRef = GameObject.Find("HUDElements");
        GameOverPanel = GameObject.Find("GameOverScreen");
    }

    //enemies are forced asleep after the player retries from the checkpoint, this seems more like a failsafe than a feature    
    //enemy's position resets to whatever their starting position was
    public void EnemyReplenish()
    {
        foreach (Enemy en in enemiesInLevel)
        {
            if (!en.isBox && en.displayedHealth > 0)
            {
                en.isAwake = false; //enemy behavior reset     
                en.transform.localPosition = en.startingLocation;
                en.displayedHealth = en.startingHealth;
            }
        }
    }

    //called in the player controller class to pause gameplay and remove player controls
    //also spawns the unity UI object/panel that shows gameover buttons like, retry, restart, quit, mainmenu etc 
    public void GameOver()
    {
        //Spawn the game over panels or UI game object here
        //The player script disables movement, this handles UI
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
        //Spawn the victory screen here
        //The player script disables movement
        SetActiveChildren(hudRef.transform, false);
        victoryPanel.SetActive(true);
    }

    public void RetryCheckpoint()//Retry from a checkpoint rather than from the beginning
    {
        PlayerController pc = FindObjectOfType<PlayerController>();
        if (canRetry == true)
        {
            cy.WalletProperty = (cy.WalletProperty - checkpointCost);
            //A "replenish" function for playercontroller might be best for using checkpoints
            Camera mc = FindObjectOfType<Camera>();

            //Extremely sequential approach that doesn't allow dynamic checkpoints, the first if statement proceeds as long as the second checkpoint isn't flagged but once it is the first if statement is ignored
            if(flaggedCheckpoint && !flaggedCheckpoint2)
            {
                pc.transform.position = checkpoint.transform.position;
                Vector3 camerapoint = new Vector3(pc.transform.position.x, pc.transform.position.y, -10);
                mc.transform.position = camerapoint;
            }
            else if (flaggedCheckpoint2)
            {
                pc.transform.position = checkpoint2.transform.position;
                Vector3 camerapoint = new Vector3(pc.transform.position.x, pc.transform.position.y, -10);
                mc.transform.position = camerapoint;
            }

            pc.Replenish();
            EnemyReplenish(); //Resets the enemies behaviors
            SetActiveChildren(hudRef.transform, true);
            SetActiveChildren(GameOverPanel.transform, false);
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

    public void CheckpointCostCheck()
    {
        if (cy.WalletProperty >= checkpointCost)
        {
            Debug.Log(cy.WalletProperty);
            canRetry = true;
        }
        else if (cy.WalletProperty < checkpointCost)
        {
            Debug.Log(cy.WalletProperty);
            canRetry = false;
        }

    }
}
