using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBaseMovement : MonoBehaviour
{
	//This internal class controls each droppable item set in the inspector (Set in prefabs unless it's a special enemy)
	[Serializable]
	public class EnemyDrops
	{
		public GameObject drop;
		public int weight;
		public EnemyDrops(GameObject d, int w)
		{
			drop = d;
			weight = w;
		}
	}

	[Header("Enemy Drops")]
	//Consider making these private and serialized
	public int dropValue;
	//List of EnemyDrops (explained above) to allow for multiple drops
	public List<EnemyDrops> drops;
	private int dropOnce = 1; //Quick bandaid fix for making sure items are only dropped once

	[Header("Ground Check")]
	public LayerMask groundLayer;
	public bool onGround = false;
	public float groundLength = 0.6f;
	public Vector3 colliderOffset;

	
	[Header("Enemy Rerefences")]
	public int startingHealth;
	public Vector3 startingLocation; //the enemies start location is stored here - used for moving them back after the player respawns
	public float enemySpeed = 8f; //This will control how fast an enemy moves, change in prefabs for each enemy type
	public float bulletRefSpeed;

	[HideInInspector]
	public bool facingRight;
	public bool isChasing = false;
	public bool isActive;

	EnemyBaseStats ebs;
	public Vector2 direction;

	public PlayerStateManager player; //this can be private, pretty sure this works now
	//public Animator animator;
	public Rigidbody2D rb;


	[Header("Physics Behavior")]
	public float linearDrag = 4f;
	public float gravity = 1f;
	public float fallMultiplier = 5f;

	private void Start()
	{
		ebs = GetComponent<EnemyBaseStats>();
		player = FindObjectOfType<PlayerStateManager>();

		startingLocation = gameObject.transform.localPosition;
		startingHealth = ebs.CharacterHealth;
		rb = GetComponent<Rigidbody2D>();
		//animator = GetComponent<Animator>();
	}

	public void Flip() //dump because it doesn't matter but it's needed or errors
	{
		if (player.transform.position.x < this.transform.position.x && !facingRight)
		{
			facingRight = true;
			Vector3 scale = transform.localScale;
			scale.x *= -1;
			colliderOffset.x *= -1;

			transform.localScale = scale;
		}
		else if (player.transform.position.x >= this.transform.position.x && facingRight)
		{
			facingRight = false;
			Vector3 scale = transform.localScale;
			scale.x *= -1;
			colliderOffset.x *= -1;

			transform.localScale = scale;
		}
	}

	public void RunTowards()
	{
		if (facingRight && onGround)
		{
			Vector2 movement = new Vector2(-enemySpeed, 0.0f);
			if (rb.velocity.x <= -enemySpeed)
			{
				direction = new Vector2(rb.velocity.x, rb.velocity.y);
				rb.velocity = movement;
			}
			else if (rb.velocity.x >= -enemySpeed)
			{
				direction = new Vector2(rb.velocity.x, rb.velocity.y);
				rb.AddForce(movement);
			}
		}
		else if (onGround)
		{
			Vector2 movement = new Vector2(enemySpeed, 0.0f);
			if (rb.velocity.x >= -enemySpeed)
			{
				rb.velocity = movement;
			}
			else if (rb.velocity.x <= enemySpeed)
			{
				rb.AddForce(movement);
			}
		}
	}

	public void ModifyPhysics()
	{
		bool changingDirection = (direction.x > 0 && rb.velocity.x < 0) || (direction.x < 0 && rb.velocity.x > 0);

		if (onGround)
		{
			if (Mathf.Abs(direction.x) < 0.4f || changingDirection)
			{
				rb.drag = linearDrag;
			}
			else
			{
				rb.drag = 0f;
			}
			rb.gravityScale = 0;
		}
		else
		{
			rb.gravityScale = gravity;
			rb.drag = linearDrag * 0.15f;
			if (rb.velocity.y < 0)
			{
				rb.gravityScale = gravity * fallMultiplier;
			}
			else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
			{
				rb.gravityScale = gravity * (fallMultiplier / 2);
			}
		}
	}

	public void ElimCharacter()
	{
		//this might be too simple but for now checking if the health is at or below 0 might be enough
		if (ebs.CharacterHealth <= 0 && !gameObject.GetComponent<PoliceDrone>())
		{
			EnemyDropItem();
			//should avoid outright destroying the characters bc it should do an animation or whatever first, should use coroutine to delay this but for now:

			Debug.Log($"Destroyed {gameObject.name}");
			gameObject.GetComponent<Animator>().SetBool("IsAttacking", false);
			gameObject.GetComponent<Animator>().SetBool("IsChasing", false);
			gameObject.GetComponent<Animator>().SetBool("IsDead", true);

			gameObject.GetComponent<EnemyDetection>().enabled = false;
			gameObject.GetComponent<EnemyBaseCombatController>().isAwake = false;
			gameObject.GetComponent<EnemyBaseCombatController>().isChasing = false;
			gameObject.GetComponent<EnemyBaseCombatController>().enabled = false;
			gameObject.GetComponent<Animator>().Play("Death");
			gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
			gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
		}
		else if (ebs.CharacterHealth <= 0 && gameObject.GetComponent<PoliceDrone>())
		{
			Destroy(gameObject);
		}
	}

	//Overridden elimcharacter method to account for unique animation names - can be standardized to allow for inheritance
	public void EliminateBoss()
	{
		//this might be too simple but for now checking if the health is at or below 0 might be enough
		if (ebs.CharacterHealth <= 0)
		{
			//should avoid outright destroying the characters bc it should do an animation or whatever first, should use coroutine to delay this but for now:

			gameObject.GetComponent<Animator>().SetBool("IsLasering", false);
			gameObject.GetComponent<Animator>().SetBool("IsShooting", false);
			gameObject.GetComponent<Animator>().SetBool("IsMoving", false);
			gameObject.GetComponent<Animator>().SetTrigger("IsDead");
			gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
		//	gameObject.GetComponent<BoxCollider2D>().enabled = false;
			gameObject.GetComponent<EnemyBaseCombatController>().enabled = false;
			Invoke("BossDeath", 0.5f);
		}
	}

	public void BossDeath()
	{
		LevelManager LM = FindObjectOfType<LevelManager>();
		LM.VictoryCheck();
		//Turn off HUD stuff like healthbar
	}

	public void EnemyDropItem()
	{
		//Variables for the dynamic drop search
		GameObject drop;
		int totalweight = 0;
		int rand;
		int finder = 0;
		System.Random random = new System.Random();

		//Adds up total weight for use in RNG
		foreach (EnemyDrops dr in drops)
		{
			totalweight += dr.weight;
		}
		//+1 because .next returns a random int less than the provided number
		rand = random.Next(totalweight + 1);

		//Goes through each drop in the drops list, adds its weight and checks if it passed rand
		//If it did, then that is the object that will drop on death
		if (dropOnce > 0)
		{
			dropOnce = 0;
			foreach (EnemyDrops dr in drops)
			{
				finder += dr.weight;
				if (finder >= rand)
				{
					if (dr.drop == null)
					{
						//If the drop chosen happens to be empty, this allows it to have no drop without breaking the game
						break;
					}
					drop = Instantiate(dr.drop) as GameObject;
					drop.transform.position = this.transform.position;
					break;
				}
			}
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawLine(transform.position + colliderOffset, transform.position + colliderOffset + Vector3.down * groundLength);
		Gizmos.DrawLine(transform.position - colliderOffset, transform.position - colliderOffset + Vector3.down * groundLength);

	}

	public void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("PlayerProjectile") && gameObject.CompareTag("Enemy"))
		{
			ElimCharacter();
		}
		else if (collision.CompareTag("PlayerProjectile") && gameObject.CompareTag("Boss"))
		{
			EliminateBoss();
		}
	}
}
