using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalMechBoss : Enemy
{

    //Boss notes preproduction:
    //Will be a horizontally moving boss, possibly with airborne phases that it cant be touched during (since it will not be able to be shot)
    //Will have a winged melee charge attack, laser shots from the sky, laser shots horizontally (when on ground level), drop attack
    //Phases include: Airborne, Grounded, Reflecting, Charging grounded (possible)

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
    public float groundedSpeed = 12f;

    //These variables control how long before each strike, to prevent the player from taking infinite damage when charged
    private float strikeTimer = 0f;
    public float strikeCooldown = 1f;

    //unique phase end times for each phase in case we want certain ones to last longer
    public float airPhaseEndTime;
    public float groundPhaseEndTime;
    public float reflectPhaseEndTime;
    //possible use, would be used to determine the number of charging attacks before it gets "tired"
    public float chargesBeforePhaseEnds;

    //laserspawns as the boss shoots lasers
    public GameObject laserSpawnleft;
    public GameObject laserSpawnRight;

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
            if(isInPhase)
            {
                //This switch is where the boss will do things
                switch (phase)
                {
                    //airborne phase
                    case 0:
                        //Animator uses movement animation
                        HoverAttack();
                        break;
                    //grounded phase
                    case 1:
                        //Animator uses general movement animation
                        GroundedAttack();
                        break;
                    //reflecting phase
                    case 2:
                        //Animator uses shielding animation
                        Reflecting();
                        break;
                }
                currentPhaseTime += Time.deltaTime;

                //This switch will check to see if it should end the phase based on the end time of the phase
                switch(phase)
                {
                    //Airborne phase
                    case 0:
                        if(currentPhaseTime >= airPhaseEndTime)
                        {
                            isInPhase = false;
                            currentPhaseTime = 0f;
                        }
                        break;
                    //Grounded phase
                    case 1:
                        if (currentPhaseTime >= groundPhaseEndTime)
                        {
                            isInPhase = false;
                            currentPhaseTime = 0f;
                        }
                        break;
                    //Reflecting phase
                    case 2:
                        if (currentPhaseTime >= reflectPhaseEndTime)
                        {
                            isInPhase = false;
                            currentPhaseTime = 0f;
                        }
                        break;
                }
            }
            else
            {
                Movement(enemySpeed);
                phaseRateWait += Time.deltaTime;
            }

            if(phaseRateWait >= phaseRate)
            {
                isInPhase = true;
                phase += 1;
                if(phase > 2)
                {
                    phase = 0;
                }
                phaseRateWait = 0f;
            }

            strikeTimer += Time.deltaTime;
        }
    }

    //Basic back and forth movement, speed changes based on the phase
    private void Movement(float moveSpeed)
    {

    }

    //Flies at room height; fires down at the player
    private void HoverAttack()
    {
        Movement(enemySpeed);
    }

    //Idea is he ignores collision with platforms, sweeping side to side dealing melee damage to the player
    private void GroundedAttack()
    {
        Movement(groundedSpeed);
        if(IsTooClose() && strikeTimer >= strikeCooldown)
        {
            Strike();
        }
    }

    //Defensive phase, fires back at player
    private void Reflecting()
    {
        
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
    
}
