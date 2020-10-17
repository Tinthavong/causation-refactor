using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MissionSelection : MonoBehaviour
{
    private int selection = 0;
    public Button level1;
    public Button level2;
    public Button level3;

    private void Start()
    {
        if (selection == 0)
        {
            level1.enabled = true;
            level2.enabled = false;
            level3.enabled = false;
        }
    }

    public void Mission1()
    {
        selection = 1;
        SceneManager.LoadScene(5);
    }

    public void Mission2()
    {
        if (selection == 1)
        {
            selection = 2;
            level1.enabled = false;
            level2.enabled = true;
            level3.enabled = false;
            SceneManager.LoadScene(6);
        }
        
    }

    public void Mission3()
    {
        SceneManager.LoadScene(7);
    }
}
