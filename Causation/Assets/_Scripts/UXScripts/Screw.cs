using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Screw : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            Currency.currencyValue += 1;
            
            UpdateCurrency();
            FindObjectOfType<SFXManager>().PlayAudio("Pickup");
            Destroy(gameObject);
        }  
    }

    void UpdateCurrency()
    {
        Currency.walletValue = Currency.currencyValue;
        Currency.currency.text = Currency.walletValue.ToString();
    }
}
