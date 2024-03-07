using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Finish : MonoBehaviour
{
    public CoinCount coinCounter;
    private bool levelComplete = false;
    private RoomFirstDungeonGenerator roomFirstDungeonGenerator;
    public TMP_Text level;
    private int count = 1;
    private void OnTriggerEnter2D(Collider2D collision)
    {

        string objectName = collision.gameObject.name;
        if (!levelComplete && collision.tag == "Player"
            && GameObject.FindGameObjectWithTag("Boss") == null
            && GameObject.FindGameObjectWithTag("Enemy") == null)
        {
            
            levelComplete = true;
            GameObject player = GameObject.FindGameObjectWithTag("Player");
         coinCounter = FindObjectOfType<CoinCount>();
            int data = coinCounter.coinNumber;
            
            Coins c = new Coins(data);
            string json = JsonUtility.ToJson(c);
            File.WriteAllText(Application.dataPath + "/_Scripts/PlayerData/CoinConfig.json", json);
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
            level.text = "Level " + count.ToString();
            count++;
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
    public class Coins
    {
        public int coinValue;

        public Coins(int coinValue)
        {
            this.coinValue = coinValue;
        }
    }
}
