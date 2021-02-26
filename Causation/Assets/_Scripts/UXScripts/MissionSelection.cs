﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class MissionSelection : MonoBehaviour
{
    public static int iterations;
    public static int currency;

    [Header("TMP_Text Boxes")]
    public TMP_Text eraText;
    public TMP_Text currencyText;
    public TMP_Text message;
    
    [Header("Scene Transition Variables")]
    public Animator anim;
    public Canvas fadeCanvas;

    [Header("Save Menu/UnsaveData Warning Panels")]
    public GameObject saveMenu;
    public GameObject unsaveDataWarning;

    [Header("Extra Variables")]
    public Image cover;
    public Button backButton;

    [Header("Grandpa's Missions")]
    public Button era1;
    public Button level1;
    public Button level2;
    public Image padlock1;
    public GameObject GpaMission;

    [Header("Son's Missions")]
    public Button era2;
    public Button level3;
    public Button level4;
    public Image padlock2;
    public Image padlock3;
    public GameObject SonMission;

    [Header("Daughter's Missions")]
    public Button era3;
    public Button level5;
    public Button level6;
    public Image padlock4;
    public Image padlock5;
    public GameObject DauMission;

    private void Start()
    {
        currencyText.text = currency.ToString();

        saveMenu.SetActive(false);
        fadeCanvas.gameObject.SetActive(false);

        if (Currency.walletValue != 0 && iterations != 0)
        {
            UpdateCurrency();
            UpdateIteration();
        }

        //Start of game
        if (iterations == 0)
        {
            era1.enabled = true;
            era2.enabled = false;
            era3.enabled = false;
            level1.enabled = true;
            level2.enabled = false;
            padlock1.gameObject.SetActive(true);
            padlock2.gameObject.SetActive(true);
            padlock4.gameObject.SetActive(true);
        }
        //After Gpa Level 1
        else if (iterations == 1)
        {
            level1.GetComponentInChildren<TMP_Text>().color = Color.gray;
            level1.enabled = false;
            level2.enabled = true;
            padlock1.gameObject.SetActive(false);
            eraText.gameObject.SetActive(false);
            era1.gameObject.SetActive(false);
            era2.gameObject.SetActive(false);
            era3.gameObject.SetActive(false);
            GpaMission.gameObject.SetActive(true);
        }
        //After Gpa Level 2
        else if (iterations == 2)
        {
            era1.GetComponentInChildren<TMP_Text>().color = Color.gray;
            era1.enabled = false;
            era2.enabled = true;
            era3.enabled = false;
            padlock2.gameObject.SetActive(false);
            padlock3.gameObject.SetActive(true);
            padlock4.gameObject.SetActive(true);
            level3.enabled = true;
            level4.enabled = false;
        }
        //After Son Level 1
        else if (iterations == 3)
        {
            level3.GetComponentInChildren<TMP_Text>().color = Color.gray;
            level3.enabled = false;
            level4.enabled = true;
            padlock3.gameObject.SetActive(false);
            era1.gameObject.SetActive(false);
            era2.gameObject.SetActive(false);
            era3.gameObject.SetActive(false);
            SonMission.gameObject.SetActive(true);
            eraText.gameObject.SetActive(false);
        }
        //After Son Level 2
        else if (iterations == 4)
        {
            era1.GetComponentInChildren<TMP_Text>().color = Color.gray;
            era2.GetComponentInChildren<TMP_Text>().color = Color.gray;
            era1.enabled = false;
            era2.enabled = false;
            era3.enabled = true;
            padlock2.gameObject.SetActive(false);
            padlock4.gameObject.SetActive(false);
            padlock5.gameObject.SetActive(true);
            level5.enabled = true;
            level6.enabled = false;
        }
        //After Daughter Level 1
        else if (iterations == 5)
        {
            level5.GetComponentInChildren<TMP_Text>().color = Color.gray;
            level5.enabled = false;
            level6.enabled = true;
            padlock5.gameObject.SetActive(false);
            era1.gameObject.SetActive(false);
            era2.gameObject.SetActive(false);
            era3.gameObject.SetActive(false);
            DauMission.SetActive(true);
            eraText.gameObject.SetActive(false);
        }
        //After Daughter Level 2/Post Game
        else
        {
            level1.GetComponentInChildren<TMP_Text>().color = Color.white;
            level3.GetComponentInChildren<TMP_Text>().color = Color.white;
            level5.GetComponentInChildren<TMP_Text>().color = Color.white;
            era1.enabled = true;
            era2.enabled = true;
            era3.enabled = true;
            padlock1.gameObject.SetActive(false);
            padlock2.gameObject.SetActive(false);
            padlock3.gameObject.SetActive(false);
            padlock4.gameObject.SetActive(false);
            padlock5.gameObject.SetActive(false);
            cover.gameObject.SetActive(false);
            backButton.gameObject.SetActive(false);
        }
    }

    public void Update()
    {
        currencyText.text = currency.ToString();
    }

    public void Mission1()
    {
        SceneManager.LoadScene(3);
    }

    public void Mission2()
    {
        SceneManager.LoadScene(4);
    }

    public void Mission3()
    {
        SceneManager.LoadScene(5);
    }

    public void Mission4()
    {
        SceneManager.LoadScene(6);
    }

    public void Mission5()
    {
        SceneManager.LoadScene(7);
    }

    public void Mission6()
    {
        SceneManager.LoadScene(8);
    }

    void UpdateCurrency()
    {
        SaveManager.instance.gameData.currency = MenuController.totalGamePoints;
        currency = SaveManager.instance.gameData.currency;
    }

    void UpdateIteration()
    {
        SaveManager.instance.gameData.iteration = iterations;
    }

    public void SaveGame()
    {
        SaveManager.instance.gameData.currency = currency;
        SaveManager.instance.gameData.iteration = iterations;
        message.text = "Game has been saved";
        SaveManager.instance.Save();
        saveMenu.SetActive(false);
        SaveManager.instance.hasSaved = true;
    }

    public void ClearSaveData()
    {
        saveMenu.SetActive(false);

        SaveManager.instance.DeleteSavedData();

        SceneManager.LoadScene(1);
    }

    public void GameSavedToMainMenu()
    {
        if (SaveManager.instance.hasSaved)
        {
            SceneManager.LoadScene(1);
        }
        else
        {
            unsaveDataWarning.SetActive(true);
        }
        
    }
}
