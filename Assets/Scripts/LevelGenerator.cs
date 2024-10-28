using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private Transform[] levelPart;
    [SerializeField] private Vector3 nextPartPosition;
    [SerializeField] private float distanceToSpawn;
    [SerializeField] private float distanceToDelete;
    [SerializeField] private Transform player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GeneratePlatform();
        DeletePlatform();
    }

    private void GeneratePlatform()
    {
        //Trong khi khoảng cách giữa vị trí của người chơi (player.transform.position) và vị trí của platform tiếp theo (nextPartPosition) nhỏ hơn distanceToSpawn, vòng lặp sẽ tiếp tục chạy
        while (Vector2.Distance(player.transform.position, nextPartPosition) < distanceToSpawn)
        {
            //Một platform ngẫu nhiên từ mảng levelPart được chọn và gán cho biến part với kiểu Transform để sử dụng các method của nó
            Transform part = levelPart[Random.Range(0, levelPart.Length)];

            //Vị trí mới (newPosition) của platform sẽ được tính toán dựa trên vị trí của platform trước đó và vị trí của một điểm đặc biệt trên platform gọi là "StartPoint"
            Vector2 newPosition = new Vector2(nextPartPosition.x - part.Find("StartPoint").position.x, 0);

            //Một bản sao của platform được tạo ra tại vị trí mới với hướng quay mặc định(transform.rotation) và được gắn vào đối tượng gọi phương thức(transform)
            Transform newPart = Instantiate(part, newPosition, transform.rotation, transform);

            //Vị trí của platform tiếp theo (nextPartPosition) được cập nhật dựa trên vị trí của một điểm khác trên platform vừa tạo gọi là "EndPoint".
            nextPartPosition = newPart.Find("EndPoint").position;

        }
    }

    private void DeletePlatform()
    {
        //nếu như số lượng con của đối tượng Transform cha lớn hơn 0
        if(transform.childCount > 0)
        {
            //tìm đến đối tượng con đầu tiên để xóa
            Transform partToDelete = transform.GetChild(0);

            //nếu như khoảng cách từ vị trí người chơi đến platform cần xóa lớn hơn khoảng cách cần xóa thì xóa platform đó
            if(Vector2.Distance(player.transform.position, partToDelete.transform.position) > distanceToDelete)
            {
                Destroy(partToDelete.gameObject);
            }
        }
    }
}
