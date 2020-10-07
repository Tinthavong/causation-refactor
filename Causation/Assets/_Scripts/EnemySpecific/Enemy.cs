using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : CharacterBase
{
    public Enemy() //constructor
    {
        Health = displayedHealth; //Displayed Health can be set in the inspector
        Stamina = staminaActions;
        //Enemies dont use ammo for now but if it breaks just set the amount here
        Currency = dropValue;   
    }
    
    [Header("Enemy Drops")]
    //Consider making these private and serialized
    public int dropValue;
    public GameObject drop; //Can drop items or money, just drops money for now
    public PlayerController player; //this can be private, pretty sure this works now

    //Temp Shooting behavior
    public float firerate = 2f;
    private float firerateWait = 0f;
    bool facingRight;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        //This should use an actual find method/algorithm instead of just knowing where the player is
    }

    // Update is called once per frame
    void Update()
    {
        //firerateWait changes based on fps time
        firerateWait -= Time.deltaTime;
        //if firerateWait is 0, time to fire and reset the wait
        if (firerateWait <= 0)
        {
            Shoot();
            firerateWait = firerate;
        }

        ElimCharacter();
        //Controls where the enemy is looking (currently looks at player at all times)
        Flip(0f);
    }

    public override void Flip(float dump) //dump because it doesn't matter but it's needed or errors
    {
        if (player.transform.position.x < this.transform.position.x && !facingRight)
        {
            facingRight = true;
            Vector3 scale = transform.localScale;
            scale.x *= -1;

            transform.localScale = scale;
        }
        else if (player.transform.position.x >= this.transform.position.x && facingRight)
        {
            facingRight = false;
            Vector3 scale = transform.localScale;
            scale.x *= -1;

            transform.localScale = scale;
        }
    }

    public override void PostDeath()
    {
        //Temporary death, needs animation and drops
        GameObject d = Instantiate(drop) as GameObject;
        d.transform.position = this.transform.position;
    }

    void Death()
    {

        Destroy(this.gameObject);
    }
}
