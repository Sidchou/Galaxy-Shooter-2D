using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool _isGameOver;
    private void Start()
    {

    }

    private void Update()
    {
        if (_isGameOver)
        {

            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(1);
                _isGameOver = false;
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene(0);
            }

        }

    }


    public void pauseGame(bool pauseGame)
    {
        Time.timeScale = pauseGame ? 0 : 1;
        
    }
    public void quitGame() {
        Application.Quit();
    
    }


    public void GameOver() {
        _isGameOver = true;
    }


}
