using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTrigger : MonoBehaviour
{

    [Header("Boss Components")]
    //public Enemy bossReference; //the boss object that will be activated after dialogue is finished
    public GameObject bossHP; //only necessary for boss triggers
    public EnemyBaseCombatController bossCharacter;

    private float cutsceneDuration;

    // Start is called before the first frame update
    void Start()
    {
        cutsceneDuration = FindObjectOfType<TimelineManager>().cutsceneDuration;   
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            BossStartUp();
        }
    }

    void BossStartUp()
    {
        Invoke("BossTriggerSleep", cutsceneDuration);
    }

    void BossTriggerSleep() //Makes the trigger game object inactive after the cutscene has played
    {
        bossHP.SetActive(true);
        bossCharacter.isAwake = true;
        gameObject.SetActive(false);
    }
}
