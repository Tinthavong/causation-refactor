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

    void Start()
    {
        bulletRefSpeed = bulletPrefab.GetComponent<BulletScript>().bulletSpeed;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        player = FindObjectOfType<PlayerController>();
        //Gets initial position for the bullets
        gatlingCurrentLeft = gatlingStartLeft.transform.position;
        gatlingCurrentRight = gatlingStartRight.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        firerateWait -= Time.deltaTime;

        //This fires regardless of the phase, bad player no get close
        if (firerateWait <= 0 && isTooClose())
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
            minorMovement();

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
                    changeGatlingHeight(1);
                    gatlingStream();
                    break;
                case 1:
                    changeGatlingHeight(2);
                    gatlingStream();
                    break;
                case 2:
                    alternatingStream();
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

    //Overridden elimcharacter method to account for unique animation names - can be standardized to allow for inheritance
    public override void ElimCharacter()
    {
        //this might be too simple but for now checking if the health is at or below 0 might be enough
        if (displayedHealth <= 0)
        {
            PostDeath(); //might override and display the victory screen instead
            //should avoid outright destroying the characters bc it should do an animation or whatever first, should use coroutine to delay this but for now:

            gameObject.GetComponent<Animator>().SetBool("IsLasering", false);
            gameObject.GetComponent<Animator>().SetBool("IsShooting", false);
            gameObject.GetComponent<Animator>().SetBool("IsMoving", false);
            gameObject.GetComponent<Animator>().SetTrigger("IsDead");
            gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            gameObject.GetComponent<Enemy>().enabled = false;
            //Destroy(gameObject); //Commented out for now, destroying the game object is too abrupt.
        }
    }

    public override void PostDeath()
    {
        PlayerController pc = FindObjectOfType<PlayerController>();
        pc.LM.VictoryCheck();
    }

    private void minorMovement()
    {
        if (player.transform.position.x < transform.position.x)
        {
            Vector2 movement = new Vector2(-enemySpeed, 0.0f);
            rb.MovePosition(rb.position + movement * Time.fixedDeltaTime);
        }
        else
        {
            Vector2 movement = new Vector2(enemySpeed, 0.0f);
            rb.MovePosition(rb.position + movement * Time.fixedDeltaTime);
        }
    }

    //Shoots a stream of bullets on the level that the guns are on
    private void gatlingStream()
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
    private void alternatingStream()
    {
        //Firerate will be psuedo controlled by the animations ends for switching heights
        if (firerateWait <= 0)
        {
            Shoot();
            bulletsFired += 1;
            firerateWait = firerate;
            changeGatlingHeight();
        }
    }

    //just switches from high to low or vice versa
    private void changeGatlingHeight()
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
    private void changeGatlingHeight(int pos)
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
        b.GetComponent<Rigidbody2D>().AddForce(Vector2.right * bulletRefSpeed);
        b.GetComponent<Rigidbody2D>().AddForce(Vector2.down * bulletRefSpeed * 1.3f);
        c.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 55f);
        c.GetComponent<Rigidbody2D>().AddForce(Vector2.left * bulletRefSpeed);
        c.GetComponent<Rigidbody2D>().AddForce(Vector2.down * bulletRefSpeed * 1.3f);

    }

    private new void Shoot()
    {
        GameObject b = Instantiate(bulletPrefab) as GameObject;
        GameObject c = Instantiate(bulletPrefab) as GameObject;
        b.transform.position = gatlingCurrentLeft;
        c.transform.position = gatlingCurrentRight;
        b.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0f);
        b.GetComponent<Rigidbody2D>().AddForce(Vector2.left * -bulletRefSpeed);
        c.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0f);
        c.GetComponent<Rigidbody2D>().AddForce(Vector2.right * -bulletRefSpeed);
    }

    //Special strike for boss to literally just not have it constantly play the melee sound
    public override void Strike() //Melee attack
    {
        //Enemies in range of attack here
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(StrikeZone.position, strikeRange, enemyLayers);

        //Damage calculations
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<CharacterBase>().DamageCalc(strikeDamage);
            Debug.Log($"{gameObject.name} hit {enemy.name}");
        }
    }
}
