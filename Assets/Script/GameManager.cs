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

            if (Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene(1);
                _isGameOver = false;
            }

        }

    }
    private void ClearScreen()
    {
        GameObject[] collection;
        collection = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject item in collection)
        {
            Destroy(item);
        }
        collection = GameObject.FindGameObjectsWithTag("Laser");
        foreach (GameObject item in collection)
        {
            Destroy(item);
        }
        collection = GameObject.FindGameObjectsWithTag("Laser");
        foreach (GameObject item in collection)
        {
            Destroy(item);
        }
    }

    public void pauseGame(bool pauseGame)
    {
        Time.timeScale = pauseGame ? 0 : 1;

    }
    public void quitGame()
    {
        // Application.Quit();  // for desktop
        SceneManager.LoadScene(0);

    }


    public void GameOver()
    {
        _isGameOver = true;
        ClearScreen();
    }


}
