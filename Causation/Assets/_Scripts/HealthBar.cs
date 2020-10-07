using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
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
