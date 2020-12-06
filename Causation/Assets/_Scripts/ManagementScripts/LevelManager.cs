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
    public GameObject victoryCutscene;
    public GameObject timelineManager;

    public GameObject dronePrefab;
    private GameObject hudRef;
    public Currency currency; //Right now this has to be referenced like this because FindObjectOfType is finicky with how the pause screen disables everything- consider putting currency in LevelManager
    private PlayerController pc;

    //Enemy mangement
    Enemy[] enemiesInLevel; //an array that stores all of the enemies inside of them for the specific scene

    public int maxDronesAliveAtOnce = 3;
    int droneIndex = 0;

    public Button retryButton;

    void Start()
    {
        pc = FindObjectOfType<PlayerController>();

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
            if (!en.isBox && en.displayedHealth > 0 && en != null && !en.isRestrictedFromRespawning) //should this ignore bosses too?
            {
                en.isAwake = false; //enemy behavior reset     
                en.transform.localPosition = en.startingLocation;
                en.displayedHealth = en.startingHealth;
            }
        }
    }

    public void EnemyDestruction() //Destroys all enemy game objects at the end of the level
    {
        //if null or whatever then don't do anything
        foreach (Enemy en in enemiesInLevel)
        {
            if (!en.isBox && en.displayedHealth > 0 && en != null) //should this ignore bosses too?
            {
                Destroy(en);
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
        timelineManager.GetComponent<TimelineManager>().hasCutscenePlayed = false;
        CheckpointCostCheck();
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

    //Overall victory logic
    public void VictoryCheck()
    {
        //Spawn the victory screen here
        //The player script disables movement
        EnemyDestruction(); //simply destroys the enemy component for now
        if (victoryCutscene != null)
        {
            victoryCutscene.SetActive(true);
            pc.canMove = false; //prevents player from moving when cutscene is playing (also means they can't shoot)
            pc.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            pc.GetComponent<SpriteRenderer>().enabled = false;
        }
        else
        {
            ShowVictoryPanels();
        }
    }


    private void ShowVictoryPanels()
    {
        SetActiveChildren(hudRef.transform, false);
        victoryPanel.SetActive(true);
        //freeze player controls here
    }

    public void RetryCheckpoint()//Retry from a checkpoint rather than from the beginning
    {
        //PlayerController pc = FindObjectOfType<PlayerController>();
        if (canRetry == true)
        {
            currency.WalletProperty = (currency.WalletProperty - checkpointCost);
            //A "replenish" function for playercontroller might be best for using checkpoints
            Camera mc = FindObjectOfType<Camera>();

            //using arrays for checkpoints
            if (flaggedCheckpoints[checkpointIndex])
            {
                pc.transform.position = checkpoints[checkpointIndex].transform.position;
                Vector3 camerapoint = new Vector3(pc.transform.position.x, pc.transform.position.y, -10);
                mc.transform.position = camerapoint;
            }

            pc.Replenish();
            CheckpointCostCheck();
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
