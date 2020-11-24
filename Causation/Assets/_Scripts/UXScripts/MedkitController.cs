using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedkitController : MonoBehaviour
{
    [SerializeField]
    public GameObject[] medkits;
    public HealthBar hb;
    
    [HideInInspector]
    public int medkitNumber;
    public PlayerController pc;

    void Start()
    {
        pc = FindObjectOfType<PlayerController>();

        //for (int i = 0; i <= 2; i++)
        //{
        //    medkits[i].gameObject.SetActive(true);
        //}

        medkitNumber = 0;
    }

    void Update()
    {
        //When the player presses Q to use a medkit
        if (Input.GetKeyDown(KeyCode.Q) && medkitNumber > 0 && pc.displayedHealth != 15)
        {
            medkitNumber -= 1;

            //Checks what the integer of the medkitNumber variable to 
            //show what medkit was used in the UI
            switch (medkitNumber)
            {
                case 0:
                    {
                        medkits[0].gameObject.SetActive(false);
                        medkits[1].gameObject.SetActive(false);
                        medkits[2].gameObject.SetActive(false);
                        break;
                    }
                case 1:
                    {
                        medkits[1].gameObject.SetActive(false);
                        medkits[2].gameObject.SetActive(false);
                        break;
                    }
                case 2:
                    {
                        medkits[2].gameObject.SetActive(false);
                        break;
                    }
            }
            
            //Checks the players current health when using a medkit to not go over 15 HP
            //Anything over messes with the health bar displaying to the player
            //Also could mess up the medkits visualy in the UI
            if (pc.displayedHealth < 15 && pc.displayedHealth >= 13)
            {
                pc.displayedHealth = 15;
            }
            else
            {
                pc.displayedHealth += 3;
            }

            hb.SetHealth(pc.displayedHealth);

            Debug.Log("Medkit Used, Counter is at: " + medkitNumber);
            Debug.Log("Player has: " + pc.displayedHealth + " HP left");
        }
    }

    
}
