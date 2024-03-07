using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public SimpleRandomWalkDungeonGenerator dungeonGenerator;
    public void SelectCharacter()
    {
        SceneManager.LoadScene("Character Selection");
        
    }
    public void Shop()
    {
        SceneManager.LoadScene("Pet Selection");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void Play()
    {
        SceneManager.LoadScene("Level 1");

    }
}
