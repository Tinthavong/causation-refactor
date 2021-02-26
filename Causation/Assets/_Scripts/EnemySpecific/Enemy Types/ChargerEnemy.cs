using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ChargerEnemy : EnemyBaseCombatController
{
    /*
    EnemyBase enemyBase;
    EnemyBaseStats enemyBaseStats;
    EnemyDetection enemyDetection;

    PlayerStateManager psm;
    */
    /*
   // Start is called before the first frame update
   void Start()
   {

       enemyBase = GetComponent<EnemyBase>();
       enemyBaseStats = GetComponent<EnemyBaseStats>();
       enemyDetection = GetComponent<EnemyDetection>();
       animator = GetComponent<Animator>();
       
}
*/
    // Update is called once per frame
    void Update()
    {
        if (enemyDetection.IsClose()) //maybe a proper detection method would work better, right now they stop chasing a little bit too soon
        {
            isAwake = true;
        }
        else if (!enemyDetection.IsClose() && enemyBaseStats.CharacterHealth > 0) //If the enemy isn't close and is not dead then...
        {
            isAwake = false;
            //animator.Play("Idle"); //Idle animation doesn't even exist for most/all enemies, this can probably be left out
        }

        if (isAwake)
        {
            enemyBase.onGround = (Physics2D.Raycast(transform.position + enemyBase.colliderOffset, Vector2.down, enemyBase.groundLength, enemyBase.groundLayer) || Physics2D.Raycast(transform.position - enemyBase.colliderOffset, Vector2.down, enemyBase.groundLength, enemyBase.groundLayer));
            //Controls where the enemy is looking
            if (enemyDetection.IsClose())
            {
                enemyBase.Flip();
            }

            //firerateWait changes based on fps time
            firerateWait -= Time.deltaTime;
            //if firerateWait is 0, time to strike and reset the wait
            if (enemyDetection.IsChasing() && !player.IsDead())
            {
                animator.SetBool("IsChasing", true);
                isChasing = true;
                enemyBase.RunTowards();
                //enemyBase.ModifyPhysics();
            }

            if (firerateWait <= 0 && enemyDetection.IsAttacking() && !player.IsDead())
            {
                animator.SetBool("IsChasing", false);
                isChasing = false;
                animator.SetBool("IsAttacking", true);
                Strike();
                //Firerate makes sense here as melee is this enemies only attack
                firerateWait = firerate;
            }
            else if(!enemyDetection.IsTooClose() || player.IsDead())
            {
                //animator.SetBool("IsChasing", false);
                animator.SetBool("IsAttacking", false);
            }

            //Makes the enemy horizontal speed lower while falling
            if (!enemyBase.onGround)
            {
                Vector2 airBrake = new Vector2(-(enemyBase.rb.velocity.x / 2), 0.0f);
                enemyBase.rb.AddForce(airBrake);
            }
        }
    }
}
