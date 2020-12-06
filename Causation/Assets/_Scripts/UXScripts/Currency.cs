using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Currency : MonoBehaviour
{
    public static TMP_Text currency;
    public static int walletValue; //this is the walletValue that is going to be used for save data

    private int walletProperty; //this is the walletProperty that is referenced inside of playerController

    public int WalletProperty
    {
        get { return walletProperty; }

        set
        {           
            walletValue = walletProperty;
            walletProperty = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        currency = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        currency.text = walletProperty.ToString();
    }
}
