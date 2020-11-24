using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTriggers : MonoBehaviour
{
    Enemy[] enemiesInLevel = UnityEngine.Object.FindObjectsOfType<Enemy>();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //if player enters pod then they are triggered

    //if player presses the retry from checkpoint button enemies go back to sleep
    public void EnemyAsleep()
    {
        foreach (Enemy en in enemiesInLevel)
        {
            en.isAwake = false;
        }
    }




}
