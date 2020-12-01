using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorFlight : MonoBehaviour
{
    public GameObject waypointA;
    public GameObject waypointB;
    private float speed = 5;
    public bool shouldChangeFacing = false;
    private bool directionAB = false;


    private void FixedUpdate()
    {
        if (this.transform.position ==
            waypointA.transform.position && directionAB == false
            || this.transform.position ==
            waypointB.transform.position && directionAB == true)
        {
            directionAB = !directionAB;
        }

        if (directionAB == true)
        {
             this.transform.position =
             Vector3.MoveTowards(this.transform.position,
             waypointB.transform.position, speed * Time.fixedDeltaTime);
        }
        else
        {
             this.transform.position =
             Vector3.MoveTowards(this.transform.position,
             waypointA.transform.position, speed * Time.fixedDeltaTime);
        }
    }

}
