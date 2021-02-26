using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class AmmoDisplay : MonoBehaviour
{
    public TMP_Text CurrentAmmoDisplay;
    public TMP_Text MaximumAmmoDisplay;

    string unlimitedAmmo = "∞";

    PlayerBaseStats pbs;
    // Start is called before the first frame update
    void Start()
    {
        pbs = FindObjectOfType<PlayerBaseStats>();
        CurrentAmmoDisplay.text = pbs.CharacterAmmo.ToString();
        CurrentAmmoDisplay.text = unlimitedAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        CurrentAmmoDisplay.text = pbs.CharacterAmmo.ToString();
        
    }
}
