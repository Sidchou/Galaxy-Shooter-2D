using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;


public class Missile : MonoBehaviour
{
    private GameObject[] _enemy;
    private GameObject _target;
    private float _speed = 3.5f;

    private float _spawnTime;
    private Quaternion _startDir;

    // Start is called before the first frame update
    void Start()
    {
        _speed -= 1f;
        GetTarget();

    }

    private void GetTarget()
    {
        string _pat = @".*Laser.*";
        Regex r = new Regex(_pat, RegexOptions.IgnoreCase);

        _enemy = GameObject.FindGameObjectsWithTag("Enemy");
        float _minDist = Mathf.Infinity;

        for (int j = 0; j < 2; j++)
        {
            for (int i = 0; i < _enemy.Length; i++)
            {
                float _dist = Vector3.Magnitude(_enemy[i].transform.position - transform.position);

                if (_minDist > _dist && !r.Match(_enemy[i].name).Success)
                {
                    if (_enemy[i].name == "Boss")
                    {
                        Boss _boss = _enemy[i].GetComponent<Boss>();
                        if (_boss != null)
                        {
                            if (_boss.Targetable())
                            {
                                _minDist = _dist;
                                _target = _enemy[i];
                            }
                        }
                    }
                    else
                    {
                        _minDist = _dist;
                        _target = _enemy[i];
                    }


                }
            }
            _enemy = GameObject.FindGameObjectsWithTag("Boss");
        }

        _spawnTime = Time.time;
        _startDir = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        float _deltaTime = Time.time - _spawnTime;
        if (_target == null)
        {
            GetTarget();
        }
        else
        {
            transform.LookAt(_target.transform, Vector3.back);
            transform.Rotate(Vector3.right * 90f);
            if (_deltaTime < 1f)
            {
                Quaternion _angle = Quaternion.Slerp(_startDir, transform.rotation, _deltaTime);
                transform.rotation = _angle;
                _speed += Time.deltaTime;
            }
        }
        transform.Translate(Vector3.up * _speed * Time.deltaTime);
        BottomEdge();
        SideEdge();
    }

    void BottomEdge()
    {
        if (transform.position.y < -5)
        {
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
}
