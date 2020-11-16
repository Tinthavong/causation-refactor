using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPickUp : MonoBehaviour
{
    string bulletState;

    private void Start()
    {
        bulletState = gameObject.tag; //This might seem like a roundabout way to handle this but this stores the tag for the appropriate bullet type used for the switch
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (bulletState)
        {
            case "BlueBulletItem":
                if (collision.gameObject.tag.Equals("Player"))
                {
                    collision.gameObject.GetComponent<PlayerController>().remainingAmmo[1] += 6;
                    FindObjectOfType<SFXManager>().PlayAudio("Pickup");
                    Destroy(gameObject);
                }
                break;

            case "RedBulletItem":
                if (collision.gameObject.tag.Equals("Player"))
                {
                    collision.gameObject.GetComponent<PlayerController>().remainingAmmo[2] += 6;
                    FindObjectOfType<SFXManager>().PlayAudio("Pickup");
                    Destroy(gameObject);
                }
                break;

                
            case "GreenBulletItem":
                if (collision.gameObject.tag.Equals("Player"))
                {
                    collision.gameObject.GetComponent<PlayerController>().remainingAmmo[3] += 6;
                    FindObjectOfType<SFXManager>().PlayAudio("Pickup");
                    Destroy(gameObject);
                }
                break;
                

            default:
                //Value was unexpected for whatever reason
                break;

        }
    }
}
