using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinCount : MonoBehaviour
{

    private int coinNumber = 0;
    public TMP_Text CoinText;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        CoinText.text = coinNumber.ToString();
        Debug.Log(coinNumber.ToString());   
    }

    public void CollectCoin(int amount)
    {

        if (amount < 0)
        {
            throw new System.ArgumentOutOfRangeException("Cannot have negative Coin");
        }
        

        this.coinNumber += amount;
        CoinText.text = coinNumber.ToString();
    }

    public void SpendCoin(int amount)
    {

        if (amount < 0)
        {
            throw new System.ArgumentOutOfRangeException("Cannot have negative Coin");
        }
        

        this.coinNumber -= amount;
        CoinText.text = coinNumber.ToString();
    }
}
