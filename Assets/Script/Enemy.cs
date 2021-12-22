using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3;
    float _randomOffset = 0;
    [SerializeField]
    private float _rotate = 0;

    private bool _shielded = false;
    [SerializeField]
    private GameObject _shield;
    [SerializeField]
    private AudioClip _loseShieldAudioClip;

    [SerializeField]
    private GameObject _laserPrefab;

    [SerializeField]
    private AudioClip _laserAudioClip;

    private Player _player;

    private Animator _animator;
    private bool _exploding = false;

    [SerializeField]
    private AudioClip _explosionClip;
    private AudioSource _Audio;

    [SerializeField]
    private int _enemyID = 0;
    /*
    0 = astroid
    1 = classic
    2 = drop
    3 = centipede
    */

    private bool _setSpecial = false;
    [SerializeField]
    private int _special = -1;
    /*
    0 = none
    1 = fire
    2 = sheild
    3 = follow
    4 = zigzag
    5 = turn around shoot
    6 = dodge
    */
    [SerializeField]
    private GameObject _astroidExplosion;

    [SerializeField]
    private GameObject _drop;
    private SpawnManager _spawnManager;
    private AudioManager _audioManager;


    // Start is called before the first frame update
    void Start()
    {
        _audioManager = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
        _player = GameObject.Find("Player").GetComponent<Player>();
        _animator = GetComponent<Animator>();
        _Audio = GetComponent<AudioSource>();

        _Audio.volume = _audioManager.GetVolume();

        NullCheck();

        InitID();
        _randomOffset = Random.Range(0f, 1f) - 0.5f;
    }

    void NullCheck()
    {
        if (_audioManager == null && _enemyID != 0)
        {
            Debug.LogError("audio Manager is Null");
        }
        if (_player == null)
        {
            Debug.LogError("player is null");
        }
        if (_Audio == null && _enemyID != 0)
        {
            Debug.LogError("audio source is Null");
        }
        if (_animator == null && _enemyID != 0)
        {
            Debug.LogError("animator is null");
        }

    }

    void InitID()
    {
        switch (_enemyID)
        {
            case 0:
                AsteroidInit();
                break;
            case 1:
                InitType();
                break;
            case 2:
                StartCoroutine(DropLaser());
                break;
            case 3:
                break;
            default:
                break;
        }
    }
    void AsteroidInit()
    {
        if (_enemyID == 0)
        {
            _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
            if (_spawnManager == null)
            {
                Debug.LogError("spawn manager is null");
            }
        }
    }
    void InitType()
    {
        if (_shield != null)
        {
            _shield.SetActive(false);
        }
        _setSpecial = true;
        switch (_special)
        {
            case 0:
                break;
            case 1:
                StartCoroutine(ShootLaser());
                break;
            case 2:
                SetShield(true);
                break;
            case 3:
                StartCoroutine(Follow());
                break;
            case 4:
                StartCoroutine(Zigzag());
                break;
            case 5:
                StartCoroutine(TurnAround());
                break;
            case 6:
                StartCoroutine(Dodge());
                break;
            default:
                _setSpecial = false;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (!_setSpecial)
        {
            InitType();
        }
        else if (!_exploding)
        {
            if (_player != null)
            {
                EnemyMovement();
            }
            else
            {
                Destroy(gameObject);
            }
        }
        BottomEdge();
        SideEdge();
    }

    void EnemyMovement()
    {
        Vector3 _translation = Vector3.zero;
        Vector3 _rotation = Vector3.zero;
        switch (_enemyID)
        {
            case 0:
                _rotation = Vector3.forward * _rotate * Time.deltaTime;
                break;
            case 1:
                _translation = Vector3.down * _speed * Time.deltaTime;
                break;
            case 2:
                _translation = Vector3.right * _speed * Time.deltaTime;
                break;
            case 3:
                _rotation = Vector3.forward * _rotate * Time.deltaTime;
                float _move = transform.localRotation.z + 1f;
                _move = (_move + 0.5f) % 2f - 1f;
                _move = Mathf.Pow(2 * _move, 2f);
                _translation = Vector3.down * _speed * Time.deltaTime;
                _translation.y -= _move * Time.deltaTime * 2f;
                break;
            default:
                break;
        }

        transform.Rotate(_rotation, Space.Self);
        transform.Translate(_translation);

        BottomEdge();
        SideEdge();
    }

    void BottomEdge()
    {
        if (transform.position.y < -5)
        {
            // float _x = Random.Range(-8f, 8f);
            // transform.position = new Vector3(_x, 6, 0); //reset to top
            Destroy(gameObject);
        }

    }

    void SideEdge()
    {
        if (Mathf.Abs(transform.position.x) > 15)
        {
            Destroy(gameObject);
        }

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!_exploding)
        {
            if (other.tag == "Player")
            {
                PlayerCollision(other);
            }

            if (other.tag == "Laser")
            {
                LaserCollision(other);
            }

            if (other.tag == "Blast" || other.tag == "LargeLaser")
            {
                BlastCollision(other);
            }
            if (other.tag == "Token")
            {
                Destroy(other.gameObject);
            }
        }
    }
    private void PlayerCollision(Collider2D other)
    {
        if (_player != null)
        {
            _player.TakeDamage();
        }
        _shield.SetActive(false);
        ExplosionAnimation();
    }
    private void LaserCollision(Collider2D other)
    {
        Destroy(other.gameObject);
        if (_shielded)
        {
            SetShield(false);

        }
        else
        {
            if (_player)
            {
                _player.UpdateScore(10);
            }
            ExplosionAnimation();
            DropLoot();
        }
    }
    private void BlastCollision(Collider2D other)
    {
        if (_player)
        {
            _player.UpdateScore(10);
        }
        ExplosionAnimation();
        DropLoot();
    }

    private void ExplosionAnimation()
    {
        _exploding = true;
        AudioSource.PlayClipAtPoint(_explosionClip, transform.position);

        if (_enemyID == 1)
        {
            _animator.SetTrigger("EnemyDestroyed");
            Destroy(this.gameObject, 2.5f);
        }
        else
        {
            GameObject _explode = Instantiate(_astroidExplosion, transform.position, Quaternion.identity);
            switch (_enemyID)
            {
                case 0:
                    _spawnManager.StartSpawning(0);
                    break;
                case 3:
                    _explode.transform.localScale = Vector3.one * 0.25f;
                    break;
                default:
                    break;
            }
            gameObject.SetActive(false);
            Destroy(gameObject, 2f);
        }
    }

    private void DropLoot()
    {
        if (_drop != null && Random.Range(0f, 1f) < 0.5f)
        {
            Instantiate(_drop, transform.position + Vector3.down, Quaternion.identity);
        }
    }

    private void SetShield(bool sheild)
    {
        _shielded = sheild;
        _shield.SetActive(sheild);
        if (!sheild)
        {
            _Audio.clip = _loseShieldAudioClip;
            _Audio.Play();
        }
    }

    IEnumerator ShootLaser()
    {

        float _laserTime = 2f;
        while (!_exploding)
        {
            GameObject _laser = Instantiate(_laserPrefab, transform.position, transform.rotation);

            _Audio.clip = _laserAudioClip;
            _Audio.Play();
            yield return new WaitForSeconds(_laserTime);

            // AudioSource.PlayClipAtPoint(_laserAudioClip,transform.position);        
        }
    }
    IEnumerator Follow()
    {
        bool follow = true;
        while (follow && !_exploding)
        {
            follow = Mathf.Abs(transform.localRotation.z) < 0.5f;
            if (_player != null)
            {
                Vector3 _dir = _player.transform.position - transform.position;
                float _dist = Vector3.Magnitude(_dir);
                float angle = Vector3.Angle(_dir, transform.right);
                angle -= 90f;
                Vector3 _turn = new Vector3(0f, 0f, -1f / _dist * Mathf.Sign(angle) * Mathf.Pow(Mathf.Abs(angle), 0.5f));
                transform.Rotate(_turn, Space.Self);
                transform.Translate(Vector3.down / _dist);

                yield return new WaitForSeconds(0.05f);
            }
        }
    }
    IEnumerator Zigzag()
    {
        while (!_exploding)
        {
            float _randomAngle = Random.Range(0, 360f);
            if (_randomAngle > 90 && _randomAngle < 270)
            {
                if (transform.position.y > 8f || Random.Range(0, 1f) < 0.5f)
                {
                    _randomAngle += 180;
                }
            }
            Quaternion _turn = Quaternion.identity;
            _turn.eulerAngles = new Vector3(0, 0, _randomAngle);
            transform.rotation = _turn;
            yield return new WaitForSeconds(1.5f);
        }
    }

    IEnumerator TurnAround()
    {
        bool _pass = true;
        while (_pass)
        {
            if (_player != null)
            {
                _pass = transform.position.y > _player.transform.position.y;
            }
            yield return new WaitForSeconds(0.1f);
        }

        if (_player != null && !_exploding)
        {
            if (transform.position.x > _player.transform.position.x)
            {
                transform.Rotate(Vector3.forward * -90, Space.Self);
            }
            else
            {
                transform.Rotate(Vector3.forward * 90, Space.Self);
            }
        }
        StartCoroutine(ShootLaser());

        while (!_exploding)
        {
            if (_player != null)
            {
                Vector3 _dir = _player.transform.position - transform.position;
                float angle = Vector3.Angle(_dir, transform.right);
                angle -= 90f;
                Vector3 _turn = new Vector3(0f, 0f, -1f / Mathf.Sign(angle) * Mathf.Pow(Mathf.Abs(angle), 0.5f));
                transform.Rotate(_turn, Space.Self);
                yield return new WaitForSeconds(0.05f);
            }
        }
    }

    IEnumerator Dodge()
    {
        while (!_exploding)
        {
            Vector2 _origin = new Vector2(transform.position.x, transform.position.y - 1.5f);
            Vector2 _size = new Vector2(1f, 1f);
            Vector2 _dir = Vector2.down;
            RaycastHit2D hit = Physics2D.BoxCast(_origin, _size, 0, _dir);
            if (hit.collider != null)
            {
                if (hit.transform.tag == "Laser")
                {
                    float _dif = hit.transform.position.x - transform.position.x;
                    if (Mathf.Abs(_dif) > 0.25f)
                    {
                        for (int i = 0; i < 10; i++)
                        {
                            float _dodge = 3f * Mathf.Sign(_dif);
                            transform.Translate(Vector3.left * _dodge * Time.deltaTime);
                            yield return new WaitForEndOfFrame();
                        }

                    }
                }
            }
            yield return new WaitForSeconds(0.5f);
        }

    }

    IEnumerator DropLaser()
    {
        float _laserTime = 1f;
        while (!_exploding)
        {
            GameObject _laser = Instantiate(_laserPrefab, transform.position + new Vector3(0.25f, -0.4f, 0), _laserPrefab.transform.rotation, gameObject.transform);
            _Audio.Play();
            yield return new WaitForSeconds(_laserTime);
        }
    }

    ///// public method /////
    public void SetType(int _type)
    {
        _special = _type;
    }

}
