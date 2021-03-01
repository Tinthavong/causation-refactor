using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    [Header("Enemy Detection Variables")]
    public int sightRange = 10;
    public int meleeRange = 2;
    public bool ignoresVerticalSightRestriction = false; //Used to determine if an enemy will react to players if they arent on the same y level
    public float verticalSight = 2.5f;

    public int fireRange = 5; //Shotgunner and drones

    private PlayerStateManager player; //this can be private, pretty sure this works now

    EnemyBaseMovement enemyBase;

    [Header("Enemy States")]
    //Combat states
    public bool isAttacking;
    public bool isOutOfAmmo; //Not necessary?

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerStateManager>();
        enemyBase = GetComponent<EnemyBaseMovement>();
    }

    //Checks to see if the player object is within a certain distance
    public bool IsClose()//shouldve made these public long time ago, but they are now
    {
        if (Math.Abs(player.transform.position.x - this.gameObject.transform.position.x) < sightRange)
        {
            return true;
        }
        else if (player.IsDead())
        {
            return false;
        }
        else
        {
            return false;
        }
    }

    //Used for melee support
    public bool IsTooClose()//shouldve made these public long time ago, but they are now
    {
        if (Math.Abs(player.transform.position.x - this.gameObject.transform.position.x) < meleeRange && !player.IsDead())
        {
            return true;
        }
        else if (player.IsDead())
        {
            return false;
        }
        return false;
    }

    public bool VertRangeSeesPlayer()
    {
        if (ignoresVerticalSightRestriction)
        {
            return true;
        }
        else if (Math.Abs(transform.position.y - player.transform.position.y) <= verticalSight)
        {
            return true;
        }
        else if (player.IsDead())
        {
            return false;
        }
        else
        {
            return false;
        }
    }

    public bool IsChasing()
    {
        if (IsClose() && !IsTooClose() && enemyBase.onGround)
        {
            return true;
        }
        else if (player.IsDead())
        {
            return false;
        }
        else
        {
            return false;
        }
    }
    
    //Is *MeleeAttacking
    public bool IsAttacking()
    {
        if (IsTooClose() && !player.IsDead() && VertRangeSeesPlayer())
        {
            return true;
        }
        else if(player.IsDead())
        {
            return false;
        }
        else
        {
            return false;
        }
    }

    public bool IsShooting()
    {
        if (IsClose() && !player.IsDead() && VertRangeSeesPlayer())
        {
            return true;
        }
        else if (player.IsDead())
        {
            return false;
        }
        else
        {
            return false;
        }
    }

    public bool IsCloseEnough()
    {
        if (Math.Abs(player.transform.position.x - this.gameObject.transform.position.x) <= fireRange && !player.IsDead())
        {
            return true;
        }
        else if (player.IsDead())
        {
            return false;
        }
        return false;
    }
}
