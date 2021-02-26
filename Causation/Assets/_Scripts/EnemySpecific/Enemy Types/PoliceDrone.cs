using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PoliceDrone : EnemyBaseCombatController
{
    public LayerMask ignoreLayers;

    // Start is called before the first frame update
   new void Start()
    {
        //Going to tighten this up and use layerMask later, this is to have a drone right away
        Physics2D.IgnoreLayerCollision(gameObject.layer, 9, true);
        Physics2D.IgnoreLayerCollision(gameObject.layer, 11, true);
        Physics2D.IgnoreLayerCollision(gameObject.layer, 14, true);
        Physics2D.IgnoreLayerCollision(gameObject.layer, 15, true);
        Physics2D.IgnoreLayerCollision(gameObject.layer, 16, true);

        //onGround = true; //This is necessary because RunTowards checks if the enemy is on the ground before being able to move
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyDetection.IsClose())
        {
            isAwake = true;
        }
        else if (Math.Abs(transform.position.x - player.transform.position.x) >= (enemyDetection.sightRange * 2) || player.IsDead())
        {
            enemyBase.rb.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
            isAwake = false;
        }

        if (isAwake)
        {
            if (!enemyDetection.IsClose())
            {
                enemyBase.Flip();
                DroneMovement();
            }
            else if (enemyDetection.IsClose())
            {
                enemyBase.Flip();
                //Drone will move vertically, matching the players y position to act as a wall until killed
                VertMovement();
            }

            //firing from a distance
            if (firerateWait <= 0 && enemyDetection.IsTooClose() && !player.IsDead())
            {
                enemyBase.Flip();
                enemyBase.rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
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
        if (enemyBase.facingRight)
        {
            //var playerPos = player.transform.position;
            // playerPositionDifference = new Vector2(-playerPos.x, playerPos.y);
            Vector2 movement = new Vector2(-enemyBase.enemySpeed, 0.0f);
            enemyBase.rb.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;

            if (enemyBase.rb.velocity.x <= -enemyBase.enemySpeed)
            {
                enemyBase.rb.velocity = movement;
            }
            else if (enemyBase.rb.velocity.x >= -enemyBase.enemySpeed)
            {
                enemyBase.rb.AddForce(movement);
            }
        }
        else
        {

            // var playerPos = player.transform.position;
            // Vector2 playerPositionDifference = new Vector2(playerPos.x, playerPos.y);
            Vector2 movement = new Vector2(enemyBase.enemySpeed, 0.0f);
            enemyBase.rb.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;

            if (enemyBase.rb.velocity.x >= -enemyBase.enemySpeed)
            {
                enemyBase.rb.velocity = movement;
            }
            else if (enemyBase.rb.velocity.x <= enemyBase.enemySpeed)
            {
                enemyBase.rb.AddForce(movement);
            }
        }
    }

    //Similar to how RunTowards() works, but using the y position instead of x
    private void VertMovement()
    {
        if (transform.position.y < player.transform.position.y)
        {
            Vector2 movement = new Vector2(0.0f, enemyBase.enemySpeed);
            //Sets velocity to max if the force ever makes it go over
            if (enemyBase.rb.velocity.y > enemyBase.enemySpeed)
            {
                enemyBase.rb.velocity = movement;
            }
            else
            {
                enemyBase.rb.AddForce(movement);
            }
        }
        else if (transform.position.y > player.transform.position.y)
        {
            Vector2 movement = new Vector2(0.0f, -enemyBase.enemySpeed);
            //Sets velocity to max if the force ever makes it go over
            if (enemyBase.rb.velocity.y < -enemyBase.enemySpeed)
            {
                enemyBase.rb.velocity = movement;
            }
            else
            {
                enemyBase.rb.AddForce(movement);
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
            b.GetComponent<Rigidbody2D>().AddForce(Vector2.left * bulletPrefab.GetComponent<ProjectileProperties>().projectileSpeed);

        }
        else
        {
            b.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 180f);
            b.GetComponent<Rigidbody2D>().AddForce(Vector2.right * bulletPrefab.GetComponent<ProjectileProperties>().projectileSpeed);
        }
    }
}
