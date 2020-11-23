using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedkitController : MonoBehaviour
{
    [SerializeField]
    public GameObject[] medkits;
    public HealthBar hb;
    public PlayerController pc;
    
    public int medkitNumber;
    
    // Start is called before the first frame update
    void Start()
    {
        pc = FindObjectOfType<PlayerController>();

        //for (int i = 0; i <= 2; i++)
        //{
        //    medkits[i].gameObject.SetActive(true);
        //}

        medkitNumber = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && medkitNumber > 0 && pc.displayedHealth != 15)
        {
            medkitNumber -= 1;

            switch (medkitNumber)
            {
                case 0:
                    {
                        medkits[0].gameObject.SetActive(false);
                        break;
                    }
                case 1:
                    {
                        medkits[1].gameObject.SetActive(false);
                        break;
                    }
                case 2:
                    {
                        medkits[2].gameObject.SetActive(false);
                        break;
                    }
            }
                
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
