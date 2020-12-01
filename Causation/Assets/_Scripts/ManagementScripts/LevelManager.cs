using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{  //game over manager for both player wins and losses
  //  public GameObject checkpoint; //the last visited checkpoint, a flag is set that allows the player to respawn there
  //  public bool flaggedCheckpoint = false;

    //The sloppiest and quickest way to add another checkpoint, works fine because of the small - medium sized levels
    //public GameObject checkpoint2;
//    public bool flaggedCheckpoint2 = false;

    public GameObject[] checkpoints; //Checkpoints in the scene, set in the inspector
    public bool[] flaggedCheckpoints;
    public int checkpointIndex; //The current checkpoint that the player can reference, -1 means no checkpoint

    public bool canRetry = false; //can afford the toll/checkpoint cost
    public int checkpointCost;

    public GameObject victoryPoint;
    public GameObject levelLoader;
    public GameObject GameOverPanel;
    public GameObject victoryPanel;
    public GameObject dronePrefab;
    private GameObject hudRef;
    public Currency currency; //Right now this has to be referenced like this because FindObjectOfType is finicky with how the pause screen disables everything- consider putting currency in LevelManager

    //Enemy mangement
    Enemy[] enemiesInLevel; //an array that stores all of the enemies inside of them for the specific scene

    public int maxDronesAliveAtOnce = 3;
    int droneIndex = 0;

    public Button retryButton;

    void Start()
    {
        checkpointCost = 5;
        flaggedCheckpoints = new bool[checkpoints.Length];
        checkpointIndex = -1;

        currency = FindObjectOfType<Currency>();

        retryButton.interactable = false;

        enemiesInLevel = FindObjectsOfType<Enemy>();

        hudRef = GameObject.Find("HUDElements");
        GameOverPanel = GameObject.Find("GameOverScreen");
    }

    //enemies are forced asleep after the player retries from the checkpoint, this seems more like a failsafe than a feature    
    //enemy's position resets to whatever their starting position was
    public void EnemyReplenish()
    {
        //if null or whatever then don't do anything
        foreach (Enemy en in enemiesInLevel)
        {
            if (!en.isBox && en.displayedHealth > 0 && en != null) //should this ignore bosses too?
            {
                en.isAwake = false; //enemy behavior reset     
                en.transform.localPosition = en.startingLocation;
                en.displayedHealth = en.startingHealth;
            }
        }
    }

    //WIP drone spawning method
    public void DroneSpawner()
    {
        foreach (Enemy dr in enemiesInLevel)
        {
            if (dr.GetComponent<Drone>() && dr.displayedHealth > 0 && dr != null)
            {
                droneIndex += 1;
            }
        }
        Debug.Log(droneIndex);

        //right now this spits out a drone one by one ensuring that there are always 3 drones - this number might be too much?
        if (droneIndex <= (maxDronesAliveAtOnce + 1))
        {
            //sound that plays when they spawn
            GameObject drone = Instantiate(dronePrefab) as GameObject;
            //            drone.transform.position = (Camera.main.transform.position + );

            droneIndex -= 1;
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
            currency.WalletProperty = (currency.WalletProperty - checkpointCost);
            //A "replenish" function for playercontroller might be best for using checkpoints
            Camera mc = FindObjectOfType<Camera>();

            /*
            //Extremely sequential approach that doesn't allow dynamic checkpoints, the first if statement proceeds as long as the second checkpoint isn't flagged but once it is the first if statement is ignored
            if (flaggedCheckpoint && !flaggedCheckpoint2)
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
            }*/


            //using arrays for checkpoints
            if (flaggedCheckpoints[checkpointIndex])
            {
                pc.transform.position = checkpoints[checkpointIndex].transform.position;
                Vector3 camerapoint = new Vector3(pc.transform.position.x, pc.transform.position.y, -10);
                mc.transform.position = camerapoint;
            }

            pc.Replenish();
            EnemyReplenish(); //Resets the enemies behaviors
            SetActiveChildren(hudRef.transform, true);
            SetActiveChildren(GameOverPanel.transform, false);
        }
    }

    public void CheckpointCostCheck()
    {
        if (currency.WalletProperty >= checkpointCost)
        {
            canRetry = true;

            if (checkpointIndex >= 0)
            {
                if (flaggedCheckpoints[checkpointIndex]) retryButton.interactable = true;
            }
        }
        else if (currency.WalletProperty < checkpointCost)
        {
            retryButton.interactable = false;
            canRetry = false;
        }
    }
}
