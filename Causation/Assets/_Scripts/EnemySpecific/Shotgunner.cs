using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Shotgunner : Enemy
{
    Animator animator;
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

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        player = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isClose())
        {
            Flip(0);
            animator.SetBool("IsChasing", true);
            RunTowards();
        }

        //firerateWait changes based on fps time
        firerateWait -= Time.deltaTime;
        if (isCloseEnough() && firerateWait <= 0)
        {
            animator.SetBool("IsChasing", false);
            animator.Play("Attack");
            Shoot();
            firerateWait = firerate;
        }

        if (!isClose() || player.displayedHealth <= 0)
        {
            animator.SetBool("IsChasing", false);
        }
        ElimCharacter();//Want to find some way for elimcharacter to be checked each time damage is taken, not on every frame like it is now
    }

    public override void DamageCalc(int damage)
    {
        animator.Play("Damaged");
        base.DamageCalc(damage);
    }

    public void RunTowards()
    {
        if (Math.Abs(player.transform.position.x - this.gameObject.transform.position.x) >= fireRange)
        {
            if(facingRight)
            {
                Vector3 movement = new Vector3(-enemySpeed, 0.0f, 0.0f);
                transform.position = transform.position + movement * Time.deltaTime;
            }
            else
            {
                Vector3 movement = new Vector3(enemySpeed, 0.0f, 0.0f);
                transform.position = transform.position + movement * Time.deltaTime;
            }
        }
    }

    private bool isClose() 
    {
        if (Math.Abs(player.transform.position.x - this.gameObject.transform.position.x) < sightRange && player.displayedHealth > 0) //Dirty fix. Stop, he's already dead!
        {
            return true;
        }
        return false;
    }

    private bool isCloseEnough()//Shotguns are never too close hehehehehe
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
            b.GetComponent<Rigidbody2D>().AddForce(Vector2.left * bulletSpeed);
            c.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 80f);
            c.GetComponent<Rigidbody2D>().AddForce(Vector2.left * bulletSpeed);
            c.GetComponent<Rigidbody2D>().AddForce(Vector2.up * (bulletSpeed/6));
            d.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 100f);
            d.GetComponent<Rigidbody2D>().AddForce(Vector2.left * bulletSpeed);
            d.GetComponent<Rigidbody2D>().AddForce(Vector2.down * (bulletSpeed/6));
        }
        else
        {
            b.transform.rotation = Quaternion.Euler(0.0f, 0.0f, -90f);
            b.GetComponent<Rigidbody2D>().AddForce(Vector2.right * bulletSpeed);
            c.transform.rotation = Quaternion.Euler(0.0f, 0.0f, -80f);
            c.GetComponent<Rigidbody2D>().AddForce(Vector2.right * bulletSpeed);
            c.GetComponent<Rigidbody2D>().AddForce(Vector2.up * (bulletSpeed/6));
            d.transform.rotation = Quaternion.Euler(0.0f, 0.0f, -100f);
            d.GetComponent<Rigidbody2D>().AddForce(Vector2.right * bulletSpeed);
            d.GetComponent<Rigidbody2D>().AddForce(Vector2.down * (bulletSpeed/6));
        }
    }
}
