using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<PlayerController>() != null)
        {
            AudioManagerController.instance.PlaySFX(4);
            GameManager.instance.coins++;
            Destroy(gameObject);
        }
    }
}
