using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateManager : MonoBehaviour, ICharacterStatesCalculations
{
    [Header("Enemy States")]
    //Movement states
    public bool isJumping;
    public bool isCrouched = false;
    public bool isFacingRight = true;

    EnemyBaseStats enemyBaseStats;
    public float recoveryTime = 0.25f;

    //Boss HP Bar
    public HealthBar bossHealthBar;

    public void AmmoReloadCalulcation()
    {
        throw new System.NotImplementedException();
    }

    public void AmmoState()
    {
        throw new System.NotImplementedException();
    }

    public void AmmoUsageCalculation()
    {
        throw new System.NotImplementedException();
    }

    //UI Reference, hopefully this counts as a manager and a calculation and isn't out of place
    //HealthBar healthbar; 

    public void DamageCalculation(int damageValue)
    {
        if(gameObject.CompareTag("Boss"))
        {

            enemyBaseStats.CharacterHealth -= damageValue;
            bossHealthBar.UpdateHealthBar(enemyBaseStats.CharacterHealth);
            IsDead();
        }
        else
        {
            enemyBaseStats.CharacterHealth -= damageValue;
            gameObject.GetComponent<Animator>().SetBool("IsDamaged", true);
            Invoke("RecoverFromDamageAnimation", recoveryTime);
            IsDead();
        }
    }


    public void HealCalculation(int healValue)
    {

    }

    private bool IsDead()
    {
        if (enemyBaseStats.CharacterHealth <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void RecoverFromDamageAnimation()
    {
        //gameObject.GetComponent<Animator>().SetBool("IsChasing", true);
        gameObject.GetComponent<Animator>().SetBool("IsDamaged", false);
    }

    // Start is called before the first frame update
    void Start()
    {
        enemyBaseStats = GetComponent<EnemyBaseStats>();
        if (gameObject.CompareTag("Boss"))
        {
            bossHealthBar.SetMaxHealth(enemyBaseStats.CharacterHealth);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
