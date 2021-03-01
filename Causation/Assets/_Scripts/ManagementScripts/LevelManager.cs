using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public Currency currency;

    [Header("Game is Over References")]
    public GameObject victoryPoint;
    public GameObject levelLoader;
    public GameObject victoryPanel;
    public GameObject GameOverPanel;

    [Header("Cutscene References")]
    public GameObject victoryCutscene;
    public GameObject[] timelineManagers;

    private GameObject hudRef;
    private PlayerStateManager psm;

    [Header("Checkpoint Values")]
    public GameObject[] checkpoints; //Checkpoints in the scene, set in the inspector
    public bool[] flaggedCheckpoints;
    public int checkpointIndex; //The current checkpoint that the player can reference, -1 means no checkpoint
    public bool canRetry = false; //can afford the toll/checkpoint cost
    public int checkpointCost;
    public Button retryButton;

    void Start()
    {
        psm = FindObjectOfType<PlayerStateManager>();

        flaggedCheckpoints = new bool[checkpoints.Length];
        checkpointIndex = -1;
        retryButton.interactable = false;

        hudRef = GameObject.Find("HUDElements");
        GameOverPanel = GameObject.Find("GameOverScreen");
        currency = FindObjectOfType<Currency>();
    }

    //Game over logic
    public void GameOver()
    {
        for(int i = 0; i < timelineManagers.Length; i++)
        {
            timelineManagers[i].GetComponent<TimelineManager>().hasCutscenePlayed = false;
        }
        CheckpointCostCheck();
        psm.isControlling = false;
        SetActiveChildren(hudRef.transform, false);
        SetActiveChildren(GameOverPanel.transform, true);
    }

    //Sets the children of UI objects inactive
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
        if (victoryCutscene != null)
        {
            victoryCutscene.SetActive(true);
            psm.isControlling = false;
            psm.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            psm.GetComponent<SpriteRenderer>().enabled = false;
        }
        else
        {
            ShowVictoryPanels();
        }
    }
    
    private void ShowVictoryPanels()
    {
        SetActiveChildren(hudRef.transform, false);
        psm.isControlling = false;
        victoryPanel.SetActive(true);
    }

    //Retry from a checkpoint rather than from the beginning if the player meets the necessary requirements:
    //flagged checkpoints
    //Can afford the cost of the checkpoint (set in the inspector as 5)
    public void RetryCheckpoint()
    {
        if (canRetry == true)
        {
            currency.WalletProperty = (currency.WalletProperty - checkpointCost);
            //A "replenish" function for playercontroller might be best for using checkpoints
            Camera mc = FindObjectOfType<Camera>();
            //using arrays for checkpoints
            if (flaggedCheckpoints[checkpointIndex])
            {
                psm.transform.position = checkpoints[checkpointIndex].transform.position;
                Vector3 camerapoint = new Vector3(psm.transform.position.x, psm.transform.position.y, -10);
                mc.transform.position = camerapoint;
            }
        }
        psm.Replenish();
        CheckpointCostCheck();
        SetActiveChildren(hudRef.transform, true);
        SetActiveChildren(GameOverPanel.transform, false);

    }

    //Checkpoint cost is set in the inspector but could ideally scale with different difficulty settings
    //Checks to ensure that the player is eligble to retry from a checkpoint
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
