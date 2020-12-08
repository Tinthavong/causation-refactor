using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class MainMenuController : MonoBehaviour
{
    public static bool isNewGame;
    public Animator anim;
    public Canvas fadeCanvas;
    public TMP_Text message;
    public TMP_Text saveNotFoundMessage;
    public GameObject warningPanel;

    // Start is called before the first frame update
    void Start()
    {
        anim.SetBool("Transition", true);
        isNewGame = false;
        saveNotFoundMessage.text = "";
    }

    public void NewGame()
    {
        warningPanel.SetActive(false);
        fadeCanvas.gameObject.SetActive(true);
        Time.timeScale = 1f;
        PauseController.isPaused = false;
        SaveManager.instance.DeleteSavedData();
        SaveManager.instance.Save();
        isNewGame = true;
        message.text = "Thank you for playing\nEnjoy the game\n\nAuto Saving New Game...";
        StartCoroutine(LoadNextScene(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public void LoadGame()
    {
        string savePath = Application.persistentDataPath;

        if (File.Exists(savePath + "/" + SaveManager.instance.gameData.saveName + ".dat"))
        {
            fadeCanvas.gameObject.SetActive(true);
            Time.timeScale = 1f;
            PauseController.isPaused = false;
            message.text = "Welcome Back\n\nNow Loading...";
            StartCoroutine(LoadNextScene(SceneManager.GetActiveScene().buildIndex + 1));
            Debug.Log("Saved Game Loaded");
        }
        else
        {
            saveNotFoundMessage.text = "Save File Not Found";
        }

    }

    public void CreditsScreen()
    {
        SceneManager.LoadScene(9);
    }

    public void QuitGame()
    {
        SaveManager.instance.gameData.currency = MissionSelection.currency;
        SaveManager.instance.gameData.iteration = MissionSelection.iterations;
        SaveManager.instance.Save();

        Debug.Log("Quitting Game");
        Application.Quit();
    }

    public void Warning()
    {
        string savePath = Application.persistentDataPath;

        if (File.Exists(savePath + "/" + SaveManager.instance.gameData.saveName + ".dat"))
        {
            warningPanel.SetActive(true);
        }
        else
        {
            fadeCanvas.gameObject.SetActive(true);
            Time.timeScale = 1f;
            PauseController.isPaused = false;
            isNewGame = true;
            message.text = "Thank you for playing\nEnjoy the game\n\nAuto Saving New Game...";
            StartCoroutine(LoadNextScene(SceneManager.GetActiveScene().buildIndex + 1));
        }
    }

    IEnumerator LoadNextScene(int levelIndex)
    {
        anim.SetBool("Transition", true);

        yield return new WaitForSeconds(5);

        SceneManager.LoadScene(levelIndex);
    }
}
