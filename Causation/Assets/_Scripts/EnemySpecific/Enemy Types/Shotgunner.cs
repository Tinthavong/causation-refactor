using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgunner : EnemyBaseCombatController
{
    /*
    EnemyBase enemyBase;
    EnemyBaseStats enemyBaseStats;
    EnemyDetection enemyDetection;
  */
    [Header("Special variables")]
    //Range before shotgunsplosion happens
    //New bulletspawns for extra bullets
    public GameObject bulletSpawn2;
    public GameObject bulletSpawn3;

    /*
   // Start is called before the first frame update
   void Start()
   {

       enemyBase = GetComponent<EnemyBase>();
       enemyBaseStats = GetComponent<EnemyBaseStats>();
       enemyDetection = GetComponent<EnemyDetection>();
       animator = GetComponent<Animator>();

       player = FindObjectOfType<PlayerStateManager>();
       
}
    */
    // Update is called once per frame
    void Update()
    {
        if (isAwake)
        {
            enemyBase.onGround = (Physics2D.Raycast(transform.position + enemyBase.colliderOffset, Vector2.down, enemyBase.groundLength, enemyBase.groundLayer) || Physics2D.Raycast(transform.position - enemyBase.colliderOffset, Vector2.down, enemyBase.groundLength, enemyBase.groundLayer));
            //Controls where the enemy is looking
            if (enemyDetection.IsClose())
            {
                enemyBase.Flip();
                if (enemyDetection.VertRangeSeesPlayer())
                {
                    animator.SetBool("IsChasing", true);
                    isChasing = true;
                    if (Math.Abs(player.transform.position.x - this.gameObject.transform.position.x) >= enemyDetection.fireRange)
                    {
                        enemyBase.RunTowards();
                    }
                }
            }

            //firerateWait changes based on fps time
            firerateWait -= Time.deltaTime;
            //if firerateWait is 0, time to strike and reset the wait
            if (enemyDetection.IsChasing() && !player.IsDead())
            {
                animator.SetBool("IsChasing", true);
                isChasing = true;
                enemyBase.RunTowards();
            }

            if (firerateWait <= 0 && enemyDetection.IsCloseEnough() && enemyDetection.VertRangeSeesPlayer() && !player.IsDead())
            {
                animator.SetBool("IsChasing", false);
                isChasing = false;
                animator.Play("Attack");
                ShotgunShot();
                //Firerate makes sense here as melee is this enemies only attack
                firerateWait = firerate;
            }

            if (!enemyDetection.IsClose() || player.IsDead())
            {
                animator.SetBool("IsChasing", false);
            }

            //Makes the enemy horizontal speed lower while falling
            if (!enemyBase.onGround)
            {
                Vector2 airBrake = new Vector2(-(enemyBase.rb.velocity.x / 2), 0.0f);
                enemyBase.rb.AddForce(airBrake);
            }
        }
    }

    //Shotgunner needs a new shoot method as it shoots in more than one direction, this method makes 3 bullets, two at an angle
    //b shoots straight, c shoots above, d shoots below
    private void ShotgunShot()
    {
        //FindObjectOfType<SFXManager>().PlayAudio("Gunshot");

        GameObject b = Instantiate(bulletPrefab) as GameObject;
        GameObject c = Instantiate(bulletPrefab) as GameObject;
        GameObject d = Instantiate(bulletPrefab) as GameObject;
        b.transform.position = bulletSpawn.transform.position;
        c.transform.position = bulletSpawn2.transform.position;
        d.transform.position = bulletSpawn3.transform.position;
        //Bullet object shifts position and rotation based on direction
        if (gameObject.transform.localScale.x < 0)
        {
            b.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 90f);
            b.GetComponent<Rigidbody2D>().AddForce(Vector2.left * bulletPrefabSpeed);
            c.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 80f);
            c.GetComponent<Rigidbody2D>().AddForce(Vector2.left * bulletPrefabSpeed);
            c.GetComponent<Rigidbody2D>().AddForce(Vector2.up * (bulletPrefabSpeed / 6));
            d.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 100f);
            d.GetComponent<Rigidbody2D>().AddForce(Vector2.left * bulletPrefabSpeed);
            d.GetComponent<Rigidbody2D>().AddForce(Vector2.down * (bulletPrefabSpeed / 6));
        }
        else
        {
            b.transform.rotation = Quaternion.Euler(0.0f, 0.0f, -90f);
            b.GetComponent<Rigidbody2D>().AddForce(Vector2.right * bulletPrefabSpeed);
            c.transform.rotation = Quaternion.Euler(0.0f, 0.0f, -80f);
            c.GetComponent<Rigidbody2D>().AddForce(Vector2.right * bulletPrefabSpeed);
            c.GetComponent<Rigidbody2D>().AddForce(Vector2.up * (bulletPrefabSpeed / 6));
            d.transform.rotation = Quaternion.Euler(0.0f, 0.0f, -100f);
            d.GetComponent<Rigidbody2D>().AddForce(Vector2.right * bulletPrefabSpeed);
            d.GetComponent<Rigidbody2D>().AddForce(Vector2.down * (bulletPrefabSpeed / 6));
        }
    }
}
