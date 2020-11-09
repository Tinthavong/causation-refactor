using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerController : CharacterBase
{
    public PlayerController() //Constructor
    {
        Health = displayedHealth;
        Stamina = staminaActions;
        Ammo = maxAmmo; //To avoid confusion, the Ammo property is like the previous currentAmmo variable
        Currency = walletValue;
    }

    //consider serializing private
    [Header("UI References")]
    public HealthBar healthBar; //I don't like the idea of having a type just for healthbar, redo later
    public TMP_Text ammoText;
    public Image ammoDisplay; //probably temporary, i have a better idea
    public static int walletValue; //think  of a better name, but this has all of the player's screws/currency
    public int maxAmmo = 6; //six-shooter by default, set in inspector otherwise - set with weapon types

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
    public float fireDelay = 1f;

    //This is for the temporary bullet swapping system
    public bool isBurstFire = false; //extremely lazy implementation: false = default revolver true = burst fire SMG
    private int shotAmount = 1; //shoots 1 bullet by default, changes for burst fire

    [Header("Components")]
    public GameObject[] bulletList;
    private int bulletTypeCount;
    public int bulletIndex = 0;

    public GameObject nearObject;
    public LevelManager LM;

    [Header("Player States")]
    public bool isJumping;
    private bool isCrouched = false;
    public bool isShooting;
    private bool isInvincible = false;
    public bool canMove = true;

    [Header("Collision and Physics")]
    //physics
    public float linearDrag = 4f;
    public float gravity = 1f;
    public float fallMultiplier = 5f;
    private int playerLayer, platformLayer;
    public float jumpDownTime = 2f;

    //reworked movement but here's the leftovers
    private float vertical;
    [SerializeField]
    private GameObject tempRevolver, tempPunch; //necessary to show that there is a revolver when crouched or to show that a punch is happening

    void Awake()
    {
        if (Time.timeScale <= 0f)
        {
            Time.timeScale = 1f;
        }

        bulletTypeCount = (bulletList.Length); //Sets the maximum amount of varied bullets depending the prefabs set in the inspector
        bulletPrefab = bulletList[0]; //The default bullet is 0, hardcoding that should be okay
        WeaponTypeAssign();

        healthBar.SetHealth(displayedHealth);
        LM = FindObjectOfType<LevelManager>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        playerLayer = LayerMask.NameToLayer("Player");
        platformLayer = LayerMask.NameToLayer("Platform");
    }

    private void ChangeBullets()
    {
        //switch-case might be best for cycling through multiple bullet types
        switch (bulletIndex)
        {
            case 0:
                bulletIndex++;
                ammoDisplay.sprite = bulletList[bulletIndex].GetComponent<SpriteRenderer>().sprite;
                Ammo = maxAmmo;
                bulletPrefab = bulletList[bulletIndex];
                break;

            case 1:
                bulletIndex++;
                ammoDisplay.sprite = bulletList[bulletIndex].GetComponent<SpriteRenderer>().sprite;
                Ammo = maxAmmo;
                bulletPrefab = bulletList[bulletIndex];
                break;

            case 2:
                bulletIndex = 0; //last bullet, goes back to the original default bullet
                ammoDisplay.sprite = bulletList[bulletIndex].GetComponent<SpriteRenderer>().sprite;
                Ammo = maxAmmo;
                bulletPrefab = bulletList[bulletIndex];
                break;

            default:
                bulletIndex = 0;
                ammoDisplay.sprite = bulletList[bulletIndex].GetComponent<SpriteRenderer>().sprite;
                Ammo = maxAmmo;
                bulletPrefab = bulletList[bulletIndex];
                break;
        }
    }

    private void Update()
    {
        onGround = Physics2D.Raycast(transform.position + colliderOffset, Vector2.down, groundLength, groundLayer) || Physics2D.Raycast(transform.position - colliderOffset, Vector2.down, groundLength, groundLayer);
        if (canMove)
        {
            //extract out into a method but for the meanwhile
            //This allows changing bullet types
            if (Input.GetKeyDown(KeyCode.T))
            {
                ChangeBullets();
            }

            attackElapsedTime += Time.deltaTime;
            ShootingBehavior();
            StrikingBehavior();

            if (Input.GetButtonDown("Jump") && !Input.GetKey(KeyCode.S))
            {
                jumpTimer = Time.time + jumpDelay;
            }
            else if (Input.GetButton("Jump") && Input.GetKey(KeyCode.S))
            {
                StartCoroutine("JumpDown");
            }

            direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));

            //This allows crouching
            if (Input.GetKeyDown(KeyCode.S) && onGround)
            {
                //make it so movement is stopped or forced into crouch walk
                animator.Play("GrandpaCrouch");
                animator.SetBool("IsCrouched", true);
                isCrouched = true;
                rb.velocity = new Vector2(0f, 0f);
                if (isCrouched)//imagine using a nested if in the update method loooole
                {
                    tempRevolver.SetActive(true);
                    bulletSpawn.transform.localPosition = new Vector3(0.85f, -0.4f, 0);
                    GetComponent<CapsuleCollider2D>().size = new Vector2(1f, 1.45f);
                    GetComponent<CapsuleCollider2D>().offset = new Vector2(0f, -0.4f);
                }
            }
            else if (Input.GetKeyUp(KeyCode.S))
            {
                //bulletspawn goes back to normal
                //the "exit" time when not pressing the up key is too slow right now
                tempRevolver.SetActive(false);
                animator.SetBool("IsCrouched", false);
                isCrouched = false;
                bulletSpawn.transform.localPosition = new Vector3(0.85f, 0.5f, 0);
                GetComponent<CapsuleCollider2D>().size = new Vector2(1f, 2.3f);
                GetComponent<CapsuleCollider2D>().offset = new Vector2(0f, 0f);
            }

        }
        else if (!canMove && displayedHealth <= 0)
        {
            animator.SetBool("IsDead", true);
            animator.Play("GrandpaDeath");
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
        }
    }

    IEnumerator JumpDown()
    {
        Physics2D.IgnoreLayerCollision(playerLayer, platformLayer, true);
        onGround = false;
        yield return new WaitForSeconds(jumpDownTime);
        Physics2D.IgnoreLayerCollision(playerLayer, platformLayer, false);
        onGround = true;
    }

    private void FixedUpdate()
    {
        if (!isCrouched)
        {
            moveCharacter(direction.x);
        }

        if (jumpTimer > Time.time && onGround && !isCrouched)
        {
            Jump();
        }

        modifyPhysics();
    }

    void moveCharacter(float horizontal)
    {
        rb.AddForce(Vector2.right * horizontal * moveSpeed);
        if ((horizontal > 0 && !facingRight) || (horizontal < 0 && facingRight))
        {
            Flip(0f);
        }

        if (Mathf.Abs(rb.velocity.x) > maxSpeed)
        {
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxSpeed, rb.velocity.y);
        }
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

    private void StrikingBehavior()
    {
        if (Input.GetButtonDown("Fire2") && attackElapsedTime >= attackDelay)//and a bool/state check that determines if the player is not already shooting
        {
            attackElapsedTime = 0;
            tempPunch.SetActive(true);
            Strike();
        }
        if (attackElapsedTime > 0)
        {
            tempPunch.SetActive(false);
        }
    }

    //Simplified and still worked.
    private void ShootingBehavior()
    {
        if (ammoText.text != null)
        {
            ammoText.text = Ammo.ToString();

            if (Input.GetButtonDown("Fire1") && Ammo > 0 && attackElapsedTime >= attackDelay)
            {
                if (!isCrouched) animator.Play("GrandpaShoot");
                for (int i = 0; i < shotAmount; i++)
                {
                    Invoke("Shoot", (fireDelay * i));
                    Ammo--;
                }
                attackElapsedTime = 0;
                animator.SetBool("IsShooting", false);
            }

            else if (Input.GetButtonDown("Fire1") && Ammo == 0)
            {
                Debug.Log("No Ammo Left!");//implement affordance, clicking sound or something
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                Ammo = maxAmmo;
            }
        }
    }

    private void WeaponTypeAssign() //Ammo capacity will be assigned depending on what type of gun it is, single shot, burst fire, etc (burst fire has a higher capacity)
    {
        //assigns ammo capacity, weapon firing type
        //if revolver ammo is 6 if smg ammo is 15
        //also set attack delay here?
        //needs to be heavily refined

        if (isBurstFire)
        {
            shotAmount = 3;
            maxAmmo = 15; //smg/burstfire guns
            fireDelay = 0.05f;
            //bulletPrefab.GetComponent<BulletScript>().bulletSpeed = 1500f;
            attackDelay = 0.5f;
        }
        else
            return;
    }

    public override void Shoot()
    {
        FindObjectOfType<SFXManager>().PlayAudio("Gunshot");
        //switch cases for firing modes
        //This block of code is why we should use raycasts 
        GameObject b = Instantiate(bulletPrefab) as GameObject;
        b.transform.position = bulletSpawn.transform.position;
        //Bullet object shifts position and rotation based on direction
        if (!facingRight)
        {
            b.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 90f);
            b.GetComponent<Rigidbody2D>().AddForce(Vector2.left * bulletPrefab.GetComponent<BulletScript>().bulletSpeed);
        }
        else if (facingRight)
        {
            b.transform.rotation = Quaternion.Euler(0.0f, 0.0f, -90f);
            b.GetComponent<Rigidbody2D>().AddForce(Vector2.right * bulletPrefab.GetComponent<BulletScript>().bulletSpeed);
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

    public override void ElimCharacter()
    {
        if (displayedHealth <= 0)
        {
            PostDeath();
        }
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
            Time.timeScale = 0f;
            LM.VictoryCheck();
        }

        if (collision.CompareTag("Checkpoint"))
        {
            LM.checkpoint.GetComponent<SpriteRenderer>().color = Color.green;
            LM.flaggedCheckpoint = true; //be sure to reset all flags when restarting or changing levels
        }

        if (collision.CompareTag("ScrewPickUp"))
        {
            walletValue++; //screws have a default value of 1 anyways

            LM.CheckpointCostCheck(gameObject.GetComponent<PlayerController>());
        }
    }


    public override void PostDeath()
    {
        LM.GameOver();
        canMove = false; //self-explanatory but this turns off the ability to move around with the player. we can pause the gameworld too, but this way still plays enemy animations if they're still around the player
        GetComponent<CapsuleCollider2D>().enabled = false; //Dirty fix right now. The enemy should stop attacking if the player is dead anyways
    }

    public void Replenish()
    {
        //rb.WakeUp();
        LM.CheckpointCostCheck(gameObject.GetComponent<PlayerController>());
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        animator.SetBool("IsDead", false);
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
