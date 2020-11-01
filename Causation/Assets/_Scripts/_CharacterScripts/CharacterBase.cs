using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterBase : MonoBehaviour, IActions
{
	private int _health; //Think of this as max health
	public int Health
	{
		get => _health;
		set => _health = value;
	}

	[Header("Health Stats")]
	public int displayedHealth; //This is the current health used for calculations and UI. should be below properties but here for clarity
	public int staminaActions;
	private int _stamina; //not float stamina, think of this for special actions like dble jump if stamina available
	public int Stamina
	{
		get { return _stamina; }
		set { _stamina = value; }
	}

	private int _ammo; //Maybe we can have the enemies reload too
	public int Ammo
	{
		get { return _ammo; }
		set { _ammo = value; }
	}

	private int _currency;
	//the amount of points that the player has 
	//AND the amount of points the enemy will drop (should stick to smaller values for drops)
	//TODO Neat idea where the size of the currency drop will determine sprite size

	public int Currency
	{
		get { return _currency; }
		set { _currency = value; }
	}

	[Header("Ground stuff")]
	public LayerMask groundLayer;
	public bool onGround = false;
	public float groundLength = 0.6f;
	public Vector3 colliderOffset;

	[Header("Combat References")]
	//Combat mechanics region
	//I think bullet damage should be defined here as well, it would make asset creation faster for enemies
	//then IF the player has unique bullets with different damage then you can use the prefab damage values...

	public GameObject bulletPrefab; //should really just use raycasts and bullet sprites tbh
	public GameObject bulletSpawn;
	public Transform StrikeZone; //The collision area that has the player's melee weapon.
	public float strikeRange = 0.5f;
	public int strikeDamage = 1;
	public LayerMask enemyLayers; //Player is the NPC's enemy and the enemy NPC is the player's... enemy

	private void Start()
	{
		Health = displayedHealth;
	}

	public virtual void DamageCalc(int damage)
	{
		displayedHealth -= damage;
		//inheriting classes can implement this differently if needed
	}

	
	public virtual void Shoot()
	{
		GameObject b = Instantiate(bulletPrefab) as GameObject;
		b.transform.position = bulletSpawn.transform.position;
		if (gameObject.transform.localScale.x < 0)
		{
			b.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 90f);
			//b.GetComponent<Rigidbody2D>().AddForce(Vector2.left * bulletSpeed);
			b.GetComponent<Rigidbody2D>().AddForce(Vector2.left * bulletPrefab.GetComponent<BulletScript>().bulletSpeed);

		}
		else
		{
			b.transform.rotation = Quaternion.Euler(0.0f, 0.0f, -90f);
			b.GetComponent<Rigidbody2D>().AddForce(Vector2.right * bulletPrefab.GetComponent<BulletScript>().bulletSpeed);
		}
	}

	public virtual void Strike() //Melee attack
	{
		//Reference for the person doing specific enemies, you would play the animation before the other logic here

		//Might have to add a check for enemy layers and opposing tags
		//Enemies in range of attack here
		Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(StrikeZone.position, strikeRange, enemyLayers);

		//Damage calculations
		foreach(Collider2D enemy in hitEnemies)
		{
			enemy.GetComponent<CharacterBase>().DamageCalc(strikeDamage);
			Debug.Log($"{gameObject.name} hit {enemy.name}");
		}

		FindObjectOfType<SFXManager>().PlayAudio("Melee");
	}

	//Did this with the camera, hopefully this works well if every character has one attached:
	void OnDrawGizmosSelected()
	{
		if (StrikeZone == null)
			return;

		Gizmos.DrawWireSphere(StrikeZone.position, strikeRange);
	}

	//Eliminate, kill, destroy, etc.
	//should be used for enemies, players will override method and do something different
	public abstract void ElimCharacter();

	public abstract void Flip(float value);

	public abstract void PostDeath();//Item drop behavior or Game over screen  
}
