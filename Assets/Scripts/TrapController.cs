using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapController : MonoBehaviour
{
    [SerializeField] protected float chanceToSpawn = 60;

    protected virtual void Start()
    {
        bool canSpawn = chanceToSpawn >= Random.Range(0, 100);

        if(!canSpawn)
        {
            Destroy(gameObject);
        }
    }
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<PlayerController>() != null)
        {
            collision.GetComponent<PlayerController>().Damage();
        }
    }
}
