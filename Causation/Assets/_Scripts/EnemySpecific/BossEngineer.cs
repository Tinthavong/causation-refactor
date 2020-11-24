using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEngineer : Enemy
{
    /*
     * Boss ideas:
     * 
     * -boss goes through 3 phases: shotgun phase, rifle phase, charger phase, taking place on a flat battlefield with one platform in the middle
     * Shotgun: mimics shotgunner enemies but with larger sight range
     * Rifle phase: engineer fires a couple rifle shots in players direction before reloading and doing it once more
     * Charger: mimics charger enemy but does more damage with melee attack
     * In between phases: crouches maybe?
     * 
     * -Boss combines ai of all enemies:  Ill go with this option for ease of coding
     * charges at close range, gets close at medium for shotgun, shoots a stream of rifle shots at long range
    */
    public BossEngineer() //constructor
    {
        Health = displayedHealth; //Displayed Health can be set in the inspector
        Stamina = staminaActions;
        //Enemies dont use ammo for now but if it breaks just set the amount here
        Currency = dropValue;
    }

    [Header("Special Variables")] //Variables are subject to change for balance
    //When shotgun ai kicks in
    public float shotgunRange = 13;
    //Range before can fire shotguns bullets
    public float fireRange = 7;
    //New bulletspawns for shotgun bullets
    public GameObject bulletSpawn2;
    public GameObject bulletSpawn3;
    //When charger ai kicks in
    public float chargerRange = 5;
    //gunslinger ai kicks in at sightRange, which will be far

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        player = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        onGround = (Physics2D.Raycast(transform.position + colliderOffset, Vector2.down, groundLength, groundLayer) || Physics2D.Raycast(transform.position - colliderOffset, Vector2.down, groundLength, groundLayer));

        if (IsClose())
        {
            Flip(0);
            //checks closest sight range first
            if(inRangeForCharger())
            {
                chargerAI();
            }
            else if(inRangeForShotgun())
            {
                shotgunAI();
            }
            else
            {
                gunslingerAI();
            }
        }



        ElimCharacter();
    }

    private bool inRangeForShotgun()
    {
        if (Math.Abs(player.transform.position.x - this.gameObject.transform.position.x) < shotgunRange && player.displayedHealth > 0) //Dirty fix. Stop, he's already dead!
        {
            return true;
        }
        return false;
    }

    private bool inRangeForCharger()
    {
        if (Math.Abs(player.transform.position.x - this.gameObject.transform.position.x) < chargerRange && player.displayedHealth > 0) //Dirty fix. Stop, he's already dead!
        {
            return true;
        }
        return false;
    }

    private void chargerAI()
    {
        //Run close and melee for larger damage than normal
    }

    private void shotgunAI()
    {
        //Run close and fire a shotgun of bullets
    }

    private void gunslingerAI()
    {
        //Shoot from far away faster than an normal rifleman
    }

}
