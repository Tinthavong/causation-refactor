using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    public GameObject pauseScreen;
    public GameObject HUD;
    public static bool isPaused = false;

    // Start is called before the first frame update
    void Start()
    {
        isPaused = false;
        pauseScreen.SetActive(false);
        HUD.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    void PauseGame()
    {
        pauseScreen.SetActive(true);
        HUD.SetActive(false);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseScreen.SetActive(false);
        HUD.SetActive(true);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void MainMenu()
    {
        MissionSelection.iterations -= 1;
        SaveManager.instance.gameData.currency = MissionSelection.currency;
        SaveManager.instance.gameData.iteration = MissionSelection.iterations;
        SaveManager.instance.Save();

        SceneManager.LoadScene(1);
    }
}
