using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;
    float _randomOffset = 0;
    [SerializeField]
    private bool _debuff = false;
    /*
   0 = slow
   */
    [SerializeField]
    private int powerupID = 0;
    /*
    0 = triple shot 
    1 = speed
    2 = sheild 
    3 = ammo
    4 = blast shell
    5 = big laser
    6 = missile
    7 = life up
    */
    [SerializeField]
    private AudioClip _pickupAudio;

    [SerializeField]
    private GameObject _explosionPrefab;

    private Player _player;
    private AudioManager _audioManager;



    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _audioManager = GameObject.Find("Audio Manager").GetComponent<AudioManager>();

        if (_player == null)
        {
            Debug.LogError("player is null");
        }
        if (_audioManager == null)
        {
            Debug.LogError("Audio Manager is null");
        }
        _randomOffset = Random.Range(0f, 1f) - 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * (_speed + _randomOffset)  * Time.deltaTime);
        if (_player != null)
        {
            Magnetize();
        }
        else
        {
            Destroy(gameObject);
        }
        if (transform.position.y < -5)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            if (!_audioManager.GetMuted())
            {
                AudioSource.PlayClipAtPoint(_pickupAudio, transform.position, _audioManager.GetVolume());
            }
            if (_debuff)
            {
                Debuffs(player);
            }
            else
            {
                Buffs(player);
            }


            Destroy(gameObject);
        }
        if (other.tag == "Enemy" && !_debuff)
        {
            GameObject _explosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            _explosion.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            Destroy(gameObject);
        }
    }
    void Debuffs(Player player)
    {
        switch (powerupID)
        {
            case 0:
                player.SpeedDebuff();
                break;
            default:
                break;
        }

    }

    void Buffs(Player player)
    {
        switch (powerupID)
        {
            case 0:
                player.TripleShotCollected();
                break;
            case 1:
                player.SpeedBoostCollected();
                break;
            case 2:
                player.ShieldCollected();
                break;
            case 3:
                player.AmmoCollected();
                break;
            case 4:
                player.BlastShellCollected();
                break;
            case 5:
                player.BigLaser();
                break;
            case 6:
                player.MissileCollected();
                break;
            case 7:
                player.LifeCollected();
                break;
            default:
                break;
        }
    }

    void Magnetize()
    {

        Vector3 _diff = _player.transform.position - transform.position;
        float _dist = Vector3.Magnitude(_diff);
        // Debug.Log(_dist);
        if (Input.GetKey(KeyCode.C) && _dist < 3f)
        {
            transform.Translate(Vector3.Normalize(_diff) / Mathf.Pow(_dist, 2f) * Time.deltaTime);
        }
    }

}
