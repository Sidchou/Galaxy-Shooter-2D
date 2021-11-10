using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _ScoreBoard;
    [SerializeField]
    private Sprite[] _livesSprite;
    [SerializeField]
    private Image _livesDisplay;
    [SerializeField]
    private Text _GameOverText;
    [SerializeField]
    private Text _RestartText;
    [SerializeField]
    private GameObject _menu;


    private GameManager _gameManager;

    // Start is called before the first frame update
    void Start()
    {
        ScoreUpDate(0);
        LivesUpDate(3);
        GameOver(false);

        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        if (_gameManager == null)
        {
            Debug.LogError("GameManager is null");

        }
    }
     void GameOver(bool end) {
        StartCoroutine(TextFlicker(end));
        _RestartText.gameObject.SetActive(end);
    }

    ///// Coroutine ////

    IEnumerator TextFlicker(bool end) 
    {
        while (end) {

            _GameOverText.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            _GameOverText.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);

        }
        _GameOverText.gameObject.SetActive(false);
    }


    ///// Public Method ////

    public void ScoreUpDate( int score){
        string _text = "Score: " + score;
        _ScoreBoard.text = _text;
    
    }

    public void LivesUpDate(int lives)
    {
        _livesDisplay.sprite =_livesSprite[lives];
        if (lives == 0)
        {
            GameOver(true);
            _gameManager.GameOver();
        }
    }

    public void PauseMenu(bool isPaused)
    { 
        _menu.SetActive(isPaused);
        _gameManager.pauseGame(isPaused);
    }







}
