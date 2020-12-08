using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinalMechBoss : Enemy
{

    //Boss notes:
    //Grounded charge phase COMPLETE
    //Airborne firing phase COMPLETE
    //Movement phase to air COMPLETE
    //Movement phase to ground COMPLETE
    //Reflection phase X

    [Header("Special Variables")]
    //Boss will go through phases one by one in a pattern: is public as testing will be easier
    public int phase = 0;
    //Phase timers to allow for transitions to new phases
    private float phaseRateWait = 0f;
    public float phaseRate = 2f;
    //Phase bool to make sure timer doesnt go down while  he is in a phase, possibly starting a new one while in a phase (which is bad)
    private bool isInPhase = true;
    //current phase timer controls how long the boss stays in its current phase
    private float currentPhaseTime;

    //Extra speed variable for charging phase
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

    //unique phase end times for each phase
    public float airPhaseEndTime = 10f;
    public float reflectPhaseEndTime = 5f;

    //Used for movement phases for exploit prevention
    private float moveTimer = 0f;
    public float moveTime = 2.5f;

    //used to determine the number of charging attacks before it changes phase
    public int chargesBeforePhaseEnds = 3;
    private int timesCharged = 0;
    private bool isCharging = false;

    //laserspawns because the boss shoots lasers, 2 for normal attacks, 2 for reflecting phase "attacks"
    public GameObject laserSpawnleft;
    public GameObject laserSpawnRight;
    public GameObject laserReflectLeft;
    public GameObject laserReflectRight;

    //This determines how high above the player the boss will fly during the airborne phase
    public float heightAbovePlayer = 13f;

    //Boss HP Bar
    public HealthBar bossHealthBar;
    public Text bossName;

    //Test variables, may need
    private BulletScript bullet;


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
        //copied from drone code to mimic ability to ignore everything, added player for ability to pass through it during charging phase
        Physics2D.IgnoreLayerCollision(gameObject.layer, 9, true);
        Physics2D.IgnoreLayerCollision(gameObject.layer, 10, true); //This causes other enemies to also ignore the player for some reason
        Physics2D.IgnoreLayerCollision(gameObject.layer, 11, true);
        Physics2D.IgnoreLayerCollision(gameObject.layer, 14, true);
        Physics2D.IgnoreLayerCollision(gameObject.layer, 15, true);
        Physics2D.IgnoreLayerCollision(gameObject.layer, 16, true);

        startingHealth = displayedHealth;
        startingLocation = gameObject.transform.localPosition;

        bulletRefSpeed = bulletPrefab.GetComponent<BulletScript>().bulletSpeed;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        player = FindObjectOfType<PlayerController>();

        //Boss HP Bar
        bossHealthBar.SetHealth(displayedHealth);
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
                        //Animator uses firing animation
                        HoverAttack();
                        MoveToAbovePlayer();
                        if (currentPhaseTime >= airPhaseEndTime)
                        {
                            isInPhase = false;
                            currentPhaseTime = 0f;
                            moveTimer = 0f;
                        }
                        break;
                    //Small phase that moves the boss for the next phase
                    case 1:
                        MoveToPlayerY();
                        break;
                    //grounded phase
                    case 2:
                        //Animator uses firing animation to signify danger
                        GroundedAttack();
                        HoverAttack();

                        if (timesCharged >= chargesBeforePhaseEnds && chargeTimer >= chargeTime)
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
                        if (moveTimer >= moveTime)
                        {
                            //Puts the boss right into the next phase
                            moveTimer = 0;
                            phaseRateWait = phaseRate;
                            isInPhase = false;
                        }
                        break;
                }
                currentPhaseTime += Time.deltaTime;
            }
            //If not in a phase, slow down
            else
            {
                AntiMovement();
                phaseRateWait += Time.deltaTime;
            }

            //Phase control
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

            Strike();
            
        }
    }

    //Braking system to prevent it from keeping its velocity when out of a phase
    private void AntiMovement()
    {
        Vector2 airBrakeX = new Vector2(-(rb.velocity.x), 0.0f);
        rb.AddForce(airBrakeX);
        Vector2 airBrakeY = new Vector2(0.0f, -(rb.velocity.y));
        rb.AddForce(airBrakeY);
    }

    //Flies at room height; fires down at the player
    private void HoverAttack()
    {
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
        //initiates a new charge if he isnt charging
        if (!isCharging && ((chargeTimer >= chargeTime + chargeCooldown) || timesCharged == 0))
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
            else if (chosenDirection == 0 || chosenDirection == 2)
            {
                //charge right
                chosenDirection = 2;
                Vector2 movement = new Vector2(groundedSpeed, 0.0f);
                rb.velocity = movement;
            }
        }
        //Slows down the boss after the charge time has been reached
        if (chargeTimer >= chargeTime && chargeTimer < chargeTime + chargeCooldown)
        {
            Vector2 airBrake = new Vector2(-(rb.velocity.x), 0.0f);
            rb.AddForce(airBrake);
        }


        chargeTimer += Time.deltaTime;
    }

    //Defensive phase, fires back at player
    private void Reflecting()
    {
        AntiMovement();

        //Instead of taking damage, whenever a bullet hits the boss, it launches another bullet back at them
        //May need to have it take damage and heal back instantly, then launch a bullet back at the player
    }

    //Moves the boss to the same y level as the player in prep for the next phase
    private void MoveToPlayerY()
    {

        if (transform.position.y < player.transform.position.y)
        {
            Vector2 movement = new Vector2(0.0f, enemySpeed);
            //Sets velocity to max if the force ever makes it go over
            if (rb.velocity.y > enemySpeed)
            {
                rb.velocity = movement;
            }
            else
            {
                rb.AddForce(movement);
            }
        }
        else if (transform.position.y > player.transform.position.y)
        {
            Vector2 movement = new Vector2(0.0f, -enemySpeed);
            //Sets velocity to max if the force ever makes it go over
            if (rb.velocity.y < -enemySpeed)
            {
                rb.velocity = movement;
            }
            else
            {
                rb.AddForce(movement);
            }
        }

        moveTimer += Time.deltaTime;
        if(moveTimer >= moveTime)
        {
            //Vector2 noMovement = new Vector2(0f, 0f);
            //rb.velocity = noMovement;
            moveTimer = 0;
            isInPhase = false;
            phaseRateWait = phaseRate / 2;
        }
    }

    //Will move the boss to a specific height above the player
    private void MoveToAbovePlayer()
    {

        //Y MOVEMENT
        if (transform.position.y < player.transform.position.y + heightAbovePlayer)
        {
            Vector2 movement = new Vector2(0.0f, enemySpeed);
            //Sets velocity to max if the force ever makes it go over
            if (rb.velocity.y > enemySpeed)
            {
                movement = new Vector2(rb.velocity.x, enemySpeed);
                rb.velocity = movement;
            }
            else
            {
                rb.AddForce(movement);
            }
        }
        else if (transform.position.y > player.transform.position.y + heightAbovePlayer)
        {
            Vector2 movement = new Vector2(0.0f, -enemySpeed);
            //Sets velocity to max if the force ever makes it go over
            if (rb.velocity.y < -enemySpeed)
            {
                movement = new Vector2(rb.velocity.x, -enemySpeed);
                rb.velocity = movement;
            }
            else
            {
                rb.AddForce(movement);
            }
        }

        //X MOVEMENT
        if (transform.position.x < player.transform.position.x)
        {
            Vector2 movement = new Vector2(enemySpeed / 1.5f, 0.0f);
            //Sets velocity to max if the force ever makes it go over
            if (rb.velocity.x > enemySpeed)
            {
                movement = new Vector2(enemySpeed / 1.5f, rb.velocity.y);
                rb.velocity = movement;
            }
            else
            {
                rb.AddForce(movement);
            }
        }
        else if (transform.position.x > player.transform.position.x)
        {
            Vector2 movement = new Vector2(-enemySpeed / 1.5f, 0.0f);
            //Sets velocity to max if the force ever makes it go over
            if (rb.velocity.x < -enemySpeed)
            {
                movement = new Vector2(-enemySpeed/ 1.5f, rb.velocity.y);
                rb.velocity = movement;
            }
            else
            {
                rb.AddForce(movement);
            }
        }

        moveTimer += Time.deltaTime;
        
    }

    //Used to deal damage while the boss sweeps the floor
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

    public void Reflect()
    {
        GameObject b = Instantiate(bulletPrefab) as GameObject;
        b.transform.position = laserReflectLeft.transform.position;
        GameObject c = Instantiate(bulletPrefab) as GameObject;
        c.transform.position = laserReflectRight.transform.position;

        b.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        b.GetComponent<Rigidbody2D>().AddForce(Vector2.left * bulletPrefab.GetComponent<BulletScript>().bulletSpeed);
        c.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 180f);
        c.GetComponent<Rigidbody2D>().AddForce(Vector2.right * bulletPrefab.GetComponent<BulletScript>().bulletSpeed);
    }

    //Boss HP Bar
    public override void DamageCalc(int damage)
    {
        bossHealthBar.SetHealth(displayedHealth);  //This throws errors since bossHealthBar is not set
        base.DamageCalc(damage);
    }

    public new void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Projectile":
                bullet = collision.GetComponent<BulletScript>();
                if(phase != 3)
                {
                    DamageCalc(bullet.GetDamage());
                    FindObjectOfType<SFXManager>().PlayAudio("Damage");
                }
                else
                {
                    Reflect();
                }
                break;
        }

        
    }

    //WHEN THIS DIES: set the gravity to something so it will hit the floor and explode

}
