using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    private int lastLoad;
    private int loadNextScene;
    public Animator anim;
    public Canvas fadeCanvas;

    private void Start()
    {
        loadNextScene = SceneManager.GetActiveScene().buildIndex + 1;
        anim.SetBool("Transition", true);
        //fadeCanvas.SetActive(true);
    }

    public void PlayGame()
    {
        Time.timeScale = 1f;
        PauseController.isPaused = false;
        //SceneManager.LoadScene(2);
        StartCoroutine(LoadNextScene(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public void CreditsScreen()
    {
        SceneManager.LoadScene(8);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game");
        Application.Quit();
    }

    public void LoadGame()
    {
        Debug.Log("Saved Game Loaded");
    }

    public void MissionSelect()
    {
        StartCoroutine(LoadNextScene(SceneManager.GetActiveScene().buildIndex + 1));
    }
    public void RestartGame()
    {
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
        if (LM.flaggedCheckpoint)
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

    IEnumerator LoadNextScene(int levelIndex)
    {
        anim.SetBool("Transition", true);

        yield return new WaitForSeconds(3);

        SceneManager.LoadScene(levelIndex);
    }
}
