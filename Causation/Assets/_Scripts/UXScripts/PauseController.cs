using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour
{
    public GameObject pauseScreen;
    public static bool isPaused = false;

    // Start is called before the first frame update
    void Start()
    {
        pauseScreen.SetActive(false);
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
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseScreen.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }
}
