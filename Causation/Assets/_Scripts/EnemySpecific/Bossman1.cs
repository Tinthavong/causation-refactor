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
    private Vector3 gatlingCurrentLeft;
    private Vector3 gatlingCurrentRight;

    void Start()
    {
        floorHax = true;
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
            LaserShot();
            firerateWait = firerate;
        }

        if (!isInPhase)
        {
            phaseRateWait -= Time.deltaTime;
            //Minor movement
            minorMovement();
            
        }

        if (phaseRateWait <= 0)
        {
            //phase is changed in the phaseengage method
            isInPhase = true;
            phaseRateWait = phaseRate;
            
        }

        if(isInPhase)
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

        if(bulletsFired >= bulletsFiredStop)
        {
            phase += 1;
            if (phase >= 3)
            {
                phase = 0;
            }
            isInPhase = false;
            bulletsFired = 0;
            
        }

        

        ElimCharacter();//Want to find some way for elimcharacter to be checked each time damage is taken, not on every frame like it is now
    }

    private void minorMovement()
    {
        if(player.transform.position.x < transform.position.x)
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
            if(firerateWait <= 0)
            {
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
        if(gatlingCurrentLeft == gatlingStartLeft.transform.position)
        {
            gatlingCurrentLeft = gatlingStartLeft2.transform.position;
            gatlingCurrentRight = gatlingStartRight2.transform.position;
        }
        else
        {
            gatlingCurrentLeft = gatlingStartLeft.transform.position;
            gatlingCurrentRight = gatlingStartRight.transform.position;
        }
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
    }

    //Happens whenever player gets close, hopefully how ive coded it it will activate even in the middle of a phase
    private void LaserShot()
    {
        //create laser objects that destroy themselves after a short time

        //Uses strike as it accomplishes the same goal
        Strike();
        
    }

    private new void Shoot()
    {
        GameObject b = Instantiate(bulletPrefab) as GameObject;
        GameObject c = Instantiate(bulletPrefab) as GameObject;
        b.transform.position = gatlingCurrentLeft;
        c.transform.position = gatlingCurrentRight;
        b.transform.rotation = Quaternion.Euler(0.0f, 0.0f, -90f);
        b.GetComponent<Rigidbody2D>().AddForce(Vector2.left * -bulletSpeed);
        c.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 90f);
        c.GetComponent<Rigidbody2D>().AddForce(Vector2.right * -bulletSpeed);
    }
}
