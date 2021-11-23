using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject[] _powerup;
    [SerializeField]
    private GameObject _laserRounds;
    [SerializeField]
    private GameObject _blast;

    private bool _stopSpawning = false;
    // Start is called before the first frame update
    void Start()
    {
      

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    ///// coroutine /////

    IEnumerator EnemySpawnRoutine(){
        yield return new WaitForSeconds(4.0f);
        Vector3 _spawnPosition = new Vector3(0, 6, 0);
        while(!_stopSpawning){
            _spawnPosition.x = Random.Range(-10f,10f);
            GameObject newEnemy = Instantiate(_enemyPrefab,_spawnPosition,Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(3.0f);
        }
    }

    IEnumerator PowerupSpawnRoutine()
    {
        yield return new WaitForSeconds(6.0f);

        Vector3 _spawnPosition = new Vector3(0, 6, 0);
        while (!_stopSpawning)
        {
            _spawnPosition.x = Random.Range(-10f, 10f);
            int _randomizedID = Random.Range(0,_powerup.Length);
            GameObject newToken = Instantiate(_powerup[_randomizedID], _spawnPosition, Quaternion.identity);
            newToken.transform.parent = gameObject.transform;
            yield return new WaitForSeconds(Random.Range(3f, 7f));
        }
    }

    IEnumerator LaserRoundsRoutine() 
    {
        yield return new WaitForSeconds(2.0f);

        Vector3 _spawnPosition = new Vector3(0, 6, 0);
        while (!_stopSpawning)
        {
            _spawnPosition.x = Random.Range(-10f, 10f);
            GameObject newToken = Instantiate(_laserRounds, _spawnPosition, Quaternion.identity);
            newToken.transform.parent = gameObject.transform;
            yield return new WaitForSeconds(Random.Range(2f, 4f));
        }


    }

    IEnumerator SecondarySpawnRoutine()
    {
        yield return new WaitForSeconds(10.0f);

        Vector3 _spawnPosition = new Vector3(0, 6, 0);
        while (!_stopSpawning)
        {
            _spawnPosition.x = Random.Range(-10f, 10f);
            GameObject newToken = Instantiate(_blast, _spawnPosition, Quaternion.identity);
            newToken.transform.parent = gameObject.transform;
            yield return new WaitForSeconds(Random.Range(20f, 40f));
        }


    }

    ///// public methods /////

    public void OnPlayerDeath(){
        _stopSpawning = true;
    }

    public void StartSpawning() {
        StartCoroutine(EnemySpawnRoutine());
        StartCoroutine(PowerupSpawnRoutine());
        StartCoroutine(LaserRoundsRoutine());
        StartCoroutine(SecondarySpawnRoutine());
    }
}
