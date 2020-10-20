using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class MissionSelection : MonoBehaviour
{
    public static int selection = 0;
    public Button level1;
    public Button level2;
    public Button level3;
    public Image padlock1;
    public Image padlock2;

    //private void Start()
    //{
    //    if (selection == 0)
    //    {
    //        level1.enabled = true;
    //        level2.enabled = false;
    //        level3.enabled = false;
    //        padlock1.enabled = true;
    //        padlock2.enabled = true;
    //    }
    //    else if (selection == 1)
    //    {
    //        level1.enabled = true;
    //        level2.enabled = true;
    //        level3.enabled = false;
    //        padlock1.enabled = false;
    //        padlock2.enabled = true;
    //    }
    //    else if (selection == 2)
    //    {
    //        level1.enabled = true;
    //        level2.enabled = true;
    //        level3.enabled = true;
    //        padlock1.enabled = false;
    //        padlock2.enabled = false;
    //    }
    //}

    private void Start()
    {

        level1.enabled = true;
        level2.enabled = true;
        level3.enabled = true;
        padlock1.enabled = false;
        padlock2.enabled = false;

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
}
