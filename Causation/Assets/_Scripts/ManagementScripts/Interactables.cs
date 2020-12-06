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
    [SerializeField] private GameObject indicatorText = null;
    [SerializeField] private TextMeshProUGUI signText;

    public static bool transitionFlag = false;

    [HideInInspector]
    public bool bossFlag;

    public int screenLock = 1;
    public int screenDestination = 0;

    [HideInInspector]
    public GameObject[] screenTransitions;

    private void Start()
    {
        //Number of appropriate screen transitions is found at start
        screenTransitions = GameObject.FindGameObjectsWithTag("Transition");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) && transitionFlag && screenLock > 0)
        {
            Invoke("TeleportTransition", 1f);
        }
        else if (Input.GetKeyDown(KeyCode.W) && bossFlag && screenLock > 0)
        {
            Invoke("BossRoomTeleportation", 1f);
        }
    }

    //Since some levels may have multiple screen transitions this is a very rough way of handling that
    private void TeleportTransition()
    {
        Camera mc = FindObjectOfType<Camera>();

        //i is increased and compared here and corresponds to which transition the player should be sent to
        switch (screenDestination)
        {
            case 0:
                //When duplicating transitions the second transition of the two (in this case between A & B) should be chosen
                gameObject.transform.position = GameObject.Find("ScreenTransitionB").transform.position;
                Vector3 newCameraPosition = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, -10);
                mc.transform.position = newCameraPosition;
                transitionFlag = false; //no backtracking. also this implementation ain't great huh

                //Always increase i
                screenDestination++;
                break;
            case 1:
                gameObject.transform.position = GameObject.Find("ScreenTransitionD").transform.position;
                newCameraPosition = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, -10);
                mc.transform.position = newCameraPosition;
                transitionFlag = false; //no backtracking. also this implementation ain't great huh

                screenDestination++;
                break;
            case 2:
                gameObject.transform.position = GameObject.Find("ScreenTransitionF").transform.position;
                newCameraPosition = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, -10);
                mc.transform.position = newCameraPosition;
                transitionFlag = false; //no backtracking. also this implementation ain't great huh

                screenDestination++;
                break;
        }

        if (screenDestination == screenTransitions.Length)
        {
            screenLock--;
            screenDestination = 0;
        }
    }

    private void BossRoomTeleportation()
    {
        Camera mc = FindObjectOfType<Camera>();

        gameObject.transform.position = GameObject.Find("ScreenTransitionF").transform.position;
        Vector3 newCameraPosition = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, -10);
        mc.transform.position = newCameraPosition;
        screenLock--;
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
                break;
            case "Interact":
                //If the object has the 'Interact' tag spawn in text above the player to show they can push a button to interact with object
                SpawnText(objectPosition);
                Debug.Log("Object player is at is " + nearObject.tag);
                break;
            case "Transition":
                //If the object has the 'Transition' tag spawn in text above the player to show they can push a button to interact with object
                SpawnText(objectPosition);
                transitionFlag = true;
                break;
            case "BossTransition":
                SpawnText(objectPosition);
                bossFlag = true;
                break;
            default:
                Debug.Log("Not colliding with tagged object");
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
    }

    public void SpawnText(Vector3 objectPosition)
    {

        objectPosition.y += 2.5f;
        Instantiate(indicatorText, objectPosition, Quaternion.identity);
    }
}
