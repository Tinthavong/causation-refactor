using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSwitch : MonoBehaviour
{
    public GameObject gpBG;
    public GameObject sonBG;
    public GameObject gdBG;

    void Start()
    {
        if (MissionSelection.iterations == 0 || MissionSelection.iterations == 1)
        {
            gpBG.SetActive(true);
            sonBG.SetActive(false);
            gdBG.SetActive(false);
        }
        if (MissionSelection.iterations == 2 || MissionSelection.iterations == 3)
        {
            gpBG.SetActive(false);
            sonBG.SetActive(true);
            gdBG.SetActive(false);
        }
        if (MissionSelection.iterations == 4 || MissionSelection.iterations == 5)
        {
            gpBG.SetActive(false);
            sonBG.SetActive(false);
            gdBG.SetActive(true);
        }
    }
}
