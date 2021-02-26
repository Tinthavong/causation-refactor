using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatController : MonoBehaviour, ICombatActions
{
    [Header("Combat Object References")]
    //Combat mechanics region
    //I think bullet damage should be defined here as well, it would make asset creation faster for enemies
    //then IF the player has unique bullets with different damage then you can use the prefab damage values...

    public GameObject projectilePrefab; //should really just use raycasts and bullet sprites tbh
    public GameObject projectileStandingSpawn;
    public GameObject projetileCrouchSpawn;

    public Transform meleeDamageZone; //The collision area that has the player's melee weapon.
    public float meleeAttackRange = 0.5f;
    public int meleeDamage = 1;
    public LayerMask enemyLayers; //Player is the NPC's enemy and the enemy NPC is the player's... enemy

    [Header("Combat Values")]
    //Attacking
    public float attackElapsedTime = 0;
    public float attackDelay = 0.2f;
    public float fireDelay = 1f;

    [Header("Ranged Weapon Values")]
    public bool isBurstFire = false; //false = default revolver true = burst fire SMG
    public int shotAmount = 1; //shoots 1 bullet by default, changes for burst fire

    PlayerStateManager psm;
    PlayerMovementController pmc;
    PlayerBaseStats charBaseStats; //Make this reference the interface not the class? //Also, this is used for referencing player ammo
    Animator animator;
    Rigidbody2D rb; //Might not be necessary for combat controls

    // Start is called before the first frame update
    void Start()
    {
        psm = GetComponent<PlayerStateManager>();
        pmc = GetComponent<PlayerMovementController>();

        charBaseStats = GetComponent<PlayerBaseStats>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(psm.isControlling)
        {
            attackElapsedTime += Time.deltaTime;
            RangeAttack();
        }
    }

    private void AssignFiringMode()
    {
        if (isBurstFire)
        {
            shotAmount = 3;
            //Set the new max ammo here
            fireDelay = 0.05f;
            attackDelay = 0.5f;
        }
        else if (!isBurstFire)
        {
            shotAmount = 1;
            //Set the new max ammohere
        }
        //Ammo = maxAmmo;
        //Send data to the UI? Would prefer not to have ANY UI references outside of the UI Manager/State manager
        return;
    }

    //Not fully designed because it was cut out for players
    public void MeleeAtack()
    {
     
    }

    public void RangeAttack()
    {
        if (Input.GetButtonDown("Fire1") && !psm.isOutOfAmmo && attackElapsedTime >= attackDelay && Time.timeScale > 0) //Not good, but for now
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

        //Reloading must be its own function, but in the interest of making this not TOO simple  make it encompass changing ammo as well.
        if (Input.GetKeyDown(KeyCode.R))
        {
            psm.AmmoReloadCalulcation();
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

    //Not yet designed
    public void SpecialAttack()
    {
        //Switching fire lets player shoot 3 bullets or whatever
        throw new System.NotImplementedException();
    }

}
