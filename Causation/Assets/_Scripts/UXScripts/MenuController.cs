using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    private int lastLoad;
    private int loadNextScene;

    private void Start()
    {
        loadNextScene = SceneManager.GetActiveScene().buildIndex + 1;
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
        MissionSelection.iterations -= 1;
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
