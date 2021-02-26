using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateManager : MonoBehaviour
{
    [Header("Character States")]
    //Movement states
    public bool isJumping;
    public bool isCrouched = false;

    //Player combat states
    public bool isShooting;
    public bool isOutOfAmmo;

    //Player gameplay loop states
    public bool isInvincible = false; //Can be a state, player is damaged therefore is temp invincible
    public bool isControlling = true; //Is a state, player is able to control their character out of cutscenes

    PlayerBaseStats charBaseStats;

    //UI Reference, hopefully this counts as a manager and a calculation and isn't out of place
    HealthBar healthbar;

    // Start is called before the first frame update
    void Start()
    {
        healthbar = FindObjectOfType<HealthBar>();
        isControlling = true; //For now set it true at the top
        charBaseStats = GetComponent<PlayerBaseStats>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AmmoState()
    {
        if (charBaseStats.CharacterAmmo <= 0)
        {
            isOutOfAmmo = true;
        }
        else
        {
            isOutOfAmmo = false;
        }
    }

    public void AmmoReloadCalulcation()
    {
        if (charBaseStats.CharacterAmmo < charBaseStats.maxCharacterAmmo)
        {
            charBaseStats.CharacterAmmo += charBaseStats.shotsFired;
            //Debug.Log(charBaseStats.CharacterAmmo + charBaseStats.shotsFired);
            charBaseStats.shotsFired = 0;
            AmmoState();
        }
    }

    //Shots fired!
    public void AmmoUsageCalculation()
    {
        if (charBaseStats.shotsFired < charBaseStats.maxCharacterAmmo && !isOutOfAmmo)
        {
            charBaseStats.shotsFired++;
            charBaseStats.CharacterAmmo--;
            AmmoState();
            Debug.Log(charBaseStats.CharacterAmmo);
        }
        else if (charBaseStats.shotsFired >= charBaseStats.maxCharacterAmmo && isOutOfAmmo)
        {
            //Player state set to out of ammo
            AmmoState();
            Debug.Log("Out of ammo!");
        }
    }

    public void DamageCalculation(int damageValue)
    {
        charBaseStats.CharacterHealth -= damageValue;
        healthbar.UpdateHealthBar(charBaseStats.CharacterHealth);
    }

    public void HealCalculation(int healValue)
    {
        /*
            if (charBaseStats.CharacterHealth < charBaseStats.maxCharacterHealth)
            {
                charBaseStats.CharacterHealth += healValue;
            }
        */
    }
}
