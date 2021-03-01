using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Bossman1 : EnemyBaseCombatController
{
    //Special Notes:
    //This boss will not move, it will be stationary with the ability to shoot both ways
    //This boss has 3 phases, high gatling, low gatling, and an alternating fire phase

    [Header("Special Variables")]
    //Boss will go through phases one by one in a pattern: is public as testing will be easier
    public int phase = 0;
    //New phase timers to allow for transitions to new phases, allowing damage to be done in between phases
    private float phaseRateWait = 0f;
    public float phaseRate = 4f;
    //Phase bool to make sure timer doesnt go down while  he is in a phase, possibly starting a new one while in a phase (which is bad)
    private bool isInPhase = false;
    //Controls amount of bullets fired while in a phase
    private int bulletsFired = 0;
    public int bulletsFiredStop = 8;
    private float bossBulletSpeed;

    //Two variables used to determine the bullet starts for the gatlings on both sides.  Each side fires simultaneously.
    public GameObject gatlingStartLeft;
    public GameObject gatlingStartRight;
    public GameObject gatlingStartLeft2;
    public GameObject gatlingStartRight2;
    public GameObject laserStartLeft;
    public GameObject laserStartRight;
    public GameObject laserBeam;
    private Vector3 gatlingCurrentLeft;
    private Vector3 gatlingCurrentRight;

  
    //public Text bossName;
    Rigidbody2D rb;

    public override void Start()
    {
        //Boss HP Bar

        bossBulletSpeed = bulletPrefab.GetComponent<ProjectileProperties>().projectileSpeed;
        enemyBase = GetComponent<EnemyBaseMovement>();
        enemyBaseStats = GetComponent<EnemyBaseStats>();
        enemyDetection = GetComponent<EnemyDetection>();
        animator = GetComponent<Animator>();
        player = FindObjectOfType<PlayerStateManager>();
        rb = GetComponent<Rigidbody2D>();

        //Gets initial position for the bullets
        gatlingCurrentLeft = gatlingStartLeft.transform.position;
        gatlingCurrentRight = gatlingStartRight.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (isAwake)
        {
            firerateWait -= Time.deltaTime;

            //This fires regardless of the phase, bad player no get close
            if (firerateWait <= 0 && enemyDetection.IsTooClose())
            {
                animator.SetBool("IsMoving", false);
                animator.SetBool("IsLasering", true);
                LaserShot();
                firerateWait = firerate;
            }

            if (!isInPhase)
            {
                phaseRateWait -= Time.deltaTime;
                //Minor movement
                animator.SetBool("IsLasering", false);
                animator.SetBool("IsShooting", false);
                animator.SetBool("IsMoving", true);
                MinorMovement();
            }

            if (phaseRateWait <= 0)
            {
                //phase is changed when a bullets fired threshold is met
                isInPhase = true;
                phaseRateWait = phaseRate;
            }

            if (isInPhase)
            {
                switch (phase)
                {
                    case 0:
                        ChangeGatlingHeight(1);
                        GatlingStream();
                        break;
                    case 1:
                        ChangeGatlingHeight(2);
                        GatlingStream();
                        break;
                    case 2:
                        AlternatingStream();
                        break;
                }
            }

            if (bulletsFired >= bulletsFiredStop)
            {
                phase += 1;
                if (phase >= 3)
                {
                    phase = 0;
                }
                isInPhase = false;
                bulletsFired = 0;
            }
        }
    }

    private void MinorMovement()
    {
        if (player.transform.position.x < transform.position.x)
        {
            Vector2 movement = new Vector2(-enemyBase.enemySpeed, 0.0f);
            rb.MovePosition(rb.position + movement * Time.fixedDeltaTime);
        }
        else
        {
            Vector2 movement = new Vector2(enemyBase.enemySpeed, 0.0f);
            rb.MovePosition(rb.position + movement * Time.fixedDeltaTime);
        }
    }

    //Shoots a stream of bullets on the level that the guns are on
    private void GatlingStream()
    {
        //Firerate needs to be set really low for this to work well:  will shoot predetermined amount of bullets then end the phase
        //while(bulletsFired < bulletsFiredStop)
        //{
        if (firerateWait <= 0)
        {
            animator.SetBool("IsMoving", false);
            animator.SetBool("IsShooting", true);
            Shoot();
            bulletsFired += 1;
            firerateWait = firerate;
        }

        //}
    }

    //switches back and forth between high and low shots for a bit
    private void AlternatingStream()
    {
        //Firerate will be psuedo controlled by the animations ends for switching heights
        if (firerateWait <= 0)
        {
            Shoot();
            bulletsFired += 1;
            firerateWait = firerate;
            ChangeGatlingHeight();
        }
    }

    //just switches from high to low or vice versa
    private void ChangeGatlingHeight()
    {
        //basically just the animation as well as moving the gatlingStart's
        if (gatlingCurrentLeft == gatlingStartLeft.transform.position)
        {
            gatlingCurrentLeft = gatlingStartLeft2.transform.position;
            gatlingCurrentRight = gatlingStartRight2.transform.position;
        }
        else
        {
            gatlingCurrentLeft = gatlingStartLeft.transform.position;
            gatlingCurrentRight = gatlingStartRight.transform.position;
        }

        //Need to change the location of the gatlings during this method, as well as animation for the change

    }
    private void ChangeGatlingHeight(int pos)
    {
        //basically just the animation as well as moving the gatlingStart's
        if (pos == 1)
        {
            gatlingCurrentLeft = gatlingStartLeft.transform.position;
            gatlingCurrentRight = gatlingStartRight.transform.position;
        }
        else if (pos == 2)
        {
            gatlingCurrentLeft = gatlingStartLeft2.transform.position;
            gatlingCurrentRight = gatlingStartRight2.transform.position;
        }

        //Need to change the location of the gatlings during this method, as well as animation for the change

    }

    //Happens whenever player gets close, hopefully how ive coded it it will activate even in the middle of a phase
    private void LaserShot()
    {
        //create laser objects that destroy themselves after a short time

        //Might keep strike here as it will ensure damage gets dealt
        //animator.Play("Lasers");
        Strike();

        //I need to make use of the laser art ive been given for this boss

        GameObject b = Instantiate(laserBeam) as GameObject;
        GameObject c = Instantiate(laserBeam) as GameObject;
        b.transform.position = laserStartLeft.transform.position;
        c.transform.position = laserStartRight.transform.position;
        b.transform.rotation = Quaternion.Euler(0.0f, 0.0f, -55f);
        b.GetComponent<Rigidbody2D>().AddForce(Vector2.right * bossBulletSpeed);
        b.GetComponent<Rigidbody2D>().AddForce(Vector2.down * bossBulletSpeed * 1.3f);
        c.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 55f);
        c.GetComponent<Rigidbody2D>().AddForce(Vector2.left * bossBulletSpeed);
        c.GetComponent<Rigidbody2D>().AddForce(Vector2.down * bossBulletSpeed * 1.3f);

    }

    private new void Shoot()
    {
        GameObject b = Instantiate(bulletPrefab) as GameObject;
        GameObject c = Instantiate(bulletPrefab) as GameObject;
        b.transform.position = gatlingCurrentLeft;
        c.transform.position = gatlingCurrentRight;
        b.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0f);
        b.GetComponent<Rigidbody2D>().AddForce(Vector2.left * -bossBulletSpeed);
        c.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0f);
        c.GetComponent<Rigidbody2D>().AddForce(Vector2.right * -bossBulletSpeed);
    }

    //Special strike for boss to literally just not have it constantly play the melee sound
    public override void Strike() //Melee attack
    {
        //Enemies in range of attack here
        Collider2D[] hitBoxPlayer = Physics2D.OverlapCircleAll(StrikeZone.position, strikeRange, enemyLayers);

        //Damage calculations
        foreach (Collider2D player in hitBoxPlayer)
        {
            player.GetComponent<PlayerStateManager>().DamageCalculation(strikeDamage);
        }
    }

    /*
    //Boss HP Bar
    public override void DamageCalc(int damage)
    {

        bossHealthBar.UpdateHealthBar(displayedHealth);
        base.DamageCalc(damage);
    }
    */
}
