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
    private int dropOnce = 1; //Quick bandaid fix for making sure items are only dropped once

    [Header("Enemy Variables")]
    //Temp Shooting behavior
    public float firerate = 2f;
    protected float firerateWait = 0f;
    public int sightRange = 10;
    public int meleeRange = 2;
    public bool floorHax = false; //Used to determine if an enemy will react to players if they arent on the same y level
    public float verticalSight = 2.5f;

    [HideInInspector]
    public bool facingRight;

    protected PlayerController player; //this can be private, pretty sure this works now
    protected Animator animator;
    protected Rigidbody2D rb;

    public float enemySpeed = 8f; //This will control how fast an enemy moves, change in prefabs for each enemy type
    public float bulletRefSpeed;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(bulletRefSpeed);
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
        if (floorHax)
        {
            return true;
        }
        else if (Math.Abs(transform.position.y - player.transform.position.y) <= verticalSight)
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
        if (animator != null)
        {
            animator.Play("Damaged");
        }

        base.DamageCalc(damage);
    }

    public override void ElimCharacter()
    {
        //this might be too simple but for now checking if the health is at or below 0 might be enough
        if (displayedHealth <= 0)
        {
            PostDeath();
            //should avoid outright destroying the characters bc it should do an animation or whatever first, should use coroutine to delay this but for now:

            Debug.Log($"Destroyed {gameObject.name}");
            gameObject.GetComponent<Animator>().SetBool("IsAttacking", false);
            gameObject.GetComponent<Animator>().SetBool("IsChasing", false);
            gameObject.GetComponent<Animator>().SetTrigger("IsDead");
            gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
            gameObject.GetComponent<Enemy>().enabled = false;
            //Destroy(gameObject); //Commented out for now, destroying the game object is too abrupt.
        }
    }

    public override void PostDeath()
    {
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
        rand = random.Next(totalweight + 1);

        //Goes through each drop in the drops list, adds its weight and checks if it passed rand
        //If it did, then that is the object that will drop on death
        if (dropOnce > 0)
        {
            dropOnce = 0;
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
    }

    void Death()
    {
        Destroy(this.gameObject);
    }
}
