using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBaseCombatController : MonoBehaviour, IActions
{
    [Header("Combat References")]
    //Combat mechanics region
    public float firerate = 2f;
    public float firerateWait = 0f;

    public GameObject bulletPrefab; //should really just use raycasts and bullet sprites tbh
    public GameObject bulletSpawn;
	public float bulletPrefabSpeed;

    public Transform StrikeZone; //The collision area that has the player's melee weapon.
    public float strikeRange = 0.5f;
    public int strikeDamage = 1;
    public LayerMask enemyLayers; //Player is the NPC's enemy and the enemy NPC is the player's... enemy

	public PlayerStateManager player;
	public Animator animator;

	public bool isAwake = true;
	public bool isChasing = false;


	public EnemyBase enemyBase;
	public EnemyBaseStats enemyBaseStats;
	public EnemyDetection enemyDetection;


	public void EnemyDropItem()
    {
        throw new System.NotImplementedException();
    }

	public virtual void Shoot()
	{
		GameObject b = Instantiate(bulletPrefab) as GameObject;
		b.transform.position = bulletSpawn.transform.position;
		if (gameObject.transform.localScale.x < 0)
		{
			b.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 90f);
			b.GetComponent<Rigidbody2D>().AddForce(Vector2.left * bulletPrefab.GetComponent<ProjectileProperties>().projectileSpeed);
		}
		else
		{
			b.transform.rotation = Quaternion.Euler(0.0f, 0.0f, -90f);
			b.GetComponent<Rigidbody2D>().AddForce(Vector2.right * bulletPrefab.GetComponent<ProjectileProperties>().projectileSpeed);
		}
	}

	public virtual void Strike() //Melee attack
	{
		//Reference for the person doing specific enemies, you would play the animation before the other logic here

		//Might have to add a check for enemy layers and opposing tags
		//Enemies in range of attack here
		Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(StrikeZone.position, strikeRange, enemyLayers);

		//The enemy in this case is the player
		//Damage calculations
		foreach (Collider2D enemy in hitEnemies)
		{
			enemy.GetComponent<PlayerStateManager>().DamageCalculation(strikeDamage);
			Debug.Log($"{gameObject.name} hit {enemy.name}");
		}

		FindObjectOfType<SFXManager>().PlayAudio("Melee");
	}

	// Start is called before the first frame update
	public virtual void Start()
    {
		//If it exists or is not null then...
		if(bulletPrefab != null)
        {
			bulletPrefabSpeed = bulletPrefab.GetComponent<ProjectileProperties>().projectileSpeed;
			enemyBase = GetComponent<EnemyBase>();
			enemyBaseStats = GetComponent<EnemyBaseStats>();
			enemyDetection = GetComponent<EnemyDetection>();
			animator = GetComponent<Animator>();
			player = FindObjectOfType<PlayerStateManager>();
		}
		else
        {
			enemyBase = GetComponent<EnemyBase>();
			enemyBaseStats = GetComponent<EnemyBaseStats>();
			enemyDetection = GetComponent<EnemyDetection>();
			animator = GetComponent<Animator>();
			player = FindObjectOfType<PlayerStateManager>();
		}
	}

    // Update is called once per frame
    void Update()
    {
        
    }

	//Did this with the camera, hopefully this works well if every character has one attached:
	void OnDrawGizmosSelected()
	{
		if (StrikeZone == null)
			return;

		Gizmos.DrawWireSphere(StrikeZone.position, strikeRange);
	}

}
