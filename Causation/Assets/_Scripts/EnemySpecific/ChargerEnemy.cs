using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ChargerEnemy : Enemy
{
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
        startingHealth = displayedHealth;
        startingLocation = gameObject.transform.localPosition;

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        player = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsClose()) //maybe a proper detection method would work better, right now they stop chasing a little bit too soon
        {
            isAwake = true;
        }
        else if (!IsClose() && displayedHealth > 0) //If the enemy isn't close and is not dead then...
        {
            isAwake = false;
            animator.Play("Idle"); //Idle animation doesn't even exist for most/all enemies, this can probably be left out
        }

        if (isAwake)
        {
            onGround = (Physics2D.Raycast(transform.position + colliderOffset, Vector2.down, groundLength, groundLayer) || Physics2D.Raycast(transform.position - colliderOffset, Vector2.down, groundLength, groundLayer));
            //Controls where the enemy is looking
            if (IsClose())
            {
                Flip(0f);
            }

            //firerateWait changes based on fps time
            firerateWait -= Time.deltaTime;
            //if firerateWait is 0, time to strike and reset the wait
            if (IsClose() && !IsTooClose() && VertRangeSeesPlayer() && onGround)
            {
                animator.SetBool("IsChasing", true);
                isChasing = true;
                RunTowards();
            }

            if (firerateWait <= 0 && IsTooClose() && player.displayedHealth > 0 && VertRangeSeesPlayer())
            {
                animator.SetBool("IsChasing", false);
                isChasing = false;
                animator.SetBool("IsAttacking", true);
                Strike();
                //Firerate makes sense here as melee is this enemies only attack
                firerateWait = firerate;
            }

            if (!IsTooClose() || player.displayedHealth <= 0)
            {
                animator.SetBool("IsAttacking", false);
            }

            //Makes the enemy horizontal speed lower while falling
            if (!onGround)
            {
                Vector2 airBrake = new Vector2(-(rb.velocity.x / 2), 0.0f);
                rb.AddForce(airBrake);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + colliderOffset, transform.position + colliderOffset + Vector3.down * groundLength);
        Gizmos.DrawLine(transform.position - colliderOffset, transform.position - colliderOffset + Vector3.down * groundLength);

    }
}
