using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterManager : MonoBehaviour
{
    public CharaterDatabase characterDB;

    public Text nameText;
    public SpriteRenderer artworkSprite;
    

    public int selectOption = 0;
    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("selectOption"))
        {
            selectOption = 0;
        }
        else
        {
            Load();
            // Kiểm tra xem có thông tin nhân vật đã lưu hay không và cập nhật nhân vật hiển thị
            if (PlayerPrefs.HasKey("characterName") && PlayerPrefs.HasKey("characterSprite"))
            {
                string characterName = PlayerPrefs.GetString("characterName");
                string characterSpriteName = PlayerPrefs.GetString("characterSprite");
                // Tìm nhân vật trong database dựa vào thông tin đã lưu
                for (int i = 0; i < characterDB.CharaterCount; i++)
                {
                    Character character = characterDB.GetCharacter(i);
                    if (character.characterName == characterName && character.characterSprite.name == characterSpriteName)
                    {
                        selectOption = i;
                        break;
                    }
                }
            }
        }
        UpdateCharater(selectOption);
    }

    public void NextOption()
    {
        selectOption++;

        if(selectOption >= characterDB.CharaterCount)
        {
            selectOption = 0;
        }

        UpdateCharater(selectOption);
        Save();
    }

    public void BackOption() 
    {
        selectOption--;

        if(selectOption <0)
        { 
            selectOption = characterDB.CharaterCount -1; 
        }
        UpdateCharater(selectOption);
        Save();
    }

    private void UpdateCharater(int selectedOption)
    {
        Character character = characterDB.GetCharacter(selectedOption);
        artworkSprite.sprite = character.characterSprite;
        nameText.text = character.characterName;
        
    }

    private void Load()
    {
        selectOption= PlayerPrefs.GetInt("selectOption");
    }

    private void Save()
    {
        PlayerPrefs.SetInt("selectOption", selectOption);
        // Lưu thông tin nhân vật đã chọn
        Character selectedCharacter = characterDB.GetCharacter(selectOption);
        PlayerPrefs.SetString("characterName", selectedCharacter.characterName);
        PlayerPrefs.SetString("characterSprite", selectedCharacter.characterSprite.name);
    }

    private void ChaneScreen(int sceneID)
    {
        SceneManager.LoadScene(sceneID);
    }
}
