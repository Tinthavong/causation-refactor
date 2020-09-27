using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactables : MonoBehaviour
{
    /// <summary>
    /// The Interactables script is going to be designed with the ideas of how we want the character to interact with objects.
    /// *Important* This script is going to be attached directly to the player (and potentially Enemies/NPCs) and will search for tags nearby or directly in contact of the player.
    /// This will include stuff like:
    ///     -Collision detection between items like crates and platforms
    ///     -Detection for if the player is in range of an NPC or Sign that can initiate dialogue as well
    ///      as if the player is in the range of a door that can transition to a new scene.
    /// </summary>

    //TODO: Create basic collision detector for the following tags:
    //      Enemy, Object, and Interact
    //Collisions for Object should make it so character cannot move past object without jumping past (if possible) or not at all
    //Collisions for Enemy and Projectile should register hits against player and take away health (Not Included in First Playable)
    //Collisions for Interact should see if a player is at or near an object such as a sign or door that can be interacted with via the W key



    //nearObject is supposed to be the trigger object the player has just come into contact with
    public GameObject nearObject;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //set the tag of nearObject to the tag of the collided item
        nearObject = collision.GetComponent<GameObject>();
        nearObject.transform.tag = collision.tag;

        //switch case to determine what to do for each tag
        switch (nearObject.tag)
        {
            case "Enemy":
                Debug.Log("Object player is at is " + nearObject.tag);
                break;
            case "Object":
                Debug.Log("Object player is at is " + nearObject.tag);
                break;
            case "Interact":
                Debug.Log("Object player is at is " + nearObject.tag);
                break;
            default:
                Debug.Log("Not colliding with proper object");
                break;
        }

        Debug.Log(nearObject.tag);
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
}
