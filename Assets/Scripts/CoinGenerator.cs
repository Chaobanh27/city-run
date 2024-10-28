using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinGenerator : MonoBehaviour
{
    [SerializeField] private int amountOfCoin;  
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private int minCoins;
    [SerializeField] private int maxCoins;
    [SerializeField] private SpriteRenderer[] coinImg;

    //[SerializeField] private float chanceToSpawn;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < coinImg.Length; i++)
        {
            coinImg[i].sprite = null;
        }
        amountOfCoin = Random.Range(minCoins, maxCoins);
        int additionalOffset = amountOfCoin / 2;
        for (int i = 0; i < amountOfCoin; i++)
        {
            //bool canSpawn = chanceToSpawn > Random.Range(0, 100);
            Vector3 offset = new Vector2(i - additionalOffset, 0);
            Instantiate(coinPrefab, transform.position + offset, Quaternion.identity, transform);

            //if(canSpawn)
            //{
            //    Instantiate(coinPrefab, transform.position + offset, Quaternion.identity, transform);
            //}
        }
    }
}
