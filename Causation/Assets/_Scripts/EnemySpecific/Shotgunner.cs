using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Shotgunner : Enemy
{
    
    public Shotgunner() //constructor
    {
        Health = displayedHealth; //Displayed Health can be set in the inspector
        Stamina = staminaActions;
        //Enemies dont use ammo for now but if it breaks just set the amount here
        Currency = dropValue;
    }

    [Header("Special variables")]
    //Range before shotgunsplosion happens
    public int fireRange = 5;
    //New bulletspawns for extra bullets
    public GameObject bulletSpawn2;
    public GameObject bulletSpawn3;

    //private float bulletRefSpeed; //Bullet reference speed
    // Start is called before the first frame update
    void Start()
    {
        startingHealth = displayedHealth;
        startingLocation = gameObject.transform.localPosition;

        bulletRefSpeed = bulletPrefab.GetComponent<BulletScript>().bulletSpeed;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        player = FindObjectOfType<PlayerController>();
        animator.SetBool("IsShotgunner", true);
    }

    // Update is called once per frame
    void Update()
    {
        onGround = (Physics2D.Raycast(transform.position + colliderOffset, Vector2.down, groundLength, groundLayer) || Physics2D.Raycast(transform.position - colliderOffset, Vector2.down, groundLength, groundLayer));

        if (IsClose()) //maybe a proper detection method would work better, right now they stop chasing a little bit too soon
        {
            isAwake = true;
        }
        else if (!IsClose() && displayedHealth > 0) //If the enemy isn't close and is not dead then...
        {
            isAwake = false;
            animator.Play("Idle"); //Idle animation doesn't even exist for most/all enemies, this can probably be left out
        }

        if(isAwake)
        {
            if (IsClose())
            {
                Flip(0);
                if (VertRangeSeesPlayer())
                {
                    animator.SetBool("IsChasing", true);
                    isChasing = true;
                    if (Math.Abs(player.transform.position.x - this.gameObject.transform.position.x) >= fireRange)
                    {
                        RunTowards();
                    }
                }
            }

            //firerateWait changes based on fps time
            firerateWait -= Time.deltaTime;
            if (isCloseEnough() && firerateWait <= 0 && VertRangeSeesPlayer())
            {
                animator.SetBool("IsChasing", false);
                isChasing = false;
                animator.Play("Attack");
                Shoot();
                firerateWait = firerate;
            }

            if (!IsClose() || player.displayedHealth <= 0)
            {
                animator.SetBool("IsChasing", false);
            }

            //Makes the enemy horizontal speed lower drastically while falling
            if (!onGround)
            {
                Vector2 airBrake = new Vector2(-(rb.velocity.x / 4), 0.0f);
                rb.AddForce(airBrake);
            }
        }
    }

   /* public void RunTowards()
    {
        if (Math.Abs(player.transform.position.x - this.gameObject.transform.position.x) >= fireRange)
        {
            if(facingRight && onGround)
            {
                Vector2 movement = new Vector2(-enemySpeed, 0.0f);
                if (rb.velocity.x <= -enemySpeed)
                {
                    rb.velocity = movement;
                }
                else if (rb.velocity.x >= -enemySpeed)
                {
                    rb.AddForce(movement);
                }
            }
            else if(onGround)
            {
                Vector2 movement = new Vector2(enemySpeed, 0.0f);
                if (rb.velocity.x >= -enemySpeed)
                {
                    rb.velocity = movement;
                }
                else if (rb.velocity.x <= enemySpeed)
                {
                    rb.AddForce(movement);
                }
            }
        }
    }*/

    private bool isCloseEnough()//Shotguns are never isTooClose() hehehehehe
    {
        if (Math.Abs(player.transform.position.x - this.gameObject.transform.position.x) <= fireRange && player.displayedHealth > 0) 
        {
            return true;
        }
        return false;
    }

    //Shotgunner needs a new shoot method as it shoots in more than one direction, this method makes 3 bullets, two at an angle
    //b shoots straight, c shoots above, d shoots below
    private new void Shoot()
    {
        FindObjectOfType<SFXManager>().PlayAudio("Gunshot");

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
            b.GetComponent<Rigidbody2D>().AddForce(Vector2.left * bulletRefSpeed);
            c.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 80f);
            c.GetComponent<Rigidbody2D>().AddForce(Vector2.left * bulletRefSpeed);
            c.GetComponent<Rigidbody2D>().AddForce(Vector2.up * (bulletRefSpeed / 6));
            d.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 100f);
            d.GetComponent<Rigidbody2D>().AddForce(Vector2.left * bulletRefSpeed);
            d.GetComponent<Rigidbody2D>().AddForce(Vector2.down * (bulletRefSpeed / 6));
        }
        else
        {
            b.transform.rotation = Quaternion.Euler(0.0f, 0.0f, -90f);
            b.GetComponent<Rigidbody2D>().AddForce(Vector2.right * bulletRefSpeed);
            c.transform.rotation = Quaternion.Euler(0.0f, 0.0f, -80f);
            c.GetComponent<Rigidbody2D>().AddForce(Vector2.right * bulletRefSpeed);
            c.GetComponent<Rigidbody2D>().AddForce(Vector2.up * (bulletRefSpeed / 6));
            d.transform.rotation = Quaternion.Euler(0.0f, 0.0f, -100f);
            d.GetComponent<Rigidbody2D>().AddForce(Vector2.right * bulletRefSpeed);
            d.GetComponent<Rigidbody2D>().AddForce(Vector2.down * (bulletRefSpeed / 6));
        }
    }
}
