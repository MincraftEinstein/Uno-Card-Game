using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    public GameObject pauseMenu;
    private TurnManager turnManager;

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "GameBoard")
        {
            //pauseMenu = GameObject.Find("PauseMenu");
            turnManager = GameObject.Find("GameManager").GetComponent<TurnManager>();
        }
    }

    //public void RoundButton(string sceneName)
    //{
    //    TurnManager turnManager = GameObject.Find("GameManager").GetComponent<TurnManager>();
    //    if (turnManager.GameOver == false)
    //    {
    //        PlayerPrefs.SetInt("playerScore", turnManager.PlayerScoreInt);
    //        PlayerPrefs.SetInt("enemy1Score", turnManager.Enemy1ScoreInt);
    //        PlayerPrefs.SetInt("enemy2Score", turnManager.Enemy2ScoreInt);
    //        PlayerPrefs.SetInt("enemy3Score", turnManager.Enemy3ScoreInt);
    //    }
    //    for (int i = 0; i < DrawCards.PlayedCards.Count; i++)
    //    {
    //        DrawCards.PlayedCards.Remove(DrawCards.PlayedCards[i]);
    //    }
    //    TurnManager.Turns = 0;
    //    SceneManager.LoadScene(sceneName);
    //}

    public void LoadScene(string sceneName)
    {
        string currentScene = SceneManager.GetActiveScene().name;
        PlayerPrefs.SetInt("playerScore", 0);
        PlayerPrefs.SetInt("enemy1Score", 0);
        PlayerPrefs.SetInt("enemy2Score", 0);
        PlayerPrefs.SetInt("enemy3Score", 0);
        if (currentScene == "GameBoard")
        {
            //TurnManager turnManager = GameObject.Find("GameManager").GetComponent<TurnManager>();
            if (turnManager.gameOver == false)
            {
                PlayerPrefs.SetInt("playerScore", turnManager.PlayerScoreInt);
                PlayerPrefs.SetInt("enemy1Score", turnManager.Enemy1ScoreInt);
                PlayerPrefs.SetInt("enemy2Score", turnManager.Enemy2ScoreInt);
                PlayerPrefs.SetInt("enemy3Score", turnManager.Enemy3ScoreInt);
            }
            for (int i = 0; i < DrawCards.PlayedCards.Count; i++)
            {
                DrawCards.PlayedCards.Remove(DrawCards.PlayedCards[i]);
            }
            TurnManager.Turns = 0;
        }
        if (sceneName == "MainMenu")
        {
            Time.timeScale = 1;
        }
        SceneManager.LoadScene(sceneName);
        SceneManager.UnloadSceneAsync(currentScene);
    }

    public void Quit()
    {
        Debug.Log("Exiting application");
        Application.Quit();
    }

    public void PauseButton()
    {
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
        turnManager.isPaused = true;
        Debug.Log("Game paused");
    }

    public void PlayButton()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        turnManager.isPaused = false;
        Debug.Log("Game unpaused");
    }
}