using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeDetection : MonoBehaviour
{
    [SerializeField] private float radius;
    [SerializeField] private LayerMask groundCheck;
    [SerializeField] private PlayerController player;
    [SerializeField] private bool isLedgeDetected;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isLedgeDetected)
        {
            player.isLedge = Physics2D.OverlapCircle(transform.position, radius, groundCheck);
        }

    }
    //nói chung là chúng ta sẽ có 1 cái hình tròn gizmos và 1 collision chồng lên nhau với hình tròn nằm dưới và collision nằm đè lên nửa trên gizmos
    //nếu như đưa object có layer là ground vào phạm vi của gizmos nhưng lại phải đi qua phạm vi của collision thì sẽ trả về là isLedgeDetected = false
    //còn nếu đưa object có layer là ground vào phạm vi của gizmos nhưng KHÔNG phải đi qua phạm vi của collision thì sẽ trả về là isLedgeDetected = true
    //nếu như isLedgeDetected = true thì sẽ xác định được đó là ledge hay không

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isLedgeDetected = false;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isLedgeDetected = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
