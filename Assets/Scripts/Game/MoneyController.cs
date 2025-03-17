using System;
using TMPro;
using UnityEngine;

public class MoneyController : MonoBehaviour
{
    public TMP_Text moneyText;
    public static MoneyController instance;
    public void Start()
    {
        instance = this;
        moneyText.text = PlayerPrefs.GetInt("money").ToString();
    }

    public void AddMoney()
    {
        PlayerPrefs.SetInt("money", PlayerPrefs.GetInt("money") + 1);
        moneyText.text = PlayerPrefs.GetInt("money").ToString();
    }
}
