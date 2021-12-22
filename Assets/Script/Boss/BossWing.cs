using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWing : MonoBehaviour
{
    private float _health = 50;
    private bool _exploding = false;
    [SerializeField]
    private GameObject _explosion;
    [SerializeField]
    private GameObject _drop;
    [SerializeField]
    private GameObject _laser;

    private Vector3 _position1;
    [SerializeField]
    private Vector3 _position2;
    private bool _takingDamage = false;
    private bool _largeLaserHit = false;
    private Boss _bossBody;
    private Player _player;


    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _bossBody = gameObject.GetComponentInParent<Boss>();

        NullCheck();

        _position1 = transform.localPosition;

    }
    private void NullCheck()
    {
        if (_player == null)
        {
            Debug.LogError("player is null");
        }

        if (_bossBody == null)
        {
            Debug.LogError("boss is null");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void ShotFX(Vector3 _pos)
    {
        _bossBody.ExplodeAudio();
        GameObject _exp = Instantiate(_explosion, _pos, Quaternion.identity, gameObject.transform);
        _exp.transform.localScale = Vector3.one * 0.1f;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (_takingDamage == true && !_exploding)
        {
            if (other.tag == "Laser")
            {
                ShotFX(other.transform.position + Vector3.up * 0.1f);
                Destroy(other.gameObject);
                TakeDamage(10);
                Destroyed();
            }
            if (other.tag == "Blast")
            {
                ShotFX(other.transform.position + Vector3.up * 0.1f);
                TakeDamage(20);
                Destroyed();
            }
            if (other.tag == "LargeLaser")
            {
                StartCoroutine(LargeLaserDamage());
            }
            if (other.tag == "Player")
            {
                Player _player = other.GetComponent<Player>();
                if (_player != null)
                {
                    TakeDamage(5);
                    _player.TakeDamage();
                }
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
    void TakeDamage(float _damage)
    {
        float damage = Mathf.Min(_damage, _health);
        _health -= damage;
        _bossBody.UpdateHealth(damage);
    }
    void Destroyed()
    {
        if (_health <= 0)
        {
            _exploding = true;
            Instantiate(_explosion, transform.position, Quaternion.identity, gameObject.transform.parent);
            _bossBody.ExplodeAudio();
            Instantiate(_drop, transform.position + Vector3.down, Quaternion.identity);

            Destroy(gameObject);
        }
    }


    IEnumerator FireLaser()
    {
        yield return new WaitForSeconds(1f);
        while (!_exploding)
        {
            _bossBody.LaserAudio();
            Vector3 _angle = _laser.transform.rotation.eulerAngles + transform.rotation.eulerAngles;
            Instantiate(_laser, transform.position, Quaternion.Euler(_angle));
            yield return new WaitForSeconds(2f);
        }
    }
    IEnumerator StageTwoMovement()
    {
        float _t = 0;
        yield return new WaitForSeconds(3f);
        while (!_exploding)
        {

            transform.localPosition = Vector3.Slerp(_position1, _position2, Mathf.Clamp(Mathf.Sin(_t / Mathf.PI * 5f) + 0.5f, 0f, 1f));
            if (_player != null)
            {
                transform.up = transform.position - _player.transform.position;
            }
            yield return new WaitForEndOfFrame();
            _t += Time.deltaTime;
        }
    }
    IEnumerator LargeLaserDamage()
    {
        while (!_exploding && _largeLaserHit)
        {
            TakeDamage(5);
            Destroyed();
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void StageOne()
    {
        StartCoroutine(FireLaser());
        _takingDamage = true;
    }

    public void StageTwo()
    {
        StartCoroutine(StageTwoMovement());
    }
}
