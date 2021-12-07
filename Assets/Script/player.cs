using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Player : MonoBehaviour
{

    [SerializeField]
    private float _slowSpeed = 1.0f;
    // speed
    [SerializeField]
    private float _defaultSpeed = 4.0f;
    [SerializeField]
    private float _thrustSpeed = 8f;
    [SerializeField]
    private float _boostSpeed = 12f;
    [SerializeField]

    // laser
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
    private AudioClip _blastShellClip;
    [SerializeField]
    private GameObject _blastWave;
    [SerializeField]
    private GameObject _largeLaser;
    [SerializeField]
    private GameObject _missile;

    //thruster
    [SerializeField]
    private GameObject _thruster;
    private float _thrusterCharge = 0.7f;
    private ChargeBar  _chargeBar;
    private bool _thrusterDisable = false;

    [SerializeField]
    private GameObject  _leftEngines, _rightEngines; 
    private int _lives = 3;

    private int _score = 0;

    private int _ammoRounds = 15;

    private int _blastShellCount = 1;

    private int _missileCount = 3;

    private bool _gamePaused = false;


    ///// debuffs ///////
    private bool _speedDebuffed = false;

    ///// buffs ///////
    private bool _tripleShotActive = false;
    private float _tripleShotTimer = 0.0f;

    private bool _speedActive = false;
    private float _speedTimer = 0.0f;

    
    private int _shieldCount = 1;

    [SerializeField]
    private GameObject _shield;

    [SerializeField]
    private GameObject _camera;
    Vector3 _camPos;
    private SpawnManager _spawnManager;
    private UIManager _UIManager;




    // Start is called before the first frame update
    void Start()
    {

        //initiallize position 
        transform.position = Vector3.zero;
        _camPos = _camera.transform.position;
       


        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        _UIManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _laserAudio = GetComponent<AudioSource>();
        _chargeBar = GameObject.Find("ChargeBar").GetComponent<ChargeBar>();

        NullCheck();
        _laserAudio.clip = _laserAudioClip;

        ShieldUpdate();
        _UIManager.BlastShellUpdate(_blastShellCount);
        _UIManager.MissileUpdate(_missileCount);
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
        if (Input.GetKeyDown(KeyCode.F))
        {
            ShellBalst();
        }
        if (Input.GetKeyDown(KeyCode.E)){
            FireMissile();
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
        if (_chargeBar == null)
        {
            Debug.LogError("chargebar is Null");
        }
    }

    void PlayerMovement()
    {
        CharacterKeyBinding();
        CharacterLimit();
    }
    void CharacterKeyBinding()
    {

        float _speed = SpeedCalculation();

        //user input
        float _horizontalInput = Input.GetAxis("Horizontal");
        float _verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(_horizontalInput, _verticalInput, 0);

        transform.Translate(direction * _speed * Time.deltaTime);

    }

    float SpeedCalculation() {
        float _speed = _defaultSpeed;
        Vector3 _thrusterScale = new Vector3(0.5f, 0.75f, 0.75f);

        if ( Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
        {
            _thrusterScale =  Vector3.zero;
        }
        
        if (_speedDebuffed)
        {
            _speed = _slowSpeed;
            _thrusterScale = new Vector3(0.25f, 0.6f, 0.6f);
        } else if (_speedActive)
        {
            _speed = _boostSpeed;
            float anim = Mathf.Sin(Time.time * 10.0f) * 0.1f;
            _thrusterScale = new Vector3(1f + anim, 0.75f, 0.75f);

        }
        else if (Input.GetKey(KeyCode.LeftShift) && !_thrusterDisable)
        {
            _speed = _thrustSpeed;
            _thrusterScale = new Vector3(0.8f, 0.8f, 0.8f);
            _thrusterCharge -= Time.deltaTime*0.5f;

            if (_thrusterCharge < 0.05f)
            {
                _thrusterDisable = true;
            }
        }
        else if (_thrusterCharge < 1f)
        {
            _thrusterCharge += Time.deltaTime * 0.2f;

            if (_thrusterCharge > 0.3f && !Input.GetKey(KeyCode.LeftShift))
            {
                _thrusterDisable = false;
            }
            
        }
        else
        {
            _thrusterCharge = 1f;
        }

        _chargeBar.UpdateChargeBar(_thrusterCharge,_speedDebuffed);
        _thruster.transform.localScale = _thrusterScale;

        return _speed;
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

    void ShieldUpdate() {
        if (_shieldCount == 0)
        {
            _shield.SetActive(false);
        }
        else
        {
            _shield.SetActive(true);
        }
        _UIManager.ShieldUpDate(_shieldCount);
      
    }

    void ShellBalst() {
        if (_blastShellCount > 0)
        {
            _blastShellCount--;
            _UIManager.BlastShellUpdate(_blastShellCount);
            StartCoroutine(ShellBlastSeq());
        }
    
    }

    void FireMissile(){
        if (_missileCount >0)
        {   
            _missileCount --; 
            _UIManager.MissileUpdate(_missileCount);
            if (Random.Range(0,2) == 0)
            {
                Instantiate(_missile,transform.position + new Vector3(-1f,-0.7f,0f),Quaternion.Euler(Vector3.forward*90f));
            }else{
                Instantiate(_missile,transform.position + new Vector3(1f,-0.7f,0f),Quaternion.Euler(Vector3.forward*-90f));
            }            
        }        
    }

    void FireLaser()
    {
        GameObject _ammo = PowerUpLaser();

        if (Time.time > _nextFire && _ammoRounds > 0)
        {
            _nextFire = Time.time + _fireRate;
            _laserAudio.Play();
            _ammoRounds--;
            _UIManager.AmmoUpDate(_ammoRounds);
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
    IEnumerator ShellBlastSeq()
    {
        _blastWave.SetActive(true);
        float s = 1;
        while (s<10f)
        {
            s += 0.5f;
            yield return new WaitForSeconds(0.001f);
            _blastWave.transform.localScale = new Vector3(s, s, s);
        }

        _blastWave.SetActive(false);
    }

    IEnumerator BigLaserSeq(){
        _largeLaser.SetActive(true);
        Vector3 _scale = new Vector3(0f,0f,0f);
        while(_scale.x<10){  
            _scale.x+= Time.deltaTime*30f;   
            _scale.y+= Time.deltaTime;   
            _largeLaser.transform.localScale = _scale;
            yield return new WaitForSeconds( 0.01f);
        }
        while(_scale.x>5){  
        _scale.x-= Time.deltaTime*15f;   
        _scale.y+= Time.deltaTime*30f;   
                    _largeLaser.transform.localScale = _scale;
        yield return new WaitForSeconds( 0.01f);

        }
        yield return new WaitForSeconds( 3f);
        _largeLaser.SetActive(false);

    }

    IEnumerator SpeedDebufCountDown(){
      _speedDebuffed = true;
            yield return new WaitForSeconds(3f);
        _speedDebuffed = false;
    } 
    IEnumerator CameraShake() {
        float t = 0f;
        while (t < 0.5f)
        {
            _camera.transform.position = _camPos + Random.Range(-0.1f, 0.1f) * Vector3.right + Random.Range(-0.1f, 0.1f) * Vector3.up;
            yield return new WaitForSeconds(0.05f);
            t += 0.05f;
        }
        _camera.transform.position = _camPos;

    }

    //// public method ////

    public void TakeDamage()
    {
        if (_shieldCount>0)
        {
            _shieldCount--;
            ShieldUpdate();
            return;
        }
        StartCoroutine(CameraShake());
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

            //pickups 
    //buffs
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
        if (_shieldCount<3)
        {
            _shieldCount++;
        }
        ShieldUpdate();
    }
    public void AmmoCollected() {

        _ammoRounds+=5;
        Mathf.Min(_ammoRounds, 15);
        _UIManager.AmmoUpDate(_ammoRounds);

    }
    public void BlastShellCollected()
    {
        if (_blastShellCount<3)
        {
            _blastShellCount++;
            _UIManager.BlastShellUpdate(_blastShellCount);
        }
        
    }


    public void BigLaser(){

        StartCoroutine(BigLaserSeq());
    }
    public void MissileCollected()
    {
        if (_missileCount<5)
        {
            _missileCount++;
            _UIManager.MissileUpdate(_missileCount);
        }
        
    }
    //debuffs
    public void SpeedDebuff(){
        StartCoroutine(SpeedDebufCountDown());
    }




    public void PauseGame(bool pause)
    {
        _gamePaused = pause;
        _UIManager.PauseMenu(pause);
    }

}
