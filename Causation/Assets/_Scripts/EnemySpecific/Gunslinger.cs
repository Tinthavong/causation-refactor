using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Gunslinger : Enemy
{
    public bool isCrouched;
    public GameObject tempCrouchIndicator;

    public Gunslinger() //constructor
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
        animator.SetBool("IsShotgunner", false);
    }

    // Update is called once per frame
    void Update()
    {
        onGround = (Physics2D.Raycast(transform.position + colliderOffset, Vector2.down, groundLength, groundLayer) || Physics2D.Raycast(transform.position - colliderOffset, Vector2.down, groundLength, groundLayer));

        //Controls where the enemy is looking
        //First in update to make sure bullet travels in correct direction
        if (isClose())
        {
            Flip(0f);
        }

        //firerateWait changes based on fps time
        firerateWait -= Time.deltaTime;
        //if firerateWait is 0, time to fire and reset the wait
        if (firerateWait <= 0 && isClose() && !isTooClose() && player.displayedHealth > 0 && vertRangeSeesPlayer() && !isCrouched)
        {
            animator.Play("Attack");
            Shoot();
            firerateWait = firerate;
            FindObjectOfType<SFXManager>().PlayAudio("Gunshot");

            //random chance to crouch? but for now"
            Invoke("Crouching", 2f);
        }

        //this is only necessary if they will move. right now they are all stationary.
        if (!isClose() || player.displayedHealth <= 0)
        {
            animator.SetBool("IsChasing", false);
        }

        //Consider making melee animations for the hybrid character
        /*
        if (firerateWait <= 0 && isTooClose())
        {
            //animation play here
            //Strike();
            //Using firerate as the buffer for melee for consistency, might replace later
            firerateWait = firerate;
        }*/

        //ElimCharacter();//Want to find some way for elimcharacter to be checked each time damage is taken, not on every frame like it is now
        /*
        if (displayedHealth < 0)
        {
            gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
            gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        }*/
    }

    //I don't like having two separate methods, combine them into one later
    private void Crouching()
    {
        isCrouched = true;
        //Crouching behavior, just needs to be used
        if (isCrouched)
        {
            tempCrouchIndicator.transform.localPosition = new Vector2(0.75f, 0.0f);
            GetComponent<CapsuleCollider2D>().size = new Vector2(1f, 1.55f);
            GetComponent<CapsuleCollider2D>().offset = new Vector2(0f, -0.5f);
        }
        Invoke("CrouchUp", 2f);
    }

    private void CrouchUp()
    {
        isCrouched = false;
        if (!isCrouched)
        {
            tempCrouchIndicator.transform.localPosition = new Vector2(0.75f, 0.75f);
            GetComponent<CapsuleCollider2D>().size = new Vector2(1f, 2.4f);
            GetComponent<CapsuleCollider2D>().offset = new Vector2(0f, 0f);
        }
    }

    //Can this be inherited?
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Projectile"))
        {
            ElimCharacter();
        }
    }

}
