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
    public GameObject deathScreen;

    [Header("Movement Stats")]
    [SerializeField]
    //Player's running speed
    public float moveSpeed = 10f;
    public float maxSpeed = 7f;
    public Vector2 direction;
    private bool facingRight = true;

    //Jumping
    public float jumpSpeed = 15f;
    public float jumpDelay = 0.25f;
    private float jumpTimer;

    //Attacking
    float attackElapsedTime = 0;
    public float attackDelay = 0.2f;

    [Header("Components")]
    public GameObject[] bulletList;
    public GameObject nearObject;
    public LevelManager LM;

    [Header("Player States")]
    public bool isJumping;
    private bool isCrouched;
    public bool isShooting;
    private bool isInvincible = false;
    private bool canMove = true;

    [Header("Collision and Physics")]
    public LayerMask groundLayer;
    public bool onGround = false;
    public float groundLength = 0.6f;
    public Vector3 colliderOffset;
    //physics
    public float linearDrag = 4f;
    public float gravity = 1f;
    public float fallMultiplier = 5f;


    //reworked movement but here's the leftovers
    private float vertical;

    void Awake()
    {
        //Can hardcode displayed health here if necessary
        //Apparently coding it at the top doesn't work so might have to do that. Otherwise be sure to set in inspector
        LM = FindObjectOfType<LevelManager>();
        //axisY = gameObject.transform.position.y; //axisY is set immediately to the player game object's y pos
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        //rb.Sleep();
    }

    private void Update()
    {
        onGround = Physics2D.Raycast(transform.position + colliderOffset, Vector2.down, groundLength, groundLayer) || Physics2D.Raycast(transform.position - colliderOffset, Vector2.down, groundLength, groundLayer);
        if (canMove)
        {
            if (Input.GetKeyDown(KeyCode.T) && bulletPrefab != bulletList[1])
            {
                Ammo = maxAmmo; //right now the special bullet does not have a finite amount, would like to make finite before alpha deliverable
                bulletPrefab = bulletList[1]; //this should use something more algorithmic. There are only two bullets right now, however
            }
            else if (Input.GetKeyDown(KeyCode.T) && bulletPrefab != bulletList[0])
            {
                Ammo = maxAmmo; //default ammo will have infinite
                bulletPrefab = bulletList[0];
            }

            attackElapsedTime += Time.deltaTime;
            ShootingBehavior();
            StrikingBehavior();

            if (Input.GetButtonDown("Jump"))
            {
                jumpTimer = Time.time + jumpDelay;
            }

            direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            vertical = Input.GetAxis("Vertical");
            if (vertical < 0 && direction.x == 0)
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
        }
        else if (!canMove)
        {
            animator.SetBool("IsDead", true);
            animator.Play("GrandpaDeath");
        }
    }

    private void FixedUpdate()
    {
        if (!isCrouched) moveCharacter(direction.x);

        if (jumpTimer > Time.time && onGround && !isCrouched)
        {
            Jump();
        }

        modifyPhysics();
    }

    void moveCharacter(float horizontal)
    {
        rb.AddForce(Vector2.right * horizontal * moveSpeed);
        //animator.SetFloat("Speed", Mathf.Abs(horizontal != 0 ? horizontal : 0));
        if ((horizontal > 0 && !facingRight) || (horizontal < 0 && facingRight))
        {   
            Flip(0f);
        }

        if (Mathf.Abs(rb.velocity.x) > maxSpeed)
        {
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxSpeed, rb.velocity.y);
        }
        animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
    }

    //Handles flipping the sprite across the x axis to show that movement direction has changed
    public override void Flip(float horizontal)
    {
        facingRight = !facingRight;
        transform.rotation = Quaternion.Euler(0, facingRight ? 0 : 180, 0);      
    }

    void Jump()
    {
        animator.Play("GrandpaJump");
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
        jumpTimer = 0;
    }

    void modifyPhysics()
    {
        bool changingDirection = (direction.x > 0 && rb.velocity.x < 0) || (direction.x < 0 && rb.velocity.x > 0);

        if (onGround)
        {
            if (Mathf.Abs(direction.x) < 0.4f || changingDirection)
            {
                rb.drag = linearDrag;
            }
            else
            {
                rb.drag = 0f;
            }
            rb.gravityScale = 0;
        }
        else
        {
            rb.gravityScale = gravity;
            rb.drag = linearDrag * 0.15f;
            if (rb.velocity.y < 0)
            {
                rb.gravityScale = gravity * fallMultiplier;
            }
            else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
            {
                rb.gravityScale = gravity * (fallMultiplier / 2);
            }
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
        if (Input.GetButtonDown("Fire2") && attackElapsedTime >= attackDelay)//and a bool/state check that determines if the player is not already shooting
        {
            attackElapsedTime = 0; //It's the same principle as shooting and this SHOULD work but 
            //going to spawn a red circle wherever strikezone is to show damage affordance
            Strike();
        }
    }

    private void ShootingBehavior()
    {
        if (ammoText.text != null)
        {
            ammoText.text = Ammo.ToString();

            if (Input.GetButtonDown("Fire1") && Ammo > 0 && attackElapsedTime >= attackDelay)
            {
                animator.Play("GrandpaShoot");
                if (Ammo == 0)
                {
                    Debug.Log("No Ammo Left!");//implement affordance, clicking sound or something
                }
                else
                {
                    if (!PauseController.isPaused)
                    {
                        attackElapsedTime = 0;
                        Shoot();
                        Ammo--;
                    }
                }
                animator.SetBool("IsShooting", false);
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

    public override void Shoot()
    {
        //This block of code is why we should use raycasts 
        GameObject b = Instantiate(bulletPrefab) as GameObject;
        b.transform.position = bulletSpawn.transform.position;
        //Bullet object shifts position and rotation based on direction
        if (!facingRight)
        {
            b.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 90f);
            b.GetComponent<Rigidbody2D>().AddForce(Vector2.left * bulletSpeed);
        }
        else if (facingRight)
        {
            b.transform.rotation = Quaternion.Euler(0.0f, 0.0f, -90f);
            b.GetComponent<Rigidbody2D>().AddForce(Vector2.right * bulletSpeed);
        }
    }

    public override void DamageCalc(int damage)
    {
        if (isInvincible) return;
        base.DamageCalc(damage);
        healthBar.SetHealth(displayedHealth);
        ElimCharacter();
        animator.PlayInFixedTime("GrandpaDamage", -1, 1f);
        StartCoroutine(Invinciblity());
    }

    private IEnumerator Invinciblity()
    {
        isInvincible = true;
        yield return new WaitForSeconds(1.5f);
        isInvincible = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //should use switch/cases but that's for later
        if (collision.CompareTag("FinishLine"))//I think this tag is fine for now
        {
            //Time.timeScale = 0f; If there are no panels then this is completely useless
            LM.VictoryCheck();
        }

        if (collision.CompareTag("Checkpoint"))
        {
            LM.checkpoint.GetComponent<SpriteRenderer>().color = Color.green;
            LM.flaggedCheckpoint = true; //be sure to reset all flags when restarting or changing levels
        }
    }


    public override void PostDeath()
    {
        //LM.RetryCheckpoint();
        //death animation here
        LM.GameOver();
        canMove = false; //self-explanatory but this turns off the ability to move around with the player. we can pause the gameworld too, but this way still plays enemy animations if they're still around the player
        GetComponent<CapsuleCollider2D>().enabled = false; //Dirty fix right now. The enemy should stop attacking if the player is dead anyways
    }

    public void Replenish()
    {
        displayedHealth = Health;
        healthBar.SetHealth(displayedHealth);
        Ammo = maxAmmo;
        canMove = true;
        GetComponent<CapsuleCollider2D>().enabled = true;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + colliderOffset, transform.position + colliderOffset + Vector3.down * groundLength);
        Gizmos.DrawLine(transform.position - colliderOffset, transform.position - colliderOffset + Vector3.down * groundLength);

    }
}
