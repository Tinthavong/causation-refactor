using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalMechBoss : Enemy
{

    //Boss notes:
    //Grounded charge phase COMPLETE
    //Airborne firing phase X
    //Movement phase to air X
    //Movement phase to ground X
    //Reflection phase X

    [Header("Special Variables")]
    //Boss will go through phases one by one in a pattern: is public as testing will be easier
    public int phase = 0;
    //Phase timers to allow for transitions to new phases
    private float phaseRateWait = 0f;
    public float phaseRate = 4f;
    //Phase bool to make sure timer doesnt go down while  he is in a phase, possibly starting a new one while in a phase (which is bad)
    private bool isInPhase = false;
    //current phase timer controls how long the boss stays in its current phase
    private float currentPhaseTime;

    //New speed variable for grounded charging phase
    public float groundedSpeed = 15f;

    //These variables control how long before each strike, to prevent the player from taking infinite damage when charged
    private float strikeTimer = 0f;
    public float strikeCooldown = 1f;

    //Controls how often the boss charges in grounded phase
    private float chargeTimer = 0f;
    public float chargeCooldown = 1f;
    //This determines how far each charge will go during the grounded phase
    public float chargeTime = 1f;
    private int chosenDirection = 0;

    //unique phase end times for each phase in case we want certain ones to last longer
    public float airPhaseEndTime = 6f;
    public float reflectPhaseEndTime = 4f;

    //used to determine the number of charging attacks before it changes phase
    public int chargesBeforePhaseEnds = 3;
    private int timesCharged = 0;
    private bool isCharging = false;

    //laserspawns as the boss shoots lasers
    public GameObject laserSpawnleft;
    public GameObject laserSpawnRight;
    public GameObject laserReflectLeft;
    public GameObject laserReflectRight;

    //This determines how high above the player the boss will fly during the airborne phase
    public float heightAbovePlayer = 8f;


    public FinalMechBoss() //constructor
    {
        Health = displayedHealth; //Displayed Health can be set in the inspector
        Stamina = staminaActions;
        //Enemies dont use ammo for now but if it breaks just set the amount here
        Currency = dropValue;
    }

    // Start is called before the first frame update
    void Start()
    {
        bulletRefSpeed = bulletPrefab.GetComponent<BulletScript>().bulletSpeed;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        player = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isAwake)
        {
            //Testing code, needs to be deleted
            /*if(firerateWait <= 0)
            {
                Shoot();
                firerateWait = firerate;
            }
            firerateWait -= Time.deltaTime;
            */

            if(isInPhase)
            {
                //This switch is where the boss will do things
                switch (phase)
                {
                    //airborne phase
                    case 0:
                        //Animator uses movement animation
                        HoverAttack();

                        if (currentPhaseTime >= airPhaseEndTime)
                        {
                            isInPhase = false;
                            currentPhaseTime = 0f;
                        }
                        break;
                    //Small phase that moves the boss for the next phase
                    case 1:
                        MoveToPlayerY();
                        break;
                    //grounded phase
                    case 2:
                        //Animator uses general movement animation
                        GroundedAttack();

                        if (timesCharged >= chargesBeforePhaseEnds)
                        {
                            isInPhase = false;
                            currentPhaseTime = 0f;
                            timesCharged = 0;
                        }
                        break;
                    //reflecting phase
                    case 3:
                        //Animator uses shielding animation
                        Reflecting();

                        if (currentPhaseTime >= reflectPhaseEndTime)
                        {
                            isInPhase = false;
                            currentPhaseTime = 0f;
                        }
                        break;
                    //small phase to move the boss back into the air
                    case 4:
                        MoveToAbovePlayer();
                        break;
                }
                currentPhaseTime += Time.deltaTime;
            }
            //If not in a phase, do normal movement
            else
            {
                Movement();
                phaseRateWait += Time.deltaTime;
            }

            if(phaseRateWait >= phaseRate)
            {
                isInPhase = true;
                phase += 1;
                if(phase > 4)
                {
                    phase = 0;
                }
                phaseRateWait = 0f;
            }

            strikeTimer += Time.deltaTime;
            
        }
    }

    //Basic movement, speed changes based on the phase
    private void Movement()
    {
        //Set chosenDirection and add force to velocity for a bit then change direction, all while firing
    }

    //Flies at room height; fires down at the player
    private void HoverAttack()
    {
        Movement();
        if (firerateWait <= 0)
        {
            Shoot();
            firerateWait = firerate;
        }
        firerateWait -= Time.deltaTime;
    }

    //Idea is he ignores collision with platforms, sweeping side to side dealing melee damage to the player
    private void GroundedAttack()
    {
        Charge();

        if(IsTooClose() && strikeTimer >= strikeCooldown)
        {
            Strike();
            strikeTimer = 0;
        }
    }

    //This code makes the boss charge for a certain time, then come to a stop for another charge/phase
    private void Charge()
    {
        //initiates a new charge if he isnt charging
        if(!isCharging && ((chargeTimer >= chargeTime + chargeCooldown) || timesCharged == 0))
        {
            isCharging = true;
            timesCharged += 1;
            chargeTimer = 0;
        }
        //deactivates the charge variables if his charge has reached max charge time
        if (chargeTimer >= chargeTime + chargeCooldown)
        {
            isCharging = false;
            chosenDirection = 0;
        }
        if (isCharging && chargeTimer < chargeTime)
        {
            if (player.transform.position.x < transform.position.x && (chosenDirection == 0 || chosenDirection == 1))
            {
                //charge left
                chosenDirection = 1;
                Vector2 movement = new Vector2(-groundedSpeed, 0.0f);
                rb.velocity = movement;
            }
            else if(chosenDirection == 0 || chosenDirection == 2)
            {
                //charge right
                chosenDirection = 2;
                Vector2 movement = new Vector2(groundedSpeed, 0.0f);
                rb.velocity = movement;
            }
        }
        //Slows down the boss after the charge time has been reached
        if(chargeTimer >= chargeTime && chargeTimer < chargeTime + chargeCooldown)
        {
            Vector2 airBrake = new Vector2(-(rb.velocity.x), 0.0f);
            rb.AddForce(airBrake);
        }
        

        chargeTimer += Time.deltaTime;
    }

    //Defensive phase, fires back at player
    private void Reflecting()
    {
        //Instead of taking damage, whenever a bullet hits the boss, it launches another bullet back at them
        //May need to have it take damage and heal back instantly, then launch a bullet back at the player
    }

    //Will move the boss to the same y level as the player
    private void MoveToPlayerY()
    {
        //Add force to the boss until it gets to player y
        //When boss y is below player y, stop the movement and change phase
    }

    //Will move the boss to a specific height above the player
    private void MoveToAbovePlayer()
    {
        //Add force to boss until it is above player
        //When boss y is above player y + heightAbovePlayer, stop movement and begin next phase
    }

    //Will be used to deal damage while the boss sweeps the floor
    //Overridden to have the strike sound only happen when it deals damage
    //Want to make it so it strikes right when the player is in range, then it has a cooldown
    public override void Strike()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(StrikeZone.position, strikeRange, enemyLayers);

        //Damage calculations
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<CharacterBase>().DamageCalc(strikeDamage);
            Debug.Log($"{gameObject.name} hit {enemy.name}");
            FindObjectOfType<SFXManager>().PlayAudio("Melee");
        }
    }

    //Overridden shoot method as the boss shoots downwards
    public override void Shoot()
    {
        GameObject b = Instantiate(bulletPrefab) as GameObject;
        b.transform.position = laserSpawnleft.transform.position;
        GameObject c = Instantiate(bulletPrefab) as GameObject;
        c.transform.position = laserSpawnRight.transform.position;

        b.transform.rotation = Quaternion.Euler(0.0f, 0.0f, -90f);
        b.GetComponent<Rigidbody2D>().AddForce(Vector2.down * bulletPrefab.GetComponent<BulletScript>().bulletSpeed);
        c.transform.rotation = Quaternion.Euler(0.0f, 0.0f, -90f);
        c.GetComponent<Rigidbody2D>().AddForce(Vector2.down * bulletPrefab.GetComponent<BulletScript>().bulletSpeed);
    }


    //WHEN THIS DIES: set the gravity to something so it will hit the floor and explode

}
