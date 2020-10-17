using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    private int lastLoad;
    public void PlayGame()
    {
        Time.timeScale = 1f;
        PauseController.isPaused = false;
        SceneManager.LoadScene(5);
    }

    public void CreditsScreen()
    {
        SceneManager.LoadScene(2);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(3);
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

    public void RestartGame()
    {
        lastLoad = PlayerPrefs.GetInt("SavedScene");
    }

    //IEnumerator Game()
    //{
    //    anim.SetTrigger("Start");

    //    yield return new WaitForSeconds(1);

    //    SceneManager.LoadScene(5);
    //}
}
