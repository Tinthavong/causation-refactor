using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Bossman1 : Enemy
{

    //Special Notes:
    //This boss will not move, it will be stationary with the ability to shoot both ways
    //This boss has 3 phases, high gatling, low gatling, and an alternating fire phase

    public Bossman1() //constructor
    {
        Health = displayedHealth; //Displayed Health can be set in the inspector
        Stamina = staminaActions;
        //Enemies dont use ammo for now but if it breaks just set the amount here
        Currency = dropValue;
    }
    // Start is called before the first frame update

    [Header("Special Variables")]
    //Boss will go through phases one by one in a pattern
    private int phase = 0;

    //New phase timers to allow for transitions to new phases, allowing damage to be done in between phases
    private float phaseRateWait = 0f;
    public float phaseRate = 4f;

    //Phase bool to make sure timer doesnt go down while  he is in a phase, possibly starting a new one while in a phase (which is bad)
    private bool isInPhase = false;

    //Controls amount of bullets fired while in a phase
    private int bulletsFired = 0;
    private int bulletsFiredStop = 8;

    //Two variables used to determine the bullet starts for the gatlings on both sides.  Each side fires simultaneously.
    public GameObject gatlingStartLeft;
    public GameObject gatlingStartRight;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isInPhase)
        {
            phaseRateWait -= Time.deltaTime;
        }

        if (phaseRateWait <= 0 && !isTooClose())
        {
            //phase is changed in the shoot method
            Shoot();
            phaseRateWait = phaseRate;
        }

        //This fires regardless of the phase, bad player no get close
        if (firerateWait <= 0 && isTooClose())
        {
            LaserShot();
            firerateWait = firerate;
        }

        ElimCharacter();//Want to find some way for elimcharacter to be checked each time damage is taken, not on every frame like it is now
    }

    //Used for close range lasers
    private bool isTooClose()
    {
        if (Math.Abs(player.transform.position.x - this.gameObject.transform.position.x) < meleeRange && player.displayedHealth > 0)
        {
            return true;
        }
        return false;
    }

    //Does something different each phase, each phase changes the phase variable to the next phase manually
    private new void Shoot()
    {
        isInPhase = true;
        switch(phase)
        {
            case 0:
                gatlingStream();
                phase = 1;
                break;
            case 1:
                changeGatlingHeight();
                gatlingStream();
                phase = 2;
                break;
            case 2:
                alternatingStream();
                phase = 0;
                break;
        }
        isInPhase = false;
    }

    //Shoots a stream of bullets on the level that the guns are on
    private void gatlingStream()
    {
        //Firerate needs to be set really low for this to work well:  will shoot predetermined amount of bullets then end the phase
    }

    //switches back and forth between high and low shots for a bit
    private void alternatingStream()
    {
        //Firerate will be psuedo controlled by the animations ends for switching heights
    }

    //just switches from high to low or vice versa
    private void changeGatlingHeight()
    {
        //basically just the animation as well as moving the gatlingStart's
    }

    //Happens whenever player gets close, hopefully how ive coded it it will activate even in the middle of a phase
    private void LaserShot()
    {
        //Nothing here yet, this is the close range attack
    }
}
