using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MissionSelection : MonoBehaviour
{
    public void Mission1()
    {
        SceneManager.LoadScene(5);
    }

    public void Mission2()
    {
        SceneManager.LoadScene(6);
    }

    public void Mission3()
    {
        SceneManager.LoadScene(7);
    }
}
