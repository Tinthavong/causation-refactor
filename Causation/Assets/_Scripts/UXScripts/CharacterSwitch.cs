using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSwitch : MonoBehaviour
{
    public GameObject Grandpa;
    public GameObject Son;
    public GameObject Granddaughter;

    void Start()
    {
        if (MissionSelection.iterations == 0 || MissionSelection.iterations == 1)
        {
            Grandpa.SetActive(true);
            Son.SetActive(false);
            Granddaughter.SetActive(false);
        }
        if (MissionSelection.iterations == 2 || MissionSelection.iterations == 3)
        {
            Grandpa.SetActive(false);
            Son.SetActive(true);
            Granddaughter.SetActive(false);
        }
        if (MissionSelection.iterations == 4 || MissionSelection.iterations == 5)
        {
            Grandpa.SetActive(false);
            Son.SetActive(false);
            Granddaughter.SetActive(true);
        }
    }
}
