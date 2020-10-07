using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IActions
{
    void Shoot();
    //Melee();
    void PostDeath(); //drops item for enemy, shows game over for player. should use with the elim method
}