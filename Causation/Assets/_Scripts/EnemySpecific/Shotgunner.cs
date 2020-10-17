using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Shotgunner : Enemy
{
    public Shotgunner() //constructor
    {
        Health = displayedHealth; //Displayed Health can be set in the inspector
        Stamina = staminaActions;
        //Enemies dont use ammo for now but if it breaks just set the amount here
        Currency = dropValue;
    }

    [Header("Special variables")]
    //Range before shotgunsplosion happens
    public int fireRange = 5;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isClose())
        {
            Flip(0);
            RunTowards();
        }

        //HERE IS WHERE I STOPPED
        //Going to implement a new shotgun specific shoot method that fires multiple shots in a small cone
        //Afterwards will have a shotgunner prefab to attach to
    }

    public void RunTowards()
    {
        if (Math.Abs(player.transform.position.x - this.gameObject.transform.position.x) >= fireRange)
        {
            Vector3 movement = new Vector3(-enemySpeed, 0.0f, 0.0f);
            transform.position = transform.position + movement * Time.deltaTime;
        }
    }

    private bool isClose() 
    {
        if (Math.Abs(player.transform.position.x - this.gameObject.transform.position.x) < sightRange && player.displayedHealth > 0) //Dirty fix. Stop, he's already dead!
        {
            return true;
        }
        return false;
    }

    private bool isCloseEnough()//Shotguns are never too close hehehehehe
    {
        if (Math.Abs(player.transform.position.x - this.gameObject.transform.position.x) <= fireRange && player.displayedHealth > 0) 
        {
            return true;
        }
        return false;
    }
}
