using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInGameController : MonoBehaviour
{
    private PlayerController playerController;

    [SerializeField] private TextMeshProUGUI distanceText;
    [SerializeField] private TextMeshProUGUI coinsText;

    [SerializeField] private Image heartEmpty;
    [SerializeField] private Image heartFull;

    private float distance;
    private int coins;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GameManager.instance.playerController;
    }

    // Update is called once per frame
    void Update()
    {
        distance = GameManager.instance.distance;
        coins = GameManager.instance.coins;
        if(distance > 0 )
        {
            distanceText.text = distance.ToString("#,#") + " m";
        }
        if( coins > 0 )
        {
            coinsText.text = coins.ToString("#,#");
        }

        heartEmpty.enabled = !playerController.extraLife;
        heartFull.enabled = playerController.extraLife;

    }
}
