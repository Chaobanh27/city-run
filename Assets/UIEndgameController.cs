using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIEndgameController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI distanceText;
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private TextMeshProUGUI scoreText;
    // Start is called before the first frame update
    void Start()
    {
        GameManager manager = GameManager.instance;

        if(manager.distance <= 0)
        {
            return;
        }

        if(manager.coins <= 0)
        {
            return;
        }


        distanceText.text = "Distance: " + manager.distance.ToString("#,#") + " m";
        coinsText.text = "Coins: " + manager.coins.ToString("#,#") + " coins";
        scoreText.text = "Score: " + manager.score.ToString("#,#") + " points";

    }
}
