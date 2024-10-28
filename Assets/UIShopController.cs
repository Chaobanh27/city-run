using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct ColorToPurchase
{
    public Color color;
    public int price;
}

public enum ColorType
{
    playerColor,
    platformColor
}
public class UIShopController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private TextMeshProUGUI notifyText;
    [Space]

    [Header("Platform Color")]
    [SerializeField] private GameObject platformColorBtn;
    [SerializeField] private Transform platformColorParent;
    [SerializeField] private Image platformDisplay;
    [SerializeField] private ColorToPurchase[] platformColor;

    [Header("Player Color")]
    [SerializeField] private GameObject playerColorBtn;
    [SerializeField] private Transform playerColorParent;
    [SerializeField] private Image playerDisplay;
    [SerializeField] private ColorToPurchase[] playerColor;


    // Start is called before the first frame update
    void Start()
    {
        coinsText.text = PlayerPrefs.GetInt("Coins").ToString("#,#");

        for (int i = 0; i < platformColor.Length; i++)
        {
            GameObject newBtn = Instantiate(platformColorBtn, platformColorParent);
            Color color = platformColor[i].color;
            int price = platformColor[i].price;
            newBtn.transform.GetChild(0).GetComponent<Image>().color = color;
            newBtn.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = price.ToString("#,#") + " Coins";

            newBtn.GetComponent<Button>().onClick.AddListener(() => PurchaseColor(color, price, ColorType.platformColor));
        }

        for (int i = 0; i < playerColor.Length; i++)
        {
            GameObject newBtn = Instantiate(playerColorBtn, playerColorParent);
            Color color = playerColor[i].color;
            int price = playerColor[i].price;
            Debug.Log(color.ToString("#,#"));
            Debug.Log(price.ToString("#,#"));
            newBtn.transform.GetChild(0).GetComponent<Image>().color = color;
            newBtn.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = price.ToString("#,#") + " Coins";

            newBtn.GetComponent<Button>().onClick.AddListener(() => PurchaseColor(color, price, ColorType.playerColor));
        }
    }

    private void PurchaseColor(Color color, int price, ColorType colorType)
    {

        AudioManagerController.instance.PlaySFX(1);
        if (CheckEnoughMoney(price))
        {
            if(colorType == ColorType.platformColor)
            {
                GameManager.instance.platformColor = color;
                platformDisplay.color = color;
            }
            else if (colorType == ColorType.playerColor)
            {
                GameManager.instance.playerController.GetComponent<SpriteRenderer>().color = color;
                GameManager.instance.SaveColor(color.r, color.g, color.b);
                playerDisplay.color = color;
            }
            StartCoroutine(Notify("Purchased Successful", 1));
        }
        else
        {
            StartCoroutine(Notify("Not Enough Money! ", 1));
        }
    }

    private bool CheckEnoughMoney(int price)
    {
        int myCoins = PlayerPrefs.GetInt("Coins");

        if (myCoins > price)
        {
            int newAmountOfCoins = myCoins - price;

            PlayerPrefs.SetInt("Coins", newAmountOfCoins);
            coinsText.text = newAmountOfCoins.ToString("#,#");
            return true;
        }
        return false;
    }

    private IEnumerator Notify(string text, float seconds)
    {
        notifyText.text =  text;

        yield return new WaitForSeconds(seconds);

        notifyText.text = "Click to buy";
    }
}
