using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour, ICharacterStatesCalculations
{
    [Header("Player States")]
    //Movement states
    public bool isCrouched = false;

    //Player combat states
    public bool isOutOfAmmo;

    //Player gameplay loop states
    //Utilized when the player resumes gameplay after dialogue, pausing, game overs.
    public bool isControlling = true;

    //Statemanager communicates with stats to do calculations then sends information to the levelmanager to indicate game progress (IE, game over, game win, etc)
    PlayerBaseStats charBaseStats; //Player's current character and its stats
    LevelManager LM;

    //The healthbar UI game object that will display the player's heatlh
    public HealthBar healthbar;


    [Header("Player Contact Damage References")]
    SpriteRenderer render;
    Color c;
    public float invulnerabilityTime = 1f;

    public int currentAmmoIndex = 0; //default


    // Start is called before the first frame update
    void Start()
    {
        if (Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
        }

        isControlling = true; //For now set it true at the top
        charBaseStats = GetComponent<PlayerBaseStats>();
        healthbar.SetMaxHealth(charBaseStats.CharacterHealth);
        render = GetComponent<SpriteRenderer>();
        c = render.material.color;
        LM = FindObjectOfType<LevelManager>();
    }

    /*
     void AmmoTypeAssign()
    if ammo type is == 0
    ammotypeindex++
     -----------------------


    if ammo type is >= characterammo.length
    ammotypeindex = 0
     
     */

    public void AmmoState()
    {
        //With the ammo array it would be charBaseStats.CharacterAmmo[ammoTypeIndex]
        if (charBaseStats.CharacterAmmo <= 0)
        {
            isOutOfAmmo = true;
        }
        else
        {
            isOutOfAmmo = false;
        }
    }

    public void AmmoReloadCalulcation()
    {
        if (charBaseStats.CharacterAmmo < charBaseStats.maxCharacterAmmo)
        {
            charBaseStats.CharacterAmmo += charBaseStats.shotsFired;
            charBaseStats.shotsFired = 0;
            AmmoState();
        }
    }

    //Shots fired!
    public void AmmoUsageCalculation()
    {
        if (charBaseStats.shotsFired < charBaseStats.maxCharacterAmmo && !isOutOfAmmo)
        {
            charBaseStats.shotsFired++;
            charBaseStats.CharacterAmmo--;
            AmmoState();
            Debug.Log(charBaseStats.CharacterAmmo);
        }
        else if (charBaseStats.shotsFired >= charBaseStats.maxCharacterAmmo && isOutOfAmmo)
        {
            //Player state set to out of ammo
            AmmoState();
            Debug.Log("Out of ammo!");
        }
    }

    public void DamageCalculation(int damageValue)
    {
        StartCoroutine("GetInvulnerable");
        charBaseStats.CharacterHealth -= damageValue;
        healthbar.UpdateHealthBar(charBaseStats.CharacterHealth);
    }

    public void ContactDamageCalculation(int touchDamageValue)
    {
        StartCoroutine("GetInvulnerable");
        charBaseStats.CharacterHealth -= touchDamageValue;
       // gameObject.GetComponent<Animator>().Play("Damage");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Enemy":
                ContactDamageCalculation(1);
                break;

            case "Boss":
                ContactDamageCalculation(2);
                break;

            default:
                break;
        }
    }

    public bool IsDead()
    {
        if (charBaseStats.CharacterHealth <= 0)
        {
            isControlling = false;
            GetComponent<Animator>().SetBool("IsDead", true);
            LM.GameOver();
            return true;
        }
        else
        {
            GetComponent<Animator>().SetBool("IsDead", false);
            return false;
        }
    }

    //Enumerator to be used with coroutines to trigger damage and invulnerability periods.
    IEnumerator GetInvulnerable()
    {
        Physics2D.IgnoreLayerCollision(10, 11, true); //player and enemy
        Physics2D.IgnoreLayerCollision(10, 17, true); //player abd bullet
        c.a = 0.5f;
        render.material.color = c;
        gameObject.GetComponent<Animator>().SetBool("IsDamaged", true);
        yield return new WaitForSeconds(invulnerabilityTime);
        gameObject.GetComponent<Animator>().SetBool("IsDamaged", false);
        Physics2D.IgnoreLayerCollision(10, 11, false); //player and enemy
        Physics2D.IgnoreLayerCollision(10, 17, false); //player abd bullet
        c.a = 1f;
        render.material.color = c;
    }

    //Planned function for extended healing actions
    public void HealCalculation(int healValue)
    {
        /*
            if (charBaseStats.CharacterHealth < charBaseStats.maxCharacterHealth)
            {
                charBaseStats.CharacterHealth += healValue;
            }
        */
    }

    //This functions replenishes the player to restore them to a point before game over
    //The ability to replenish is determined by the player's score
    public void Replenish()
    {
        LM.CheckpointCostCheck();
        //rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        GetComponent<Animator>().Play("Idle");
        charBaseStats.CharacterHealth = charBaseStats.maxCharacterHealth;
        healthbar.UpdateHealthBar(charBaseStats.CharacterHealth);
        IsDead();
        charBaseStats.CharacterAmmo = charBaseStats.maxCharacterAmmo;

        isControlling = true;
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        GetComponent<CapsuleCollider2D>().enabled = true;
        StartCoroutine("GetInvulnerable");
    }
}
