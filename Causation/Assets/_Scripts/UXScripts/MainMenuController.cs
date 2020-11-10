using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public static bool isNewGame;
    public Animator anim;
    public Canvas fadeCanvas;

    // Start is called before the first frame update
    void Start()
    {
        anim.SetBool("Transition", true);
        isNewGame = false;
    }

    public void NewGame()
    {
        fadeCanvas.gameObject.SetActive(true);
        Time.timeScale = 1f;
        PauseController.isPaused = false;
        isNewGame = true;
        StartCoroutine(LoadNextScene(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public void LoadGame()
    {
        fadeCanvas.gameObject.SetActive(true);
        Time.timeScale = 1f;
        PauseController.isPaused = false;
        StartCoroutine(LoadNextScene(SceneManager.GetActiveScene().buildIndex + 1));
        Debug.Log("Saved Game Loaded");
    }

    public void CreditsScreen()
    {
        SceneManager.LoadScene(9);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game");
        Application.Quit();
    }

    IEnumerator LoadNextScene(int levelIndex)
    {
        anim.SetBool("Transition", true);

        yield return new WaitForSeconds(5);

        SceneManager.LoadScene(levelIndex);
    }
}
