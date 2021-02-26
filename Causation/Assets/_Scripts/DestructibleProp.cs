using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class DestructibleProp : MonoBehaviour
{
	[Serializable]
	public class ItemDrops
	{
		public GameObject drop;
		public int weight;
		public ItemDrops(GameObject d, int w)
		{
			drop = d;
			weight = w;
		}
	}

	[Header("Item Drops")]
	//Consider making these private and serialized
	public int dropValue;
	//List of EnemyDrops (explained above) to allow for multiple drops
	public List<ItemDrops> drops;
	private int dropOnce = 1; //Quick bandaid fix for making sure items are only dropped once

	private void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("PlayerProjectile"))
        {
            //Spawn drops
            DropItems();

            Destroy(collision.gameObject);
            Destroy(gameObject); //Commented out for now, destroying the game object is too abrupt.
        }
    }

    private void DropItems()
    {
		//Variables for the dynamic drop search
		GameObject drop;
		int totalweight = 0;
		int rand;
		int finder = 0;
		System.Random random = new System.Random();

		//Adds up total weight for use in RNG
		foreach (ItemDrops dr in drops)
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
			foreach (ItemDrops dr in drops)
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
}
