using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPickUp : MonoBehaviour
{
    string bulletState;

    private void Start()
    {
        bulletState = gameObject.tag;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        /*
        switch (bulletState)
        {
            //the getcomponent finds type GrandpaController because that's the only player that should swap bullets
            case "BlueBulletItem":
                if (collision.gameObject.tag.Equals("Player"))
                {
                    collision.gameObject.GetComponent<GrandpaController>().remainingAmmo[1] += 6;
                    FindObjectOfType<SFXManager>().PlayAudio("Pickup");
                    Destroy(gameObject);
                }
                break;

            case "RedBulletItem":
                if (collision.gameObject.tag.Equals("Player"))
                {
                    collision.gameObject.GetComponent<GrandpaController>().remainingAmmo[2] += 6;
                    FindObjectOfType<SFXManager>().PlayAudio("Pickup");
                    Destroy(gameObject);
                }
                break;

                
            case "GreenBulletItem":
                if (collision.gameObject.tag.Equals("Player"))
                {
                    collision.gameObject.GetComponent<GrandpaController>().remainingAmmo[3] += 6;
                    FindObjectOfType<SFXManager>().PlayAudio("Pickup");
                    Destroy(gameObject);
                }
                break;
                

            default:
                //Value was unexpected for whatever reason
                break;

        }
        */
    }
}
