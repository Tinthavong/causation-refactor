using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatController : MonoBehaviour, ICombatActions
{
    [Header("Combat Object References")]
    public GameObject projectilePrefab;
    public GameObject projectileStandingSpawn;
    public GameObject projetileCrouchSpawn;

    //References to a melee combat component that was cut out of the final game
    public Transform meleeDamageZone;
    public float meleeAttackRange = 0.5f;
    public int meleeDamage = 1;
    public LayerMask enemyLayers;

    [Header("Combat Values")]
    //Attacking
    public float attackElapsedTime = 0;
    public float attackDelay = 0.2f;
    public float fireDelay = 1f;

    [Header("Ranged Weapon Values")]
    public bool isBurstFire = false; //Firing mode of the player's current weapon
    public int shotAmount = 1; //The amount of bullets to fire

    PlayerStateManager psm;
    PlayerMovementController pmc;
    Animator animator;
    Rigidbody2D rb; //Might not be necessary for combat controls

    // Start is called before the first frame update
    void Start()
    {
        psm = GetComponent<PlayerStateManager>();
        pmc = GetComponent<PlayerMovementController>();

        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (psm.isControlling)
        {
            attackElapsedTime += Time.deltaTime;

            //Maybe switching firing modes and reloading could be its own function
            if (Input.GetKeyDown(KeyCode.T))
            {
                psm.AmmoReloadCalulcation();
                AssignFiringMode();
            }

            //Reloading could be its own function
            if (Input.GetKeyDown(KeyCode.R))
            {
                psm.AmmoReloadCalulcation();
            }

            if (!isBurstFire)
            {
                RangeAttack();
            }
            else
            {
                AlternateRangeAttack();
            }
        }
    }


    //There are only two firing modes so a bool is fine
    private bool AssignFiringMode()
    {
        if (!isBurstFire)
        {
            isBurstFire = true;
        }
        else
        {
            isBurstFire = false;
        }

        if (isBurstFire)
        {
            shotAmount = 3;
            fireDelay = 0.05f;
            attackDelay = 0.5f;
            return true;
        }
        else
        {
            shotAmount = 1;
            attackDelay = 0.2f;
            fireDelay = 1f;
            return false;
        }
        //Communicate with the statemanager to update the UI and correct ammo value
    }

    //Not designed because it was cut out for players
    public void MeleeAtack()
    {

    }

    public void RangeAttack()
    {
        if (Input.GetButtonDown("Fire1") && !psm.isOutOfAmmo && attackElapsedTime >= attackDelay && Time.timeScale > 0)
        {
            if (!psm.isCrouched)
            {
                animator.Play("Shoot");
            }
            else if (psm.isCrouched)
            {
                animator.Play("CrouchShoot");
            }
            psm.AmmoUsageCalculation();
            RangeProjectileSpawn();
            attackElapsedTime = 0;
            animator.SetBool("IsShooting", false);
        }
        else if (Input.GetButtonDown("Fire1") && psm.isOutOfAmmo)
        {
            Debug.Log("No Ammo Left!");//implement affordance, clicking sound or something
        }  
    }

    public void AlternateRangeAttack()
    {
        if (Input.GetButtonDown("Fire1") && !psm.isOutOfAmmo && attackElapsedTime >= attackDelay && Time.timeScale > 0)
        {
            if (!psm.isCrouched)
            {
                animator.Play("Shoot");
            }
            else if (psm.isCrouched)
            {
                animator.Play("CrouchShoot");
            }

            for (int i = 0; i < shotAmount; i++)
            {
                Invoke("RangeProjectileSpawn", (fireDelay * i));
                psm.AmmoUsageCalculation();
            }
            attackElapsedTime = 0;
            animator.SetBool("IsShooting", false);
        }
        else if (Input.GetButtonDown("Fire1") && psm.isOutOfAmmo)
        {
            Debug.Log("No Ammo Left!");//implement affordance, clicking sound or something
        }     
    }

    private void RangeProjectileSpawn()
    {
        if (!psm.isCrouched)
        {
            GameObject b = Instantiate(projectilePrefab) as GameObject;
            b.transform.position = projectileStandingSpawn.transform.position;
            //Bullet object shifts position and rotation based on direction
            if (!pmc.isFacingRight)
            {
                b.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 90f);
                b.GetComponent<Rigidbody2D>().AddForce(Vector2.left * projectilePrefab.GetComponent<ProjectileProperties>().projectileSpeed);
            }
            else if (pmc.isFacingRight)
            {
                b.transform.rotation = Quaternion.Euler(0.0f, 0.0f, -90f);
                b.GetComponent<Rigidbody2D>().AddForce(Vector2.right * projectilePrefab.GetComponent<ProjectileProperties>().projectileSpeed);
            }
        }
        else
        {
            GameObject b = Instantiate(projectilePrefab) as GameObject;
            b.transform.position = projetileCrouchSpawn.transform.position;
            //Bullet object shifts position and rotation based on direction
            if (!pmc.isFacingRight)
            {
                b.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 90f);
                b.GetComponent<Rigidbody2D>().AddForce(Vector2.left * projectilePrefab.GetComponent<ProjectileProperties>().projectileSpeed);
            }
            else if (pmc.isFacingRight)
            {
                b.transform.rotation = Quaternion.Euler(0.0f, 0.0f, -90f);
                b.GetComponent<Rigidbody2D>().AddForce(Vector2.right * projectilePrefab.GetComponent<ProjectileProperties>().projectileSpeed);
            }
        }
    }

    //Not designed, cut out of game. Could potentially utilize for additional polish
    public void SpecialAttack()
    {
        //Switching fire lets player shoot 3 bullets or whatever
        throw new System.NotImplementedException();
    }

}
