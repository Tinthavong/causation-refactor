using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedkitController : MonoBehaviour
{
    [SerializeField]
    public GameObject[] medkits;
    public HealthBar hb;
    //public CharacterBase chb;
    public PlayerController pc;
    
    public int medkitNumber;
    
    // Start is called before the first frame update
    void Start()
    {
        pc = FindObjectOfType<PlayerController>();
        for (int i = 0; i <= 2; i++)
        {
            medkits[i].gameObject.SetActive(true);
        }

        medkitNumber = 3;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && medkitNumber > 0 && pc.displayedHealth != pc.Health)
        {
            medkitNumber -= 1;
            
            medkits[medkitNumber].gameObject.SetActive(false);

            pc.displayedHealth += 3;

            hb.SetHealth(pc.displayedHealth);

            //Debug.Log("Current Health: " + chb.displayedHealth);
        }
    }

    
}
