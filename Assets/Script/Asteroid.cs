using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    private Vector3 _dir;
    [SerializeField]
    private float _rotateSpeed = 3.0f;
    [SerializeField]
    private GameObject _explosion;
    private SpawnManager _spawnManager;

    // Start is called before the first frame update
    void Start()
    {
        _dir = new Vector3(4f, Random.Range(-3f, 3f), 0);
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("spawn manager is null");
        }
    }

    // Update is called once per frame
    void Update()
    {
        //transform.Translate(_dir);

        transform.Rotate(Vector3.forward * _rotateSpeed * Time.deltaTime, Space.Self);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if ( other.tag == "Laser") {
            Instantiate(_explosion, transform.position, Quaternion.identity);
            _spawnManager.StartSpawning(0);
            Destroy(other.gameObject);
            Destroy(gameObject,0.2f);
        }
        
    }
}
