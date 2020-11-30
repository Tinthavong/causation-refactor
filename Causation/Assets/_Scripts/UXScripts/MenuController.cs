using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MenuController : MonoBehaviour
{
    //Text displaying the current and total points that player has
    //Only displays in the Victory Screen
    public TMP_Text currentPointsTXT;
    public TMP_Text totalGamePointsTXT;

    //Varaible holding the total points that player has
    public static int totalGamePoints;

    private int lastLoad;
    private int loadNextScene;
    private int currentLevelPoints;
    

    private void Start()
    {
        loadNextScene = SceneManager.GetActiveScene().buildIndex + 1;
        
        currentLevelPoints = Currency.walletValue * 100;
        totalGamePoints = currentLevelPoints + MissionSelection.currency;
        
        currentPointsTXT.text = "Points from Level: " + currentLevelPoints.ToString();
        totalGamePointsTXT.text = "Total Game Points: " + totalGamePoints.ToString();

        SaveManager.instance.gameData.currency = totalGamePoints;
        SaveManager.instance.Save();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        SaveManager.instance.gameData.currency = MissionSelection.currency;
        SaveManager.instance.gameData.iteration = MissionSelection.iterations;
        SaveManager.instance.Save();

        Debug.Log("Quitting Game");
        Application.Quit();
    }

    public void MissionSelect()
    {
        SceneManager.LoadScene(2);
    }

    public void RestartGame()
    {
        Currency.walletValue = 0;
        Time.timeScale = 1f;
        lastLoad = PlayerPrefs.GetInt("SavedScene");
        if (lastLoad != 0)
        {
            SceneManager.LoadScene(lastLoad);
        }
        else
        {
            return;
        }
    }

    public void Retry() //Meant to retry from checkpoint rather than restart/reload the whole scene
    {
        LevelManager LM = FindObjectOfType<LevelManager>();
        if (LM.flaggedCheckpoint || LM.flaggedCheckpoint2)
        {
            LM.RetryCheckpoint();
        }
        else
        {
            RestartGame();//No checkpoint therefore go back to start
        }
    }

    public void NextLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(loadNextScene);
        //lastLoad = PlayerPrefs.GetInt("SavedScene");
        //if (lastLoad != 0)
        //{
        //    SceneManager.LoadScene(lastLoad + 1);
        //}
        //else
        //{
        //    return;
        //}
    }
}
