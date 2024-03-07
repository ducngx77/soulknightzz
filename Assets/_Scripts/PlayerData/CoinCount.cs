using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;


public class CoinCount : MonoBehaviour
{
    public int coinNumber ;
    [SerializeField]
    public TMP_Text CoinText;
    // Start is called before the first frame update
    void Start()
    {
        LoadJsonData();
        CoinText = GameObject.Find("CoinCount").GetComponent<TMP_Text>();
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
    public void LoadJsonData()
    {
        string json = File.ReadAllText(Application.dataPath + "/_Scripts/PlayerData/CoinConfig.json");
        Coin data = JsonUtility.FromJson<Coin>(json);
        coinNumber = data.coinValue;
    }
    [System.Serializable]
    public class Coin
    {
        public int coinValue;

    }

}
