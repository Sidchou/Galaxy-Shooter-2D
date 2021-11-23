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
    private Image[] _shieldUI;
    [SerializeField]
    private GameObject _ammo;
    [SerializeField]
    private Image _roundPrefab;
    private Image[] _rounds = new Image[15];
    [SerializeField]
    private Image[] _shellUI;


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
        AmmoSetup();
        ScoreUpDate(0);
        LivesUpDate(3);
        GameOver(false);

        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        if (_gameManager == null)
        {
            Debug.LogError("GameManager is null");

        }

        BlastShellUpdate(1);
    }

    void AmmoSetup() {
        for (int i = 0; i < _rounds.Length; i++)
        {
            _rounds[i] = Instantiate(_roundPrefab, _ammo.transform.position + Vector3.right * i*20, Quaternion.identity);
            //_rounds[i].transform.parent = _ammo.transform;
            _rounds[i].transform.SetParent(_ammo.transform,true);
        }
        AmmoUpDate(15);


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
    public void ShieldUpDate(int Shields) {
        for (int i = 0; i < _shieldUI.Length; i++)
        {
            if (i < Shields)
            {
                _shieldUI[i].enabled = true;

            }
            else
            {
                _shieldUI[i].enabled = false;

            }
        }
    }
    public void AmmoUpDate(int rounds)
    {
        for (int i = 0; i < _rounds.Length; i++)
        {
            Vector4 color = _rounds[i].color; 

            if (i < rounds)
            {
                color.w = 1f;
            }
            else
            {
                color.w = 0.2f;

            }
            _rounds[i].color = color;

        }
    }
    public void BlastShellUpdate(int shells) {
        for (int i = 0; i < _shellUI.Length; i++)
        {
            if (i < shells)
            {
                _shellUI[i].enabled = true;
            }
            else
            {
                _shellUI[i].enabled = false;

            }
        }
    
    }

    public void PauseMenu(bool isPaused)
    { 
        _menu.SetActive(isPaused);
        _gameManager.pauseGame(isPaused);
    }







}
