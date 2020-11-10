using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Currency : MonoBehaviour
{
    //I don't like this as is, but if this class is turned into a data type to contain the currency the player uses then that's fine
    //but all this does is update some text on the screen - no utility
    //Noah - Except this would just hold the currency value as an int and can be refernced in other scripts, 
    //so we're not over loading the PlayerController script

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
        currency.text = walletValue.ToString();
    }
}
