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
            Currency.currencyValue = 1;
            AudioSource.PlayClipAtPoint(this.GetComponent<AudioSource>().clip, this.transform.position);
            Destroy(gameObject);
        }  
    }
}
