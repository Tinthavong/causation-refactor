using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ChargerEnemy : Enemy
{
    Animator animator;

    //Probably like 7 or so enemy scripts in total, bosses included in that number
    public ChargerEnemy() //constructor
    {
        Health = displayedHealth; //Displayed Health can be set in the inspector
        Stamina = staminaActions;
        //Enemies dont use ammo for now but if it breaks just set the amount here
        Currency = dropValue;
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        player = FindObjectOfType<PlayerController>();
        //This should use an actual find method/algorithm instead of just knowing where the player is - maybe later down the line, but this works for now
    }

    // Update is called once per frame
    void Update()
    {
        //Controls where the enemy is looking
        if (isClose())
        {
            Flip(0f);
        }

        //firerateWait changes based on fps time
        firerateWait -= Time.deltaTime;
        //if firerateWait is 0, time to strike and reset the wait
        if (isClose() && !isTooClose())
        {
            animator.SetBool("IsChasing", true);
            RunTowards();
        }

        if (firerateWait <= 0 && isTooClose() && player.displayedHealth > 1)
        {
            animator.SetBool("IsChasing", false);
            animator.SetBool("IsAttacking", true);
            Strike();
            //Firerate makes sense here as melee is this enemies only attack
            firerateWait = firerate;
        }

        if(!isTooClose() || player.displayedHealth <= 0)
        {
            animator.SetBool("IsAttacking", false);
        }

        ElimCharacter();
    }

    public override void DamageCalc(int damage)
    {
        animator.Play("Damaged");
        base.DamageCalc(damage);
    }

    //Moves towards the player (unimplemented)
    public void RunTowards()
    {
        if(facingRight && !isTooClose())
        {
            Vector3 movement = new Vector3(-enemySpeed, 0.0f, 0.0f);
            transform.position = transform.position + movement * Time.deltaTime;
        }
        else if (!isTooClose())
        {
            Vector3 movement = new Vector3(enemySpeed, 0.0f, 0.0f);
            transform.position = transform.position + movement * Time.deltaTime;
        } 
    }

    //isClose and isTooClose are specific to gunslinger enemies, at least currently
    //Checks to see if the player object is within a certain distance
    private bool isClose()
    {
        if (Math.Abs(player.transform.position.x - this.gameObject.transform.position.x) < sightRange)
        {
            return true;
        }
        return false;
    }

    //Unused currently, will be implemented with melee support
    private bool isTooClose()
    {
        if (Math.Abs(player.transform.position.x - this.gameObject.transform.position.x) < meleeRange)
        {
            return true;
        }
        return false;
    }

    public override void Flip(float dump) //dump because it doesn't matter but it's needed or errors
    {
        if (player.transform.position.x < this.transform.position.x && !facingRight)
        {
            facingRight = true;
            Vector3 scale = transform.localScale;
            scale.x *= -1;

            transform.localScale = scale;
        }

        else if (player.transform.position.x >= this.transform.position.x && facingRight)
        {
            facingRight = false;
            Vector3 scale = transform.localScale;
            scale.x *= -1;

            transform.localScale = scale;
        }
    }

    void Death()
    {
        Destroy(this.gameObject);
    }
}
