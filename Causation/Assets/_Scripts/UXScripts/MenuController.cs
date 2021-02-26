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

        //These are also returning a null reference for the game over, I think just finding a way to separate victory and gameover would fix it
        //Band-aid fix could be, if gameobject.Name == GameWinPanel then so and so, else if gameobject.Name == GameOverPanel then yada yada
        if (this.gameObject.name == "GameWinPanel")
        {
            currentLevelPoints = Currency.walletValue * 100;
            totalGamePoints = currentLevelPoints + MissionSelection.currency;

            currentPointsTXT.text = "Points from Level: " + currentLevelPoints.ToString();
            totalGamePointsTXT.text = "Total Game Points: " + totalGamePoints.ToString();

            switch (MissionSelection.iterations)
            {
                case 0:
                    {
                        MissionSelection.iterations = 1;
                        break;
                    }
                case 1:
                    {
                        MissionSelection.iterations = 2;
                        break;
                    }
                case 2:
                    {
                        MissionSelection.iterations = 3;
                        break;
                    }
                case 3:
                    {
                        MissionSelection.iterations = 4;
                        break;
                    }
                case 4:
                    {
                        MissionSelection.iterations = 5;
                        break;
                    }
                case 5:
                    {
                        MissionSelection.iterations = 6;
                        break;
                    }
            }

            SaveManager.instance.gameData.currency = totalGamePoints;
            SaveManager.instance.gameData.iteration = MissionSelection.iterations;
            SaveManager.instance.Save();
        }
        if (this.gameObject.name == "GameOverPanel")
        {
            totalGamePoints = SaveManager.instance.gameData.currency;
            totalGamePointsTXT.text = "Total Game Points: " + totalGamePoints.ToString();
        }
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        SaveManager.instance.gameData.currency = totalGamePoints;
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
        if (LM.flaggedCheckpoints[LM.checkpointIndex])
        {
            LM.RetryCheckpoint();
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
