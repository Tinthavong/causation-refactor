using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleProp : Enemy
{
    // Update is called once per frame
    void Update()
    {
        ElimCharacter();
    }

    public override void ElimCharacter()
    {
        //this might be too simple but for now checking if the health is at or below 0 might be enough
        if (displayedHealth <= 0)
        {
            PostDeath();        
            Destroy(gameObject); //Commented out for now, destroying the game object is too abrupt.
        }
    }
}
