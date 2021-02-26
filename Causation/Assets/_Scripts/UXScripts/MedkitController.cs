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
    public PlayerBaseStats pc;

    void Start()
    {
        pc = FindObjectOfType<PlayerBaseStats>();

        for (int i = 0; i <= 2; i++)
        {
            medkits[i].gameObject.SetActive(false);
        }

        medkitNumber = 0;
    }

    void Update()
    {
        //When the player presses Q to use a medkit
        if (Input.GetKeyDown(KeyCode.Q) && medkitNumber > 0 && pc.CharacterHealth != 15)
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
            if (pc.CharacterHealth < 15 && pc.CharacterHealth >= 13)
            {
                pc.CharacterHealth = 15;
            }
            else
            {
                pc.CharacterHealth += 3;
            }

            hb.UpdateHealthBar(pc.CharacterHealth);

            Debug.Log("Medkit Used, Counter is at: " + medkitNumber);
            Debug.Log("Player has: " + pc.CharacterHealth + " HP left");
        }
    }

    
}
