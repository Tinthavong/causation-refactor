using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.PlayerLoop;

public class Interactables : MonoBehaviour
{
    /// <summary>
    /// Created by Tyler F
    /// The Interactables script is going to be designed with the ideas of how we want the character to interact with objects.
    /// *Important* This script is going to be attached directly to the player (and potentially Enemies/NPCs) and will search for tags nearby or directly in contact of the player.
    /// This will include stuff like:
    ///     -Collision detection between items like crates and platforms
    ///     -Detection for if the player is in range of an NPC or Sign that can initiate dialogue as well
    ///      as if the player is in the range of a door that can transition to a new scene.
    /// </summary>

    //TODO: Create basic collision detector for the following tags:
    //      Enemy, Object, and Interact, Projectile
    //Collisions for Object should make it so character cannot move past object without jumping past (if possible) or not at all
    //Collisions for Enemy should register hits against player and take away health (Not Included in First Playable)
    //Collisions for Projectile should be similar to enemy in the fact that it will cause damage and appropriate animations
    //Collisions for Interact should see if a player is at or near an object such as a sign or door that can be interacted with via the W key



    //nearObject is supposed to be the trigger object the player has just come into contact with
    public GameObject nearObject;
    Vector3 objectPosition;

    //Set these in inspector with appropriate prefabs of the same names
    [SerializeField] private GameObject indicatorText;
    [SerializeField] private TextMeshProUGUI signText;

    //mean for the update check when player is pushing the W key
    private bool isColliding;
    private bool transitionFlag = false;

    //TODO: Figure out how to properply display sign text. Should display below sign in red text so player can visibly see it
    private void FixedUpdate()
    {
        /*
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (isColliding = true && nearObject.tag != null)
            {
                objectPosition.y -= 2.5f;

                Instantiate(signText, objectPosition, Quaternion.identity);
            }
        }*/

        if (Input.GetKeyDown(KeyCode.W) && transitionFlag)
        {
            Camera mc = FindObjectOfType<Camera>();
            gameObject.transform.position = GameObject.Find("ScreenTransitionB").transform.position;
            Vector3 dummy = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, -10);
            mc.transform.position = dummy;
            transitionFlag = false; //no backtracking. also this implementation ain't great huh
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Assign nearObject and objectPosition to the object the player collides with
        nearObject = collision.gameObject;
        objectPosition = nearObject.transform.position;

        //set the tag of nearObject to the tag of the collided item
        nearObject.tag = collision.gameObject.tag;

        //switch case to determine what to do for each tag
        switch (nearObject.tag)
        {
            //TODO: Change tag system to determine simply objects that can be interacted with (like medkits and signs)
            case "Enemy":
                Debug.Log("Object player is at is " + nearObject.tag);
                isColliding = true;
                break;
            case "Object":
                nearObject.GetComponent<PlayerController>().OnLanding();
                Debug.Log("Object player is at is " + nearObject.tag);
                isColliding = true;
                break;
            case "Interact":
                //If the object has the 'Interact' tag spawn in text above the player to show they can push a button to interact with object
                SpawnText(objectPosition);
                Debug.Log("Object player is at is " + nearObject.tag);
                isColliding = true;
                break;
            case "Transition":
                //If the object has the 'Interact' tag spawn in text above the player to show they can push a button to interact with object
                SpawnText(objectPosition);
                transitionFlag = true;
                break;
            default:
                Debug.Log("Not colliding with tagged object");
                isColliding = true;
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //Creates an array of objects with the "Text" tag for easy deletion
        GameObject[] textObjects = GameObject.FindGameObjectsWithTag("Text");

        //Deletes everything with "Text" tag 
        foreach (GameObject text in textObjects)
        {
            Destroy(text);
        }

        nearObject = null;
        isColliding = false;
    }

    public void SpawnText(Vector3 objectPosition)
    {

        objectPosition.y += 2.5f;
        Instantiate(indicatorText, objectPosition, Quaternion.identity);
    }
}
