using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;

public class Finish : MonoBehaviour
{
    private bool levelComplete = false;
    private RoomFirstDungeonGenerator roomFirstDungeonGenerator;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string objectName = collision.gameObject.name;
        if (!levelComplete )
        {
            levelComplete = true;
            StartCoroutine(ReloadCurrentLevel());
        }
    }

    //private bool IsAgent(string objectName)
    //{
    //    // Kiểm tra xem tên đối tượng có khớp với mẫu "AgentXX" không
    //    return Regex.IsMatch(objectName, @"^Agent\d{2}$");
    //}

    private IEnumerator ReloadCurrentLevel()
    {
        yield return new WaitForSeconds(1f);

        // Tìm và kích hoạt RoomFirstDungeonGenerator trong cảnh hiện tại (Level 1)
        if (roomFirstDungeonGenerator == null)
        {
            roomFirstDungeonGenerator = FindObjectOfType<RoomFirstDungeonGenerator>();
        }

        if (roomFirstDungeonGenerator != null)
        {
            // Reset cảnh hiện tại (Level 1) bằng cách tạo lại bản đồ
            roomFirstDungeonGenerator.CreateRooms();
        }
        else
        {
            Debug.LogWarning("Không tìm thấy RoomFirstDungeonGenerator trong cảnh hiện tại (Level 1).");
        }

        levelComplete = false; // Đặt lại biến levelComplete để cho phép kích hoạt lại khi qua màn tiếp theo
    }
}
