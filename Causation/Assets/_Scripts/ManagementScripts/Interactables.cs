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
    //Interactables has been changed to only search for objects like boxes, signs, and scene transitions

    //nearObject is supposed to be the trigger object the player has just come into contact with
    public GameObject nearObject;
    Vector3 objectPosition;

    //Set these in inspector with appropriate prefabs of the same names
    [SerializeField] private GameObject indicatorText;
    [SerializeField] private TextMeshProUGUI signText;

    //mean for the update check when player is pushing the W key
    private bool isColliding;
    public static bool transitionFlag = false;
    private int hardLock = 1;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) && transitionFlag && hardLock > 0)
        {
            Invoke("TeleportTransition", 1f);
        }
    }

    private void TeleportTransition()
    {
        Camera mc = FindObjectOfType<Camera>();
        gameObject.transform.position = GameObject.Find("ScreenTransitionB").transform.position;
        Vector3 dummy = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, -10);
        mc.transform.position = dummy;
        transitionFlag = false; //no backtracking. also this implementation ain't great huh
        hardLock--;
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
            case "Object":
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
