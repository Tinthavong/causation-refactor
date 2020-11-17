using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Drone : Enemy
{

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
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        player = FindObjectOfType<PlayerController>();
        bulletRefSpeed = bulletPrefab.GetComponent<BulletScript>().bulletSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if(isClose())
        {
            Flip(0);
            //Drone will move vertically, matching the players y position to act as a wall until killed
            vertMovement();

            //Shoots when ready <-- sound change to laser sound if possible
            if(firerateWait <= 0)
            {
                Shoot();
                firerateWait = firerate;
            }
        }

        //firerateWait changes based on fps time
        firerateWait -= Time.deltaTime;

    }

    //Similar to how RunTowards() works, but using the y position instead of x
    private void vertMovement()
    {
        if(transform.position.y < player.transform.position.y)
        {
            Vector2 movement = new Vector2(0.0f, enemySpeed);
            if(rb.velocity.y > enemySpeed)
            {
                rb.velocity = movement;
            }
            else
            {
                rb.AddForce(movement);
            }
        }
        else if(transform.position.y > player.transform.position.y)
        {
            Vector2 movement = new Vector2(0.0f, -enemySpeed);
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

    public override void Shoot()
    {
        GameObject b = Instantiate(bulletPrefab) as GameObject;
        b.transform.position = bulletSpawn.transform.position;
        if (gameObject.transform.localScale.x < 0)
        {
            b.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            //b.GetComponent<Rigidbody2D>().AddForce(Vector2.left * bulletSpeed);
            b.GetComponent<Rigidbody2D>().AddForce(Vector2.left * bulletPrefab.GetComponent<BulletScript>().bulletSpeed);

        }
        else
        {
            b.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 180f);
            b.GetComponent<Rigidbody2D>().AddForce(Vector2.right * bulletPrefab.GetComponent<BulletScript>().bulletSpeed);
        }
    }
}
