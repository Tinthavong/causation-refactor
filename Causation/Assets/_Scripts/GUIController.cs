using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GUIController : MonoBehaviour
{
    public Slider slider;
    
    public Text ammoText;
    public static float currentAmmo = 6;
    public static float maxAmmo = 6; 


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (ammoText != null)
        {
            ammoText.text = currentAmmo.ToString();

            if (Input.GetButtonDown("Fire1"))
            {
                if (currentAmmo == 0)
                {
                    currentAmmo = maxAmmo + 1;
                }

                currentAmmo--;
            }
        }
    }

    public void SetHealth(int health)
    {
        slider.value = health;
    }

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }
}
