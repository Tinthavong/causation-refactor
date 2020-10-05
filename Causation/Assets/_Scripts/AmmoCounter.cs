using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AmmoCounter : MonoBehaviour
{
    public TMP_Text ammoText;
    public static int currentAmmo;
    public static int maxAmmo = 6;

    // Start is called before the first frame update
    void Start()
    {
        currentAmmo = maxAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        if (ammoText.text != null)
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
}
