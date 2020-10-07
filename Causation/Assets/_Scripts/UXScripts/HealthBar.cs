using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    //Would like to make more generic, have this be playerHUD and combine with health, ammo, currency, specials, etc.
    public Slider fill;
    public Gradient healthIndicator;
    public Image HP;

    public void SetMaxHealth(int health)
    {
        fill.maxValue = health;
        fill.value = health;

        HP.color = healthIndicator.Evaluate(1f);
    }

    public void SetHealth(int health)
    {
        fill.value = health;

        HP.color = healthIndicator.Evaluate(fill.normalizedValue);
    }
}
