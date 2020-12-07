using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Gunslinger : Enemy
{
    public bool isCrouched;

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
        startingHealth = displayedHealth;
        startingLocation = gameObject.transform.localPosition;

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
        if (IsClose())
        {
            Flip(0f);
            isAwake = true;
        }
        else if (!IsClose() && displayedHealth > 0) //If the enemy isn't close and is not dead then...
        {
            isAwake = false;
            animator.Play("Idle"); //Idle animation doesn't even exist for most/all enemies, this can probably be left out
        }

        if(isAwake)
        {
            //firerateWait changes based on fps time
            firerateWait -= Time.deltaTime;
            //if firerateWait is 0, time to fire and reset the wait
            if (firerateWait <= 0 && IsClose() && !IsTooClose() && player.displayedHealth > 0 && VertRangeSeesPlayer() && isCrouched)
            {
                animator.Play("Attack");
                Shoot();
                firerateWait = firerate;
                FindObjectOfType<SFXManager>().PlayAudio("Gunshot");

                //Changed it so he crouches until he shoots, then crouches again halfway through his firerate
                CrouchUp();
            }

            if (!isCrouched && firerateWait < (firerate / 2))
            {
                Crouching();
            }
        }    
    }

    //I don't like having two separate methods, combine them into one later.  It may be better to have them separate in order to have more control of it
    private void Crouching()
    {
        animator.SetBool("IsCrouched", true);
        isCrouched = true;
        if (isCrouched)
        {
            GetComponent<CapsuleCollider2D>().size = new Vector2(1f, 1.55f);
            GetComponent<CapsuleCollider2D>().offset = new Vector2(0f, -0.5f);
        }
        Invoke("CrouchUp", 2f);
    }

    private void CrouchUp()
    {
        animator.SetBool("IsCrouched", false);
        isCrouched = false;
        if (!isCrouched)
        {
            GetComponent<CapsuleCollider2D>().size = new Vector2(1f, 2.4f);
            GetComponent<CapsuleCollider2D>().offset = new Vector2(0f, 0f);
        }
    }
}
