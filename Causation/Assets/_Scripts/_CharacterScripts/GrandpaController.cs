using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GrandpaController : PlayerController
{
    //controller specificallty for the grandpa because of his bullet swappng mechanic

    public GrandpaController() //Constructor
    {
        Health = displayedHealth;
        Stamina = staminaActions;
        Ammo = maxAmmo; //To avoid confusion, the Ammo property is like the previous currentAmmo variable
                        // Currency = walletValue;
    }

    [Header("Components")]
    public GameObject[] bulletList;
    private int bulletTypeCount;
    public int bulletIndex = 0;
    //public int maxAmmo = 6; //six-shooter by default, set in inspector otherwise for burstfire
    private int shotsFired = 0;
    public int[] remainingAmmo; //need to refactor and fix naming for some of these variables

    void Awake()
    {
        if (Time.timeScale <= 0f)
        {
            Time.timeScale = 1f;
        }

        bulletTypeCount = (bulletList.Length); //Sets the maximum amount of varied bullets depending the prefabs set in the inspector
        bulletPrefab = bulletList[0]; //The default bullet is 0, hardcoding that should be okay
        WeaponTypeAssign(); //redesigned bullet swapping so this has to change

        healthBar.SetHealth(displayedHealth);
        LM = FindObjectOfType<LevelManager>();

        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        playerLayer = LayerMask.NameToLayer("Player");
        platformLayer = LayerMask.NameToLayer("Platform");
    }

    private void ChangeBullets()
    {
        shotsFired = 0;
        switch (bulletIndex)
        {
            //this cycles around that's why it seems backwards
            case 0: //blue
                bulletIndex++;
                ammoDisplay.sprite = bulletList[bulletIndex].GetComponent<SpriteRenderer>().sprite;
                bulletPrefab = bulletList[bulletIndex];
                Debug.Log(remainingAmmo[bulletIndex]);

                if (remainingAmmo[bulletIndex] >= maxAmmo)
                {
                    Ammo = maxAmmo;
                    ammoTotalText.text = remainingAmmo[bulletIndex].ToString();
                }

                if (remainingAmmo[bulletIndex] <= 0)
                {
                    Ammo = 0; //that is to say, 0
                    ammoTotalText.text = "0";
                }
                break;

            case 1: //red
                bulletIndex++;
                ammoDisplay.sprite = bulletList[bulletIndex].GetComponent<SpriteRenderer>().sprite;
                bulletPrefab = bulletList[bulletIndex];
                Debug.Log(remainingAmmo[bulletIndex]);
                if (remainingAmmo[bulletIndex] >= maxAmmo)
                {
                    Ammo = maxAmmo;
                    ammoTotalText.text = remainingAmmo[bulletIndex].ToString();
                }

                if (remainingAmmo[bulletIndex] <= 0)
                {
                    Ammo = 0; //that is to say, 0
                    ammoTotalText.text = "0";
                }
                break;

            case 2: //green
                bulletIndex++;
                ammoDisplay.sprite = bulletList[bulletIndex].GetComponent<SpriteRenderer>().sprite;
                bulletPrefab = bulletList[bulletIndex];
                Debug.Log(remainingAmmo[bulletIndex]);
                if (remainingAmmo[bulletIndex] >= maxAmmo)
                {
                    Ammo = maxAmmo;
                    ammoTotalText.text = remainingAmmo[bulletIndex].ToString();
                }

                if (remainingAmmo[bulletIndex] <= 0)
                {
                    //The basic logic is that once the amount of ammo left is less than zero
                    //then the ammo and the text will be set to 0 as a safety measure but this doesn't take into
                    //account if you have one bullet left or whatever
                    Ammo = 0; //that is to say, 0
                    ammoTotalText.text = "0";
                }
                break;

            case 3: //default
                bulletIndex = 0; //last bullet so far, goes back to the first bullet in the index
                ammoDisplay.sprite = bulletList[bulletIndex].GetComponent<SpriteRenderer>().sprite;
                bulletPrefab = bulletList[bulletIndex];

                /*
                if (remainingAmmo[bulletIndex] >= maxAmmo)
                {
                    Ammo = maxAmmo;
                    ammoTotalText.text = "0";
                }*/

                break;

            default:

                break;
        }
    }

    private void ReloadBullets()
    {
        switch (bulletIndex)
        {
            case 0: //default
                Ammo = maxAmmo;
                ammoTotalText.text = "∞"; //just in case

                shotsFired = 0;
                break;

            case 1: //blue
                remainingAmmo[bulletIndex] = remainingAmmo[bulletIndex] - shotsFired; //decrements the total rounds left in the "inventory"
                //right now it only substracts shots fired without taking into account having 0 bullets then picking rounds up
                if (remainingAmmo[bulletIndex] >= maxAmmo) //safety check, if the remaining ammo is more than or equal to the maximum possible ammo of (example) 6 then
                {
                    Ammo = maxAmmo; //ammo can safely be set to the maximum amount possible for the magazine or chamber
                    ammoTotalText.text = remainingAmmo[bulletIndex].ToString(); //updates the total remaining bullets left in the UI (after subtracting the shots fired
                }
                else if (remainingAmmo[bulletIndex] < maxAmmo) //if the remaining ammo is less than the maximum possible ammo then
                {
                    Ammo = maxAmmo;
                    if (remainingAmmo[bulletIndex] > 0)
                    {
                        ammoTotalText.text = remainingAmmo[bulletIndex].ToString();
                    }
                    if (remainingAmmo[bulletIndex] < 0) //basically ensures that the ammototal text should never be a negative value
                    {
                        ammoTotalText.text = "0";
                    }
                }

                shotsFired = 0;
                break;

            case 2: //red
                remainingAmmo[bulletIndex] = remainingAmmo[bulletIndex] - shotsFired; //decrements the total rounds left in the "inventory"

                if (remainingAmmo[bulletIndex] >= maxAmmo) //safety check, if the remaining ammo is more than or equal to the maximum possible ammo of (example) 6 then
                {
                    Ammo = maxAmmo; //ammo can safely be set to the maximum amount possible for the magazine or chamber
                    ammoTotalText.text = remainingAmmo[bulletIndex].ToString(); //updates the total remaining bullets left in the UI (after subtracting the shots fired
                }
                else if (remainingAmmo[bulletIndex] < maxAmmo) //if the remaining ammo is less than the maximum possible ammo then
                {
                    Ammo = maxAmmo;
                    if (remainingAmmo[bulletIndex] > 0)
                    {
                        ammoTotalText.text = remainingAmmo[bulletIndex].ToString();
                    }
                    if (remainingAmmo[bulletIndex] < 0) //basically ensures that the ammototal text should never be a negative value
                    {
                        ammoTotalText.text = "0";
                    }
                }

                shotsFired = 0;
                break;

            case 3: //green
                remainingAmmo[bulletIndex] = remainingAmmo[bulletIndex] - shotsFired; //decrements the total rounds left in the "inventory"

                if (remainingAmmo[bulletIndex] >= maxAmmo) //safety check, if the remaining ammo is more than or equal to the maximum possible ammo of (example) 6 then
                {
                    Ammo = maxAmmo; //ammo can safely be set to the maximum amount possible for the magazine or chamber
                    ammoTotalText.text = remainingAmmo[bulletIndex].ToString(); //updates the total remaining bullets left in the UI (after subtracting the shots fired
                }
                else if (remainingAmmo[bulletIndex] < maxAmmo) //if the remaining ammo is less than the maximum possible ammo then
                {
                    Ammo = maxAmmo;
                    if (remainingAmmo[bulletIndex] > 0)
                    {
                        ammoTotalText.text = remainingAmmo[bulletIndex].ToString();
                    }
                    if (remainingAmmo[bulletIndex] < 0) //basically ensures that the ammototal text should never be a negative value
                    {
                        ammoTotalText.text = "0";
                    }
                }

                shotsFired = 0;
                break;

            default:
                break;
        }
    }

    private void WeaponTypeAssign() //exclusive method for grandpa so he starts with 6 bullets instead of 15
    {
        //assigns ammo capacity, weapon firing type
        //if revolver ammo is 6 if smg ammo is 15
        //also set attack delay here?
        //needs to be heavily refined
        maxAmmo = 6;
        Ammo = maxAmmo;
        ammoText.text = Ammo.ToString();
        remainingAmmo = new int[bulletList.Length];

        remainingAmmo[0] = 9999999; //default
        remainingAmmo[1] = 0; // blue
        remainingAmmo[2] = 0; // red
        remainingAmmo[3] = 0; // green

        //get rid of this
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
                if (!isCrouched) animator.Play("Shoot");
                for (int i = 0; i < shotAmount; i++)
                {
                    Invoke("Shoot", (fireDelay * i));
                    shotsFired++;
                    Ammo--;
                }
                attackElapsedTime = 0;
                animator.SetBool("IsShooting", false);
            }

            else if (Input.GetButtonDown("Fire1") && Ammo == 0)
            {
                Debug.Log("No Ammo Left!");//implement affordance, clicking sound or something
            }

            if (Input.GetKeyDown(KeyCode.R) && remainingAmmo[bulletIndex] > 0)
            {
                ReloadBullets();
                FindObjectOfType<SFXManager>().PlayAudio("Reload");
            }
            else if (Input.GetKeyDown(KeyCode.R) && remainingAmmo[bulletIndex] <= 0)
            {
                Debug.Log("No more ammo of type" + bulletList[bulletIndex].name);
            }
        }
    }

    private void Update()
    {
        onGround = Physics2D.Raycast(transform.position + colliderOffset, Vector2.down, groundLength, groundLayer) || Physics2D.Raycast(transform.position - colliderOffset, Vector2.down, groundLength, groundLayer);
        if (canMove)
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                ChangeBullets();
            }

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
                    //tempCrouchShoot.SetActive(true);
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
            MoveCharacter(direction.x);
        }

        if (jumpTimer > Time.time && onGround && !isCrouched)
        {
            Jump();
        }

        ModifyPhysics();
    }

    void MoveCharacter(float horizontal)
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

    void ModifyPhysics()
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
            if (LM.checkpointIndex <= 0) LM.checkpointIndex++;
            LM.checkpoints[LM.checkpointIndex].GetComponent<SpriteRenderer>().color = Color.green;
            LM.flaggedCheckpoints[LM.checkpointIndex] = true;
            LM.CheckpointCostCheck();
            collision.GetComponent<BoxCollider2D>().enabled = false;
        }

        //Have to test further to see if this actually saves
        if (collision.CompareTag("ScrewPickUp"))
        {
            Currency cy = FindObjectOfType<Currency>();
            cy.WalletProperty += 1; //screws have a default value of 1 anyways

            //LM.currency.WalletProperty += 1;

            LM.CheckpointCostCheck();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Enemy>() && !collision.gameObject.GetComponent<Enemy>().isBox && !onGround) //if the colliding object has the enemy script, or its children
        {
            collision.gameObject.GetComponent<Enemy>().DamageCalc(1); //arbitrary goomba damage
        }

        //This can't be the best way to do this, gotta go back and fix later.
        if (collision.gameObject.CompareTag("BlueBulletItem"))
        {
            ammoTotalText.text = remainingAmmo[1].ToString();
        }
        else if (collision.gameObject.CompareTag("RedBulletItem"))
        {
            ammoTotalText.text = remainingAmmo[2].ToString();
        }
        else if (collision.gameObject.CompareTag("GreenBulletItem"))
        {
            ammoTotalText.text = remainingAmmo[3].ToString();
        }
    }

    public override void PostDeath()
    {
        LM.GameOver();
        canMove = false; //self-explanatory but this turns off the ability to move around with the player. we can pause the gameworld too, but this way still plays enemy animations if they're still around the player
        GetComponent<CapsuleCollider2D>().enabled = false; //Dirty fix right now. The enemy should stop attacking if the player is dead anyways
        Interactables interactable = FindObjectOfType<Interactables>();
        if(interactable.screenTransitions.Length > 0)
        {
            interactable.screenLock = 1;
            interactable.bossFlag = false;
        }
    }

    public override void Replenish()
    {
        //rb.WakeUp();
        LM.CheckpointCostCheck();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        animator.SetBool("IsDead", false);
        displayedHealth = Health;
        healthBar.SetHealth(displayedHealth);

        bulletIndex = 0; //last bullet so far, goes back to the first bullet in the index
        ammoDisplay.sprite = bulletList[bulletIndex].GetComponent<SpriteRenderer>().sprite;
        bulletPrefab = bulletList[bulletIndex];
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
