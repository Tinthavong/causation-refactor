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
    public bool floorHax = false; //Used to determine if an enemy will react to players if they arent on the same y level
    public float verticalSight = 2.5f;
    
    protected bool facingRight;
    protected PlayerController player; //this can be private, pretty sure this works now
    protected Animator animator;
    protected Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        //This should use an actual find method/algorithm instead of just knowing where the player is
    }

    // Update is called once per frame
    void Update()
    {

    }

    //isClose and isTooClose are specific to gunslinger enemies, at least currently

    //Checks to see if the player object is within a certain distance
    public bool isClose()//shouldve made these public long time ago, but they are now
    {
        if (Math.Abs(player.transform.position.x - this.gameObject.transform.position.x) < sightRange && player.displayedHealth > 0) //Dirty fix. Stop, he's already dead!
        {
            return true;
        }
        return false;
    }

    //Used for melee support
    public bool isTooClose()//shouldve made these public long time ago, but they are now
    {
        if (Math.Abs(player.transform.position.x - this.gameObject.transform.position.x) < meleeRange && player.displayedHealth > 0)
        {
            return true;
        }
        return false;
    }

    public bool vertRangeSeesPlayer()
    {
        if(floorHax)
        {
            return true;
        }
        else if(Math.Abs(transform.position.y - player.transform.position.y) <= verticalSight)
        {
            return true;
        }
        return false;
    }

    //Controls where the enemy is looking, always towards player
    public override void Flip(float dump) //dump because it doesn't matter but it's needed or errors
    {
        if (player.transform.position.x < this.transform.position.x && !facingRight)
        {
            facingRight = true;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            colliderOffset.x *= -1;

            transform.localScale = scale;
        }
        else if (player.transform.position.x >= this.transform.position.x && facingRight)
        {
            facingRight = false;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            colliderOffset.x *= -1;

            transform.localScale = scale;
        }
    }

    public override void DamageCalc(int damage)
    {
        if(animator != null)
        {
            animator.Play("Damaged");
        }
        
        base.DamageCalc(damage);
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
        //+1 because .next returns a random int less than the provided number
        rand = random.Next(totalweight+1);

        //Goes through each drop in the drops list, adds its weight and checks if it passed rand
        //If it did, then that is the object that will drop on death
        foreach (EnemyDrops dr in drops)
        {
            finder += dr.weight;
            if (finder >= rand)
            {
                if (dr.drop == null)
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
