using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Currency : MonoBehaviour
{
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
