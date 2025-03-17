using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlayerStats : MonoBehaviour
{
    [SerializeField] private int coinCount = 0;
    [SerializeField] private int scoreCount = 0;
    private int hiddenScore;
    [SerializeField] private int xpCount = 0;
    private const string CoinCountKey = "CoinCount";
    private const string xpCountKey = "xpCount";
    public List<ShopSlot> boughtGuns = new List<ShopSlot>();
    [SerializeField] private TextMeshProUGUI scoreText;


    void Start()
    {
        // Load the coin count from PlayerPrefs
        coinCount = PlayerPrefs.GetInt(CoinCountKey, 0);
        xpCount = PlayerPrefs.GetInt(xpCountKey, 0);
    }

    private void Update()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score : " + scoreCount;
        }




        if (hiddenScore >= 200)
        {
            hiddenScore = 0;
            xpCount++;
             PlayerPrefs.SetInt(xpCountKey, xpCount);
            PlayerPrefs.Save();
        }

    }

    public void AddCoins(int amount)
    {
        coinCount += amount;
        PlayerPrefs.SetInt(CoinCountKey, coinCount);
        PlayerPrefs.Save();
    }

    public void AddScore(int amount)
    {
        scoreCount += amount;
        hiddenScore += amount;
    }

 

    public void TakeCoins(int amount)
    {
        coinCount -= amount;
        PlayerPrefs.SetInt(CoinCountKey, coinCount);
        PlayerPrefs.Save();
    }

   
    public void AddBoughtGun(ShopSlot gunSlot)
    {
        boughtGuns.Add(gunSlot);
    }

    public void RemoveBoughtGun(ShopSlot gunSlot)
    {
        boughtGuns.Remove(gunSlot);
    }




}