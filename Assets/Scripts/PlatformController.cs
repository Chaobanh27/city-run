using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer platformSR;
    [SerializeField] private SpriteRenderer platformHeaderSR;

    private void Start()
    {
        platformHeaderSR.transform.parent = transform.parent;
        platformHeaderSR.transform.localScale = new Vector2(platformSR.bounds.size.x, .2f);
        platformHeaderSR.transform.position = new Vector2(transform.position.x, platformSR.bounds.max.y - .1f);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<PlayerController>() != null)
        {
            platformHeaderSR.color = GameManager.instance.platformColor;

        }
    }
}
