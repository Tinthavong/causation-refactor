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

	[Header("Combat References")]
	//Combat mechanics region
	//I think bullet damage should be defined here as well, it would make asset creation faster for enemies
	//then IF the player has unique bullets with different damage then you can use the prefab damage values...
	public float bulletSpeed = 400f;
	public GameObject bulletPrefab; //should really just use raycasts and bullet sprites tbh
	public GameObject bulletSpawn;
	public Transform StrikeZone; //The collision area that has the player's melee weapon.
	public float strikeRange = 0.5f;
	public int strikeDamage = 1;
	public LayerMask enemyLayers; //Player is the NPC's enemy and the enemy NPC is the player's... enemy
	public float enemySpeed = 8f; //This will control how fast an enemy moves, change in prefabs for each enemy type

	private void Start()
	{
		Health = displayedHealth;
	}

	public virtual void DamageCalc(int damage)
	{
		displayedHealth -= damage;
		//inheriting classes can implement this differently if needed
	}

	//Eliminate, kill, destroy, etc.
	//should be used for enemies, players will override method and do something different
	public virtual void ElimCharacter()
	{
		//this might be too simple but for now checking if the health is at or below 0 might be enough
		if (displayedHealth <= 0)
		{
			PostDeath();
			//should avoid outright destroying the characters bc it should do an animation or whatever first, should use coroutine to delay this but for now:
			if (gameObject.CompareTag("Enemy"))
			{
				Debug.Log($"Destroyed {gameObject.name}");
				Destroy(gameObject); //I think it's best to not destroy the player object if there's going to be a death animation but we'll figure it out later - Tim
			}
			else
			{
				Debug.Log("Player death");
			}
		}
	}

	public virtual void Shoot()
	{
		//This block of code is why we should use raycasts 
		GameObject b = Instantiate(bulletPrefab) as GameObject;
		b.transform.position = bulletSpawn.transform.position;
		//Bullet object shifts position and rotation based on direction
		if (gameObject.transform.localScale.x < 0)
		{
			b.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 90f);
			b.GetComponent<Rigidbody2D>().AddForce(Vector2.left * bulletSpeed);
		}
		else
		{
			b.transform.rotation = Quaternion.Euler(0.0f, 0.0f, -90f);
			b.GetComponent<Rigidbody2D>().AddForce(Vector2.right * bulletSpeed);
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


	public abstract void Flip(float value);

	public abstract void PostDeath();//Item drop behavior or Game over screen  
}
