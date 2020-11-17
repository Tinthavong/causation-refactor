using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MissionSelectionController : MonoBehaviour
{
    public GameObject selectionScreen;
    public static bool isNearComputer;


    // Start is called before the first frame update
    void Start()
    {

        Time.timeScale = 1f;
        isNearComputer = false;
        selectionScreen.SetActive(false);

        if (MainMenuController.isNewGame == true)
        {
            SaveManager.instance.DeleteSavedData();
            //SceneManager.LoadScene(SceneManager.GetActiveScene().name);

            MissionSelection.currency = SaveManager.instance.gameData.currency;
            MissionSelection.iterations = SaveManager.instance.gameData.iteration;

            MainMenuController.isNewGame = false;
        }

        if (SaveManager.instance.hasLoaded)
        {
            MissionSelection.currency = SaveManager.instance.gameData.currency;
            MissionSelection.iterations = SaveManager.instance.gameData.iteration;
        }
        else
        {
            SaveManager.instance.gameData.currency = MissionSelection.currency;
            SaveManager.instance.gameData.iteration = MissionSelection.iterations;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Only for when the player is in the hideout and is near the computer
        if (Input.GetKeyDown(KeyCode.W) && isNearComputer == false)
        {
            isNearComputer = true;
            Time.timeScale = 0f;
        }

        if (isNearComputer == true)
        {
            selectionScreen.SetActive(true);
            isNearComputer = false;
        }
    }

    public void Resume()
    {
        Time.timeScale = 1f;
    }
}
