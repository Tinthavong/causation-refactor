using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    [Header("Boss Components")]
    public GameObject bossHP; //only necessary for boss triggers
    public EnemyBaseCombatController bossCharacter; //EnemyBaseCombatController has states for being active or not

    private float cutsceneDuration;

    // Start is called before the first frame update
    void Start()
    {
        cutsceneDuration = FindObjectOfType<TimelineManager>().cutsceneDuration;   
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            BossStartUp();
        }
    }

    //The duration of the cutscene that will elapse before the boss character is active
    void BossStartUp()
    {
        Invoke("BossTriggerAwake", cutsceneDuration);
    }

    //Makes the boss object active, displays health, and makes the actuall trigger collider inactive
    void BossTriggerAwake() 
    {
        bossHP.SetActive(true);
        bossCharacter.isAwake = true;
        //Consider activating the bosses' ability to be damaged here
        gameObject.SetActive(false);
    }
}
