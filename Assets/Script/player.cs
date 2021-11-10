using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Player : MonoBehaviour
{

    [SerializeField]
    private float _defaultSpeed = 5.0f;
    [SerializeField]
    private float _boostSpeed = 8f;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private float _fireRate = 0.2f;
    private float _nextFire = 0.0f;

    [SerializeField]
    private AudioClip _laserAudioClip;
    private AudioSource _laserAudio;


    [SerializeField]
    private GameObject  _leftEngines, _rightEngines; 
    private int _lives = 3;
    private int _score = 0;

    private bool _gamePaused = false;


    ///// buffs ///////

 

    private bool _tripleShotActive = false;
    private float _tripleShotTimer = 0.0f;

    private bool _speedActive = false;
    private float _speedTimer = 0.0f;

    private bool _shieldActive = false;
    [SerializeField]
    private GameObject _shield;



    private SpawnManager _spawnManager;
    private UIManager _UIManager;




    // Start is called before the first frame update
    void Start()
    {

        //initiallize position 
        transform.position = Vector3.zero;

        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        _UIManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _laserAudio = GetComponent<AudioSource>();

        NullCheck();
        _laserAudio.clip = _laserAudioClip;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();

        if (Input.GetKeyDown(KeyCode.Space) && !_gamePaused)
        {
            FireLaser();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame(!_gamePaused);
        }

    }

    void NullCheck() {

        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is NULL");
        }

        if (_UIManager == null)
        {
            Debug.LogError("UI Manager is Null");
        }
            if (_laserAudio == null)
        {
            Debug.LogError("laser audio is Null");
        }
    }

    void PlayerMovement()
    {
        CharacterKeyBinding();
        CharacterLimit();
    }
    void CharacterKeyBinding()
    {

        float _speed = _speedActive ? _boostSpeed : _defaultSpeed;

        //user input
        float _horizontalInput = Input.GetAxis("Horizontal");
        float _verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(_horizontalInput, _verticalInput, 0);

        transform.Translate(direction * _speed * Time.deltaTime);

    }
    void CharacterLimit()
    {
        //player xy limit
        Vector3 _setPLayerPosition = transform.position;
        //vertical limit
        float _yClamp = Mathf.Clamp(transform.position.y, -4f, 4f);
        _setPLayerPosition.y = _yClamp;
        //horizontal wrap
        if (Mathf.Abs(transform.position.x) > 10)
        {
            _setPLayerPosition.x = -9.5f * Mathf.Sign(_setPLayerPosition.x);
        }
        //set position
        transform.position = _setPLayerPosition;
    }

    void EngingUpdate() {
        if (_lives == 2)
        {
            _leftEngines.SetActive(true);
        }
        else 
        {
            _rightEngines.SetActive(true);

        }


    }

    void FireLaser()
    {
        GameObject _ammo = PowerUpLaser();

        if (Time.time > _nextFire)
        {
            _nextFire = Time.time + _fireRate;
            _laserAudio.Play();
            Instantiate(_ammo, transform.position + new Vector3(0, 1.25f, 0), Quaternion.identity);
        }
    }
    GameObject PowerUpLaser()
    {
        if (_tripleShotActive)
        {
            return _tripleShotPrefab;
        }
        else
        {
            return _laserPrefab;
        }
    }

    IEnumerator TripleShotCountDown()
    {
        while (_tripleShotTimer < 5.0f)
        {
            yield return new WaitForSeconds(0.5f);
            _tripleShotTimer += 0.5f;
        }
        _tripleShotTimer = 0.0f;
        _tripleShotActive = false;
    }

    IEnumerator SpeedBoostCountDown()
    {
        while (_speedTimer<5.0f)
        {
            yield return new WaitForSeconds(0.5f);
            _speedTimer += 0.5f;
        }
        _speedTimer = 0.0f;
        _speedActive = false;
    }

    //// public method ////

    public void TakeDamage()
    {
        if (_shieldActive)
        {
            _shield.SetActive(false);
            _shieldActive = false;
            return;
        }
        _lives--;
        EngingUpdate();
        _UIManager.LivesUpDate(_lives);
        if (_lives <= 0)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }
    }

    public void UpdateScore( int point) {
        _score += point;
        _UIManager.transform.GetComponent<UIManager>().ScoreUpDate(_score);
    }

    public void TripleShotCollected()
    {
        _tripleShotTimer = 0.0f;
        if (!_tripleShotActive)
        {
            StartCoroutine(TripleShotCountDown());
        }
        _tripleShotActive = true;
    }

    public void SpeedBoostCollected() {
        _speedTimer = 0f;
        if (!_speedActive)
        {
            StartCoroutine(SpeedBoostCountDown());
        }
        _speedActive = true;
    }

    public void ShieldCollected()
    {
        _shieldActive = true;
        _shield.SetActive(true); 
    }

    public void PauseGame(bool pause)
    {
        _gamePaused = pause;
        _UIManager.PauseMenu(pause);
    }



}
