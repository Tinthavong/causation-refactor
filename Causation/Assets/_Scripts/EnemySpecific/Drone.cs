using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Drone : Enemy
{
    public LayerMask ignoreLayers;
    public Drone() //constructor
    {
        Health = displayedHealth; //Displayed Health can be set in the inspector
        Stamina = staminaActions;
        //Enemies dont use ammo for now but if it breaks just set the amount here
        Currency = dropValue;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Going to tighten this up and use layerMask later, this is to have a drone right away
        Physics2D.IgnoreLayerCollision(gameObject.layer, 9, true);
        Physics2D.IgnoreLayerCollision(gameObject.layer, 11, true);
        Physics2D.IgnoreLayerCollision(gameObject.layer, 14, true);
        Physics2D.IgnoreLayerCollision(gameObject.layer, 15, true);
        Physics2D.IgnoreLayerCollision(gameObject.layer, 16, true);

        startingHealth = displayedHealth;
        startingLocation = gameObject.transform.localPosition;

        onGround = true; //This is necessary because RunTowards checks if the enemy is on the ground before being able to move
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        player = FindObjectOfType<PlayerController>();
        bulletRefSpeed = bulletPrefab.GetComponent<BulletScript>().bulletSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsClose())
        {
            isAwake = true;
        }
        else if (Math.Abs(transform.position.x - player.transform.position.x) >= (sightRange * 2) || player.displayedHealth <= 0)
        {
            rb.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
            isAwake = false;
        }

        if (isAwake)
        {
            if (!IsClose())
            {
                Flip(0);
                DroneMovement();
            }
            else if (IsClose())
            {
                Flip(0);
                //Drone will move vertically, matching the players y position to act as a wall until killed
                VertMovement();
            }

            //firing from a distance
            if (firerateWait <= 0 && IsTooClose() && player.displayedHealth > 0)
            {
                Flip(0);
                rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
                Shoot();
                firerateWait = firerate;
            }
            //firerateWait changes based on fps time
            firerateWait -= Time.deltaTime;
        }
    }

    private void DroneMovement()
    {
        //playerPos.X needs to be multiplied by something or subtracted/added so that it is not so huggy
        if (facingRight)
        {
            //var playerPos = player.transform.position;
            // playerPositionDifference = new Vector2(-playerPos.x, playerPos.y);
            Vector2 movement = new Vector2(-enemySpeed, 0.0f);
            rb.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;

            if (rb.velocity.x <= -enemySpeed)
            {
                rb.velocity = movement;
            }
            else if (rb.velocity.x >= -enemySpeed)
            {
                rb.AddForce(movement);
            }
        }
        else
        {

            // var playerPos = player.transform.position;
            // Vector2 playerPositionDifference = new Vector2(playerPos.x, playerPos.y);
            Vector2 movement = new Vector2(enemySpeed, 0.0f);
            rb.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;

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

    //Similar to how RunTowards() works, but using the y position instead of x
    private void VertMovement()
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
    }

    //Overriden shoot method to correctly angle the lasers
    public override void Shoot()
    {
        GameObject b = Instantiate(bulletPrefab) as GameObject;
        b.transform.position = bulletSpawn.transform.position;
        if (gameObject.transform.localScale.x < 0)
        {
            b.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            b.GetComponent<Rigidbody2D>().AddForce(Vector2.left * bulletPrefab.GetComponent<BulletScript>().bulletSpeed);

        }
        else
        {
            b.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 180f);
            b.GetComponent<Rigidbody2D>().AddForce(Vector2.right * bulletPrefab.GetComponent<BulletScript>().bulletSpeed);
        }
    }

    public override void PostDeath()
    {
        //When a drone dies another one will spawn and take its place, this isn't immediately necessary (11/29/20)
        //FindObjectOfType<LevelManager>().DroneSpawner();
        base.PostDeath();
        Destroy(gameObject);
    }
}
