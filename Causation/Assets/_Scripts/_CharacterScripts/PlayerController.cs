using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerController : CharacterBase
{
    //Controller for the son and granddaughter because they don't have bullet swapping
    public PlayerController() //Constructor
    {
        Health = displayedHealth;
        Stamina = staminaActions;
        Ammo = maxAmmo; //To avoid confusion, the Ammo property is like the previous currentAmmo variable
                        // Currency = walletValue;
    }

    //consider serializing private
    [Header("UI References")]
    public HealthBar healthBar; //I don't like the idea of having a type just for healthbar, redo later
    public TMP_Text ammoText; //current ammo
    public TMP_Text ammoTotalText; //available ammo
    public Image ammoDisplay; //probably temporary, i have a better idea

    public Animator animator;
    public Rigidbody2D rb;
    public GameObject deathScreen;

    [Header("Movement Stats")]
    [SerializeField]
    //Player's running speed
    public float moveSpeed = 10f;
    public float maxSpeed = 7f;
    public Vector2 direction;
    public bool facingRight = true;

    //Jumping
    public float jumpSpeed = 15f;
    public float jumpDelay = 0.25f; 
    public float jumpTimer;

    //Attacking
    public float attackElapsedTime = 0;
    public float attackDelay = 0.2f;
    public float fireDelay = 1f;

    //This is for the temporary bullet swapping system
    public bool isBurstFire = false; //false = default revolver true = burst fire SMG
    public int shotAmount = 1; //shoots 1 bullet by default, changes for burst fire

    [Header("Components")]
    public int maxAmmo = 15; //15 round, burst fire SMG by default 

    public GameObject nearObject;
    public LevelManager LM; //can we get rid of these?

    [Header("Player States")]
    public bool isJumping;
    public bool isCrouched = false;
    public bool isShooting;
    public bool isInvincible = false;
    public bool canMove = true;

    [Header("Collision and Physics")]
    //physics
    public float linearDrag = 4f;
    public float gravity = 1f;
    public float fallMultiplier = 5f;
    public int playerLayer, platformLayer;
    public float jumpDownTime = 2f;

    //reworked movement but here's the leftovers
    private float vertical;

    //This shows the gun if the player is crouching so that they can see where bullet is coming from - placeholder
    //public GameObject tempCrouchShoot;

    void Awake()
    {
        if (Time.timeScale <= 0f)
        {
            Time.timeScale = 1f;
        }

        WeaponTypeAssign(); //redesigned bullet swapping so this has to change

        healthBar.SetHealth(displayedHealth);
        LM = FindObjectOfType<LevelManager>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        playerLayer = LayerMask.NameToLayer("Player");
        platformLayer = LayerMask.NameToLayer("Platform");
    }

    private void WeaponTypeAssign() //Might still be useful for differentiating between son and granddaughter?
    {
        //Assign remaining ammo here?

        if (isBurstFire)
        {
            shotAmount = 3;
            maxAmmo = 15; //smg/burstfire guns
            fireDelay = 0.05f;
            attackDelay = 0.5f;
        }
        else
            return;
    }

    private void ShootingBehavior()
    {
        if (ammoText.text != null)
        {
            ammoText.text = Ammo.ToString();

            if (Input.GetButtonDown("Fire1") && Ammo > 0 && attackElapsedTime >= attackDelay)
            {
                if (!isCrouched)
                {
                    animator.Play("Shoot");
                }
                else if(isCrouched)
                {
                    animator.Play("CrouchShoot");
                }

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
                FindObjectOfType<SFXManager>().PlayAudio("Reload");
            }
        }
    }

    private void Update()
    {
        onGround = Physics2D.Raycast(transform.position + colliderOffset, Vector2.down, groundLength, groundLayer) || Physics2D.Raycast(transform.position - colliderOffset, Vector2.down, groundLength, groundLayer);
        if (canMove)
        {
            attackElapsedTime += Time.deltaTime;
            ShootingBehavior();

            if (Input.GetButtonDown("Jump") && !Input.GetKey(KeyCode.S))
            {
                jumpTimer = Time.time + jumpDelay;
                FindObjectOfType<SFXManager>().PlayAudio("Jump");
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
                animator.Play("Crouch");
                animator.SetBool("IsCrouched", true);
                isCrouched = true;
                rb.velocity = new Vector2(0f, 0f);
                if (isCrouched)
                {
                    bulletSpawn.transform.localPosition = new Vector3(0.85f, -0.4f, 0);
                    GetComponent<CapsuleCollider2D>().size = new Vector2(1f, 1.45f);
                    GetComponent<CapsuleCollider2D>().offset = new Vector2(0f, -0.4f);
                }
            }
            else if (Input.GetKeyUp(KeyCode.S))
            {
                //bulletspawn goes back to normal
                //the "exit" time when not pressing the up key is too slow right now
                //tempCrouchShoot.SetActive(false);
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
            animator.Play("Death");
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
        animator.Play("Jump");
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
            b.transform.rotation = Quaternion.Euler(0.0f, 0.0f, -180.0f);
            b.GetComponent<Rigidbody2D>().AddForce(Vector2.left * bulletPrefab.GetComponent<BulletScript>().bulletSpeed);
        }
        else if (facingRight)
        {
            b.transform.rotation = Quaternion.Euler(0.0f, 0.0f, -0.0f);
            b.GetComponent<Rigidbody2D>().AddForce(Vector2.right * bulletPrefab.GetComponent<BulletScript>().bulletSpeed);
        }
    }

    public override void DamageCalc(int damage)
    {
        if (isInvincible) return;
        base.DamageCalc(damage);
        healthBar.SetHealth(displayedHealth);
        ElimCharacter();
        animator.PlayInFixedTime("Damage", -1, 1f);
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
            //Time.timeScale = 0f;
            LM.VictoryCheck();
        }

        if (collision.CompareTag("Checkpoint"))
        {
            if (LM.checkpointIndex <= 0 || LM.checkpointIndex == 1) LM.checkpointIndex++;
            LM.checkpoints[LM.checkpointIndex].GetComponent<SpriteRenderer>().color = Color.green;
            LM.flaggedCheckpoints[LM.checkpointIndex] = true;
            LM.CheckpointCostCheck();
            collision.GetComponent<BoxCollider2D>().enabled = false;
        }

        //Have to test further to see if this actually saves
        if (collision.CompareTag("ScrewPickUp"))
        {
            //Currency cy = FindObjectOfType<Currency>();
            LM.currency.WalletProperty += 1;
            //cy.WalletProperty += 1; //screws have a default value of 1 anyways

            LM.CheckpointCostCheck();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Enemy>() && !collision.gameObject.GetComponent<Enemy>().isBox && !onGround) //if the colliding object has the enemy script, or its children
        {
            collision.gameObject.GetComponent<Enemy>().DamageCalc(1); //arbitrary goomba damage
            collision.gameObject.GetComponent<Enemy>().ElimCharacter(); //jumping on enemies to death
        }
    }


    public override void PostDeath()
    {
        LM.GameOver();
        canMove = false; //self-explanatory but this turns off the ability to move around with the player. we can pause the gameworld too, but this way still plays enemy animations if they're still around the player
        GetComponent<CapsuleCollider2D>().enabled = false;
        Interactables interactable = FindObjectOfType<Interactables>();
        interactable.screenLock = 1;
        interactable.screenDestination--;
    }

    public virtual void Replenish()
    {
        //rb.WakeUp();
        LM.CheckpointCostCheck();
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
