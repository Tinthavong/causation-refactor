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

	[Header("Bullet References")]
	//Shooting mechanics region
	public float bulletSpeed = 400f;
	public GameObject bulletPrefab; //should really just use raycasts and bullet sprites tbh
	public GameObject bulletSpawn;

	private void Start()
	{
		displayedHealth = Health;
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
			Debug.Log($"Destroyed {gameObject.name}");
			Destroy(gameObject);
		}
	}


	public void Shoot()
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
	public abstract void Flip(float value);

	public abstract void PostDeath();//Item drop behavior or Game over screen  
}
