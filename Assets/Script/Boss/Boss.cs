using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    private int _stage = 0;
    private BossWing[] _wings;
    private float _bossHP = 500;
    [SerializeField]
    GameObject _shield;
    [SerializeField]
    GameObject _laser;
    [SerializeField]
    GameObject _largeLaser;
    [SerializeField]
    GameObject _fanLaser;
    [SerializeField]
    GameObject _explosion;

    [SerializeField]
    private BossUI _healthDisplay;
    private bool _takingDamage = false;
    private bool _largeLaserHit = false;

    [SerializeField]
    private AudioClip _laserAudioClip;
    [SerializeField]
    private AudioClip _explodeAudioClip;
    private AudioSource _audioSource;
    private AudioManager _audioManager;
    private UIManager _UIManager;


    Player _player;
    // Start is called before the first frame update
    void Start()
    {
        _audioManager = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
        _player = GameObject.Find("Player").GetComponent<Player>();
        _audioSource = GetComponent<AudioSource>();
        _UIManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        NullCheck();

        _audioSource.volume = _audioManager.GetVolume();
        _audioSource.mute = _audioManager.GetMuted();

        transform.position = Vector3.up * 14f;
        StartCoroutine(EnterSequence());
    }
    void NullCheck()
    {

        if (_audioManager == null)
        {
            Debug.LogError("audio source is null");
        }
        if (_player == null)
        {
            Debug.LogError("player is null");
        }
        if (_audioSource == null)
        {
            Debug.LogError("audio source is null");
        }
        if (_UIManager == null)
        {
            Debug.LogError("UI Manager is null");
        }

        _wings = GetComponentsInChildren<BossWing>();
        foreach (var item in _wings)
        {
            if (item == null)
            {
                Debug.LogError("wing is null");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z) == true)
        {
            UpdateHealth(100);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (_takingDamage == true)
        {
            if (other.tag == "Laser")
            {
                Destroy(other);
                UpdateHealth(10);
                ShotFX(transform.position);

            }
            if (other.tag == "Blast")
            {
                UpdateHealth(20);
            }
            if (other.tag == "LargeLaser")
            {
                StartCoroutine(LargeLaserDamage());
            }
            if (other.tag == "Player")
            {
                UpdateHealth(5);
            }
        }


        if (other.tag == "Player")
        {
            Player _player = other.GetComponent<Player>();
            if (_player != null)
            {
                _player.TakeDamage();
            }
        }

    }


    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "LargeLaser")
        {
            _largeLaserHit = true;
        }
        else
        {
            _largeLaserHit = false;
        }
    }

    void ShotFX(Vector3 _pos)
    {
        ExplodeAudio();
        GameObject _exp = Instantiate(_explosion, _pos, Quaternion.identity, gameObject.transform);
        _exp.transform.localScale = Vector3.one * 0.1f;
    }

    IEnumerator LargeLaserDamage()
    {
        while (_largeLaserHit)
        {
            UpdateHealth(5);
            Destroyed();
            yield return new WaitForSeconds(0.5f);
        }
    }



    IEnumerator EnterSequence()
    {
        while (transform.position.y > 5.5f)
        {
            transform.Translate(Vector3.down * Time.deltaTime * 2f);
            yield return new WaitForEndOfFrame();
        }
        transform.position = Vector3.up * 5.5f;

        _healthDisplay.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        _stage = 1;
        StartCoroutine(Stage1Movement());
        foreach (var item in _wings)
        {
            item.StageOne();
        }
    }
    IEnumerator Stage1Movement()
    {
        while (_stage < 4 && transform.position.x > -3f)
        {
            transform.Translate(Vector3.left * Time.deltaTime * 2f);
            yield return new WaitForEndOfFrame();
        }
        while (_stage < 4 && transform.position.x < 3f)
        {
            transform.Translate(Vector3.right * Time.deltaTime * 2f);
            yield return new WaitForEndOfFrame();
        }
        if (_stage < 4)
        {
            StartCoroutine(Stage1Movement());
        }
        else
        {
            StartCoroutine(Stage4Movement());
        }
    }

    IEnumerator Stage4Movement()
    {
        float _followTimer = 0;

        while (_player != null && _followTimer < 2f)
        {
            float _dif = _player.transform.position.x - transform.position.x;
            if (_dif > 0.5f)
            {
                transform.Translate(Vector3.right * Time.deltaTime * 2f);
            }
            else if (_dif < -1.5f)
            {
                transform.Translate(Vector3.left * Time.deltaTime * 2f);
            }
            if (Mathf.Abs(_dif) < 2f)
            {
                _followTimer += Time.deltaTime;
            }
            yield return new WaitForEndOfFrame();
        }

        while (transform.position.y > -1.5f)
        {
            transform.Translate(Vector3.down * Time.deltaTime * 4.5f);
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(1f);

        while (transform.position.y <= 5.5f)
        {
            transform.Translate(Vector3.up * Time.deltaTime * 2f);
            yield return new WaitForEndOfFrame();
        }

        StartCoroutine(FireLaser2());
    }


    IEnumerator FX()
    {
        while (true)
        {
            Vector3 _fxOffset = Vector3.up * Random.Range(-1.5f, 2f) + Vector3.right * Random.Range(-4f, 4f);
            GameObject _exp = Instantiate(_explosion, transform.position + _fxOffset, Quaternion.identity, gameObject.transform);
            _exp.transform.localScale = Vector3.one * Random.Range(0.2f, 0.5f);
            _audioSource.clip = _explodeAudioClip;
            _audioSource.Play();
            yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));
        }
    }

    IEnumerator FireLaser1()
    {
        Vector3[] _laserOffset = {
            new Vector3(2.8f, -1.5f, 0f),
            new Vector3(1.4f, -1.7f, 0f),
            new Vector3(-1.4f, -1.7f, 0f),
            new Vector3(-2.8f, -1.5f, 0f),
            };
        int i = 0;
        while (_stage == 3 && i < 4)
        {
            LaserAudio();
            Instantiate(_laser, transform.position + _laserOffset[i], _laser.transform.rotation);
            yield return new WaitForSeconds(0.2f);
            i++;
        }
        yield return new WaitForSeconds(1f);

        StartCoroutine(BigLaserSeq());
    }
    IEnumerator BigLaserSeq()
    {
        _largeLaser.SetActive(true);
        Vector3 _scale = new Vector3(0f, 0f, 0f);
        while (_scale.x < 10)
        {
            _scale.x += Time.deltaTime * 30f;
            _scale.y += Time.deltaTime;
            _largeLaser.transform.localScale = _scale;
            yield return new WaitForSeconds(0.01f);
        }
        while (_scale.x > 5)
        {
            _scale.x -= Time.deltaTime * 15f;
            _scale.y += Time.deltaTime * 30f;
            _largeLaser.transform.localScale = _scale;
            yield return new WaitForSeconds(0.01f);

        }
        yield return new WaitForSeconds(2f);

        _scale = new Vector3(0f, 0f, 0f);
        _largeLaser.transform.localScale = _scale;
        _largeLaser.SetActive(false);
        if (_stage == 3)
        {
            StartCoroutine(FireLaser1());
        }
    }

    IEnumerator FireLaser2()
    {
        int i = 0;
        while (i < 3)
        {
            LaserAudio();
            Instantiate(_fanLaser, transform.position, Quaternion.identity, gameObject.transform);
            yield return new WaitForSeconds(1.5f);
            i++;
        }
        StartCoroutine(Stage4Movement());
    }
    void StageThree()
    {
        StartCoroutine(FX());
        StartCoroutine(FireLaser1());
    }
    void Destroyed()
    {
        GameObject _exp = Instantiate(_explosion, transform.position, Quaternion.identity);
        _exp.transform.localScale = Vector3.one * 1.5f;
        Instantiate(_explosion, transform.position + new Vector3(4f, 2.75f, 0f), Quaternion.identity);
        Instantiate(_explosion, transform.position + new Vector3(-5.5f, 1.5f, 0f), Quaternion.identity);

        AudioSource.PlayClipAtPoint(_explodeAudioClip, transform.position);

        _UIManager.Victory();
        Destroy(gameObject);
    }


    public void UpdateHealth(float _damamage)
    {
        _bossHP -= _damamage;
        float _totalHP = _bossHP / 100 / 2;

        //health display calc
        if (_totalHP > 1f)
        {
            _totalHP = 1 + ((_totalHP - 1) / 1.5f);
        }

        //stages
        if (_totalHP < 1.6f && _stage == 1)
        {
            _stage = 2;
            _wings = GetComponentsInChildren<BossWing>();
            foreach (var item in _wings)
            {
                if (item != null)
                {
                    item.StageTwo();
                }
            }
        }
        if (_totalHP <= 1f && _stage <= 2)
        {
            _takingDamage = true;
            _stage = 3;
            _shield.SetActive(false);
            StageThree();
        }
        if (_totalHP <= 0.5f && _stage <= 3)
        {
            _stage = 4;
        }
        if (_totalHP <= 0f)
        {
            Destroyed();
        }
        _healthDisplay.UpdateBossHealth(_totalHP);
    }

    public bool Targetable()
    {
        return _takingDamage;
    }
    public void LaserAudio()
    {
        _audioSource.clip = _laserAudioClip;
        _audioSource.Play();

    }
    public void ExplodeAudio()
    {
        _audioSource.clip = _explodeAudioClip;
        _audioSource.Play();
    }
}
