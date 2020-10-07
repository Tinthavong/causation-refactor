using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Currency : MonoBehaviour
{
    //I don't like this as is, but if this class is turned into a data type to contain the currency the player uses then that's fine
    //but all this does is update some text on the screen - no utility
    public static int currencyValue;
    TMP_Text currency;

    // Start is called before the first frame update
    void Start()
    {
        currency = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        currency.text = currencyValue.ToString();
    }
}
