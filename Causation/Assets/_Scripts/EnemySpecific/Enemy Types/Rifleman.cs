using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifleman : EnemyBaseCombatController
{
    public bool isCrouched;

    /*
    EnemyBase enemyBase;
    EnemyBaseStats enemyBaseStats;
    EnemyDetection enemyDetection;
    

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

            //if firerateWait is 0, time to fire and reset the wait
            if (firerateWait <= 0 && enemyDetection.IsShooting() && !player.IsDead())
            {
                animator.Play("Attack");
                Shoot();
                //Firerate makes sense here as melee is this enemies only attack
                firerateWait = firerate;

                //                FindObjectOfType<SFXManager>().PlayAudio("Gunshot");
                //Might have to revamp the audio... or not
                CrouchUp();

            }

            if (!isCrouched && firerateWait < (firerate / 2))
            {
                Crouching();
            }
        }
    }

    private void Crouching()
    {
        animator.SetBool("IsCrouched", true);
        isCrouched = true;
        if (isCrouched)
        {
            GetComponent<CapsuleCollider2D>().size = new Vector2(1f, 1.55f);
            GetComponent<CapsuleCollider2D>().offset = new Vector2(0f, -0.5f);
        }
        Invoke("CrouchUp", 2f);
    }

    private void CrouchUp()
    {
        animator.SetBool("IsCrouched", false);
        isCrouched = false;
        if (!isCrouched)
        {
            GetComponent<CapsuleCollider2D>().size = new Vector2(1f, 2.4f);
            GetComponent<CapsuleCollider2D>().offset = new Vector2(0f, 0f);
        }
    }
}
