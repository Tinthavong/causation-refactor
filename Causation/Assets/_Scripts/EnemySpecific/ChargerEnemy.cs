using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ChargerEnemy : Enemy
{
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
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        player = FindObjectOfType<PlayerController>();
        //This should use an actual find method/algorithm instead of just knowing where the player is - maybe later down the line, but this works for now
    }

    // Update is called once per frame
    void Update()
    {
        onGround = (Physics2D.Raycast(transform.position + colliderOffset, Vector2.down, groundLength, groundLayer) || Physics2D.Raycast(transform.position - colliderOffset, Vector2.down, groundLength, groundLayer));
        //Controls where the enemy is looking
        if (isClose())
        {
            Flip(0f);
        }

        //firerateWait changes based on fps time
        firerateWait -= Time.deltaTime;
        //if firerateWait is 0, time to strike and reset the wait
        if (isClose() && !isTooClose() && vertRangeSeesPlayer() && onGround)
        {
            animator.SetBool("IsChasing", true);
            RunTowards();
        }

        if (firerateWait <= 0 && isTooClose() && player.displayedHealth > 1 && vertRangeSeesPlayer())
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

    //Moves towards the player - Needs a bug fix for vertical movement, currently kinda just glides over to the player before falling
    public void RunTowards()
    {
        if(facingRight)
        {
            Vector2 movement = new Vector2(-enemySpeed, 0.0f);
            rb.MovePosition(rb.position + movement * Time.fixedDeltaTime);

            //transform.position = transform.position + movement * Time.deltaTime;

        }
        else
        {
            Vector2 movement = new Vector2(enemySpeed, 0.0f);
            rb.MovePosition(rb.position + movement * Time.fixedDeltaTime);

           // transform.position = transform.position + movement * Time.deltaTime;
        } 
    }

    void Death()
    {
        Destroy(this.gameObject);
    }
}
