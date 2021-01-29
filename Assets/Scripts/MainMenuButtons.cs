using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    private DrawCards drawCards;

    public void LoadGameScene()
    {
        SceneManager.LoadScene("GameBoard");
    }

    public void LoadMenuScene()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}