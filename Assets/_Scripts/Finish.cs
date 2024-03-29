using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Finish : MonoBehaviour
{
    public CoinCount coinCounter;
    private bool levelComplete = false;
    private RoomFirstDungeonGenerator roomFirstDungeonGenerator;
    public TMP_Text level;
    private int countlv = 0;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        string objectName = collision.gameObject.name;
        if (!levelComplete && collision.tag == "Player"
            && GameObject.FindGameObjectWithTag("Boss") == null
            && GameObject.FindGameObjectWithTag("Enemy") == null)
        {
            levelComplete = true;
            StartCoroutine(ReloadCurrentLevel());
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            coinCounter = FindObjectOfType<CoinCount>();
            int data = coinCounter.coinNumber;

            Coins c = new Coins(data);
            string json = JsonUtility.ToJson(c);
            File.WriteAllText(Application.dataPath + "/_Scripts/PlayerData/CoinConfig.json", json);
        }
    }

    //private bool IsAgent(string objectName)
    //{
    //    // Kiểm tra xem tên đối tượng có khớp với mẫu "AgentXX" không
    //    return Regex.IsMatch(objectName, @"^Agent\d{2}$");
    //}

    private IEnumerator ReloadCurrentLevel()
    {
        countlv++;
        Debug.Log(countlv.ToString());
        if (countlv < 3)
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
            Destroy(gameObject);
        }
        else if(countlv == 3)
        {
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene("Boss");
        }
        else if(countlv >4)
        {
            yield return new WaitForSeconds(5f);
            SceneManager.LoadScene("Win");
            countlv = 0;
        }
    }
}
