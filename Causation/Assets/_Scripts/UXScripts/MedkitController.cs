using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedkitController : MonoBehaviour
{
    [SerializeField]
    public GameObject[] medkits;
    public HealthBar hb;
    public CharacterBase chb;

    
    public int medkitNumber;
    
    // Start is called before the first frame update
    void Start()
    {
        chb = GameObject.Find("Grandpa").GetComponent<CharacterBase>();
        for (int i = 0; i <= 2; i++)
        {
            medkits[i].gameObject.SetActive(true);
        }

        medkitNumber = 3;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && medkitNumber > 0 && chb.displayedHealth != chb.Health)
        {
            medkitNumber -= 1;
            
            medkits[medkitNumber].gameObject.SetActive(false);

            chb.displayedHealth += 1;

            hb.SetHealth(chb.displayedHealth);

            //Debug.Log("Current Health: " + chb.displayedHealth);
        }
    }

    
}
