using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTrigger : MonoBehaviour
{

    [Header("Boss Components")]
    public Enemy bossReference; //the boss object that will be activated after dialogue is finished
    public GameObject bossHP; //only necessary for boss triggers

    // Start is called before the first frame update
    void Start()
    {
        bossReference.isAwake = true;
        bossHP.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
