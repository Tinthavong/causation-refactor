using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : CharacterBase
{
    public PlayerController() //Constructor
    {
        Health = 5; //Arbitrary value?
        displayedHealth = Health;//Displayed Health can be set in the inspector
        Stamina = staminaActions;
        //Enemies dont use ammo for now but if it breaks just set the amount here
        Ammo = maxAmmo; //To avoid confusion, the Ammo property is like the previous currentAmmo variable
        Currency = walletValue;
    }



    //consider serializing private
    [Header("UI References")]
    public HealthBar healthBar; //I don't like the idea of having a type just for healthbar, redo later
    public TMP_Text ammoText;
    public int walletValue; //think  of a better name, but this has all of the player's screws/currency
    public int maxAmmo = 6; //six-shooter by default, set in inspector otherwise
    Animator animator;
    Rigidbody2D rb; //rigidbody used for jumping

    [Header("Movement Stats")]
    [SerializeField]
    //Player's running speed
    //An acceleration function might be cool, hold a horizontal direction down longer and you move faster?
    public int runSpeed = 5;
    //Jumping
    public float jumpForce = 700;
    private float axisY;

    //Movement Axes stash, vertical could go here as well if there was going to be vertical movement like a beat em up
    private float horizontal;
    private float vertical;

    //Animation and design
    private bool facingRight;

    public GameObject nearObject;
    [Header("Player States")]
    public bool isJumping;
    private bool isCrouched;
    public bool isShooting;
    public LevelManager LM;

    void Awake()
    {
        //Can hardcode displayed health here if necessary
        //Apparently coding it at the top doesn't work so might have to do that. Otherwise be sure to set in inspector

        axisY = gameObject.transform.position.y; //axisY is set immediately to the player game object's y pos
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        //rb.Sleep();
    }

    // Update is called once per frame
    void Update()
    {
        ShootingBehavior();
        StrikingBehavior();
        //Tighter, specific controls might be better here in order to set the speed to 0 immediately when the key is lifted (an abrupt end to the animation)
        horizontal = Input.GetAxis("Horizontal");
        animator.SetFloat("Speed", Mathf.Abs(horizontal != 0 ? horizontal : 0)); //ternary, think of it like a boolean: (is horizontal != 0? if true then horizontal value :else 0)
    }

    private void FixedUpdate()
    {
        if (horizontal != 0 && !isCrouched)
        {
            Vector3 movement = new Vector3(horizontal * runSpeed, 0.0f, 0.0f);
            transform.position = transform.position + movement * Time.deltaTime;
        }
        Flip(horizontal);

        //reminder, axisY is the y coordinate that the player jumped from so once the player falls back down and their y position is less than or equal to it will stop falling 
        if (rb.velocity.y <= 0 && isJumping) //this doesn't make any sense, if it does 
        {
            OnLanding();
        }
        //jumping isn't perfect, the player model will continuously go down instead of staying at the value that it jumped from

        if (Input.GetButton("Jump") && !isJumping && !isCrouched)
        {
            //animator.SetBool("IsJumping", true);
            animator.Play("GrandpaJump");
            axisY = transform.position.y;
            // Debug.Log("Jumped at " + axisY);
            isJumping = true;
            //rb.gravityScale = 1.5f;
            rb.WakeUp();
            rb.AddForce(new Vector2(0, jumpForce));
        }

        vertical = Input.GetAxis("Vertical");
        if (vertical < 0)
        {
            //make it so movement is stopped or forced into crouch walk
            animator.SetBool("IsCrouched", true);
            isCrouched = true;
            if (isCrouched)//imagine using a nested if in the update method loooole
            {
                //bulletspawn transform goes down with the player model
                GetComponent<CapsuleCollider2D>().size = new Vector2(1f, 1.45f);
                GetComponent<CapsuleCollider2D>().offset = new Vector2(0f, -0.4f);
            }

        }
        else if (Input.GetAxis("Vertical") > -1)
        {
            //bulletspawn goes back to normal
            //the "exit" time when not pressing the up key is too slow right now
            animator.SetBool("IsCrouched", false);
            isCrouched = false;
            GetComponent<CapsuleCollider2D>().size = new Vector2(1f, 2.3f);
            GetComponent<CapsuleCollider2D>().offset = new Vector2(0f, 0f);
        }

        //Redundant with shooting behavior, place this in there
        if (Input.GetButtonDown("Fire1"))
        {
            //animator.SetBool("IsShooting", true);
            //   animator.Play("GrandpaShoot");
        }


    }

    public void OnLanding()
    {
        if (rb.velocity.y == 0) isJumping = false;
        //This works but when jumping on crates it waits until the velocity is 0 before jump is allowed again
        //A movement refactor and environmental system overwork is needed to fix this but i will do that last - Tim
    }

    private void StrikingBehavior()
    {
        if (Input.GetButtonDown("Fire2"))//and a bool/state check that determines if the player is not already shooting
        {
            Strike();
        }
    }


    private void ShootingBehavior()
    {
        if (ammoText.text != null)
        {
            ammoText.text = Ammo.ToString();

            if (Input.GetButtonDown("Fire1") && Ammo > 0)
            {
                if (Ammo == 0)
                {
                    Debug.Log("No Ammo Left!");//implement affordance, clicking sound or something
                }
                else
                {
                    if (!PauseController.isPaused)
                    {
                        Shoot();
                        Ammo--;
                    }
                }
            }

            if (!PauseController.isPaused)
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    Ammo = maxAmmo;
                }
            }

        }
    }

    public override void DamageCalc(int damage)
    {
        base.DamageCalc(damage);
        healthBar.SetHealth(displayedHealth);
        ElimCharacter();
    }

    //Handles flipping the sprite across the x axis to show that movement direction has changed
    public override void Flip(float horizontal)
    {
        if (horizontal < 0 && !facingRight || horizontal > 0 && facingRight)
        {
            facingRight = !facingRight;
            Vector3 scale = transform.localScale;
            scale.x *= -1;

            transform.localScale = scale;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Checkpoint"))
        {
            Time.timeScale = 0f;
            LM.VictoryCheck();
        }
    }


    public override void PostDeath()
    {
        //death animation here
        LM.GameOver();
        enabled = false; //self-explanatory but this turns off the ability to move around with the player. we can pause the gameworld too, but this way still plays enemy animations if they're still around the player
        GetComponent<CapsuleCollider2D>().enabled = false; //Dirty fix right now. The enemy should stop attacking if the player is dead anyways
    }
}
