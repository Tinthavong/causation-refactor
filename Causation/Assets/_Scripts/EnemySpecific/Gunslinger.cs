using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Gunslinger : Enemy
{
    public Gunslinger() //constructor
    {
        Health = displayedHealth; //Displayed Health can be set in the inspector
        Stamina = staminaActions;
        //Enemies dont use ammo for now but if it breaks just set the amount here
        Currency = dropValue;
    }
    // Start is called before the first frame update
    void Start()
    {
        floorHax = false;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        player = FindObjectOfType<PlayerController>();
        animator.SetBool("IsShotgunner", false);
    }

    // Update is called once per frame
    void Update()
    {
        //Controls where the enemy is looking
        //First in update to make sure bullet travels in correct direction
        if (isClose())
        {
            Flip(0f);
        }

        //firerateWait changes based on fps time
        firerateWait -= Time.deltaTime;
        //if firerateWait is 0, time to fire and reset the wait
        if (firerateWait <= 0 && isClose() && !isTooClose() && player.displayedHealth > 0 )
        {
            animator.Play("Attack");
            Shoot();
            firerateWait = firerate;

            FindObjectOfType<SFXManager>().PlayAudio("Gunshot");
        }

        //this is only necessary if they will move. right now they are all stationary.
        if (!isClose() || player.displayedHealth <= 0)
        {
            animator.SetBool("IsChasing", false);
        }

        //Consider making melee animations for the hybrid character
        /*
        if (firerateWait <= 0 && isTooClose())
        {
            //animation play here
            //Strike();
            //Using firerate as the buffer for melee for consistency, might replace later
            firerateWait = firerate;
        }*/

        ElimCharacter();//Want to find some way for elimcharacter to be checked each time damage is taken, not on every frame like it is now
    }
}
