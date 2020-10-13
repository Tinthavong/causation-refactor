using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : CharacterBase
{
    //Let Trumpie know before making changes
    public Enemy() //constructor
    {
        Health = displayedHealth; //Displayed Health can be set in the inspector
        Stamina = staminaActions;
        //Enemies dont use ammo for now but if it breaks just set the amount here
        Currency = dropValue;   
    }

    //This internal class controls each droppable item set in the inspector (Set in prefabs unless it's a special enemy)
    [Serializable]
    public class EnemyDrops
    {
        public GameObject drop;
        public int weight;
        public EnemyDrops(GameObject d, int w)
        {
            drop = d;
            weight = w;
        }
    }
    
    [Header("Enemy Drops")]
    //Consider making these private and serialized
    public int dropValue;
    //List of EnemyDrops (explained above) to allow for multiple drops
    public List<EnemyDrops> drops;

    [Header("Enemy Variables")]
    //Temp Shooting behavior
    public float firerate = 2f;
    protected float firerateWait = 0f;
    public int sightRange = 10;
    public int meleeRange = 2;

    protected bool facingRight;
    protected PlayerController player; //this can be private, pretty sure this works now

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        //This should use an actual find method/algorithm instead of just knowing where the player is
    }

    // Update is called once per frame
    void Update()
    {
        //Controls where the enemy is looking
        //First in update to make sure bullet travels in correct direction
        if (isClose())
        {
            Flip(0f);
        }

        //firerateWait changes based on fps time
        firerateWait -= Time.deltaTime;
        //if firerateWait is 0, time to fire and reset the wait
        if (firerateWait <= 0 && isClose() && !isTooClose())
        {
            Shoot();
            firerateWait = firerate;
        }

        if(firerateWait <= 0 && isTooClose())
        {
            //animation play here
            Strike();
            //Using firerate as the buffer for melee for consistency, might replace later
            firerateWait = firerate;
        }


        ElimCharacter();//Want to find some way for elimcharacter to be checked each time damage is taken, not on every frame like it is now
        
        
    }

    //isClose and isTooClose are specific to gunslinger enemies, at least currently

    //Checks to see if the player object is within a certain distance
    private bool isClose()
    {
        if(Math.Abs(player.transform.position.x - this.gameObject.transform.position.x) < sightRange)
        {
            return true;
        }
        return false;
    }

    //Unused currently, will be implemented with melee support
    private bool isTooClose()
    {
        if (Math.Abs(player.transform.position.x - this.gameObject.transform.position.x) < meleeRange)
        {
            return true;
        }
        return false;
    }

    public override void Flip(float dump) //dump because it doesn't matter but it's needed or errors
    {
        if (player.transform.position.x < this.transform.position.x && !facingRight)
        {
            facingRight = true;
            Vector3 scale = transform.localScale;
            scale.x *= -1;

            transform.localScale = scale;
        }
        else if (player.transform.position.x >= this.transform.position.x && facingRight)
        {
            facingRight = false;
            Vector3 scale = transform.localScale;
            scale.x *= -1;

            transform.localScale = scale;
        }
    }

    public override void PostDeath()
    {
        //Temporary death, needs animation and drops
        //GameObject d = Instantiate(drop) as GameObject;
        // d.transform.position = this.transform.position;

        //Variables for the dynamic drop search
        GameObject drop;
        int totalweight = 0;
        int rand;
        int finder = 0;
        System.Random random = new System.Random();

        //Adds up total weight for use in RNG
        foreach (EnemyDrops dr in drops)
        {
            totalweight += dr.weight;
        }
        rand = random.Next(totalweight);
        
        //Goes through each drop in the drops list, adds its weight and checks if it passed rand
        //If it did, then that is the object that will drop on death
        foreach(EnemyDrops dr in drops)
        {
            finder += dr.weight;
            if(finder >= rand)
            {
                if(dr.drop == null)
                {
                    //If the drop chosen happens to be empty, this keeps it from exploding the game
                    break;
                }
                drop = Instantiate(dr.drop) as GameObject;
                drop.transform.position = this.transform.position;
                break;
            }
        }
    }

    void Death()
    {

        Destroy(this.gameObject);
    }
}
