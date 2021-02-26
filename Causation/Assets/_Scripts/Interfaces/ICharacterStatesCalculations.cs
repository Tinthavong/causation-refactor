using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacterStatesCalculations
{
    void DamageCalculation(int damageValue);

    void HealCalculation(int healValue);

    void AmmoUsageCalculation();

    void AmmoReloadCalulcation();

    void AmmoState();
}
