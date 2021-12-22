using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 7;
    [SerializeField]
    private bool _enemyLaser = false;
    [SerializeField]
    private bool _largeLaser = false;

    // Update is called once per frame
    void Update()
    {

        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        if (Mathf.Abs(transform.position.y) > 8f && !_largeLaser)
        {
            DestroyLaser();
        }

    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (gameObject.tag == "Boss")
        {
            BossDamage(other);
        }
        else if (_enemyLaser)
        {
            EnemyDamage(other);
        }
    }

    void EnemyDamage(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player _player = other.GetComponent<Player>();
            if (_player != null)
            {
                _player.TakeDamage();
                DestroyLaser();
            }
        }
        else if (other.tag == "Token")
        {
            Destroy(other.gameObject);
            DestroyLaser();
        }
        else if (other.tag == "Blast")
        {
            DestroyLaser();
        }
    }
    void BossDamage(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player _player = other.GetComponent<Player>();
            if (_player != null)
            {
                _player.TakeDamage();
                if (!_largeLaser)
                {
                    DestroyLaser();
                }
            }
        }
        if (other.tag == "Blast")
        {

            if (_largeLaser)
            {
                Debug.Log("_largeLaser");
                transform.parent.gameObject.SetActive(false);
            }
            else
            {
                DestroyLaser();
            }
        }
    }
    void DestroyLaser()
    {
        if (transform.parent)
        {
            Destroy(transform.parent.gameObject);
        }
        Destroy(gameObject);
    }

    public void AssignEnemyLaser()
    {
        _enemyLaser = true;
    }


}
