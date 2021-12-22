using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    [SerializeField]
    private GameObject[] _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject[] _powerup;
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
    private GameObject[] _negPowerup;
    bool _stopSpawning = true;

    private int _wave = 8;

    private float _enemySpawnTime = 3f;
    private UIManager _UIManager;


    private int[] waves;
    /*
    0 = none
    1 = fire
    2 = sheild
    3 = follow
    4 = zigzag
    5 = turn around shoot
    6 = dodge

    0,0,0,0,0
    0,1,0,1,0,1,0   
    1,0,1,2,1,2,1,0,1   #
    1,2,3,2,1,2,1,2,3,2,1    *
    2,1,3,2,1,4,4,1,2,3,1,2         
    1,2,5,2,1,5,4,5,1,2,5,2,1  #
    1,6,5,2,6,5,4,4,5,6,2,5,6,1   
    2,1,5,6,4,6,1,5,1,6,4,6,5,1,2  
    4,2,5,6,5,6,2,5,2,6,5,6,5,2,4   #  *
    boss
    */
    [SerializeField]
    private GameObject _boss;



    // private bool _stopSpawning = false;
    // Start is called before the first frame update
    void Start()
    {

        _UIManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_UIManager == null)
        {
            Debug.LogError("UI Manager is Null");
        }
    }

    void SetWaveEnemies(int _waveNum)
    {
        switch (_waveNum)
        {
            case 1:
                waves = new int[] { 0, 0, 0, 0, 0 };
                break;
            case 2:
                waves = new int[] { 0, 1, 0, 1, 0, 1, 0 };
                break;
            case 3:
                waves = new int[] { 1, 0, 1, 2, 1, 2, 1, 0, 1 };
                break;
            case 4:
                waves = new int[] { 1, 2, 3, 2, 1, 2, 1, 2, 3, 2, 1 };
                break;
            case 5:
                waves = new int[] { 2, 1, 3, 2, 1, 4, 4, 1, 2, 3, 1, 2 };
                break;
            case 6:
                waves = new int[] { 1, 2, 5, 2, 1, 5, 4, 5, 1, 2, 5, 2, 1 };
                break;
            case 7:
                waves = new int[] { 1, 6, 5, 2, 6, 5, 4, 4, 5, 6, 2, 5, 6, 1 };
                break;
            case 8:
                waves = new int[] { 2, 1, 5, 6, 4, 6, 1, 5, 1, 6, 4, 6, 5, 1, 2 };
                break;
            case 9:
                waves = new int[] { 4, 2, 5, 6, 5, 6, 2, 5, 2, 6, 5, 6, 5, 2, 4 };
                break;
            case 10:
                waves = new int[] { };
                break;
            default:
                break;
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void SpecialSpawn(int n)
    {
        Vector3 _spawnPosition = new Vector3(-10, 0, 0);
        _spawnPosition.y = Random.Range(2f, 6f);
        if (_wave % 3 == 0 && Mathf.Floor(waves.Length / 3) == n)
        {
            Instantiate(_enemyPrefab[1], _spawnPosition, Quaternion.identity, _enemyContainer.transform);
        }
        if (_wave % 5 == 4 && Mathf.Floor(waves.Length / 2) == n)
        {
            StartCoroutine(Enemy3SpawnRoutine());
        }
    }


    ///// coroutine /////

    IEnumerator EnemySpawnRoutine()
    {
        yield return new WaitForSeconds(4.0f);
        Vector3 _spawnPosition = new Vector3(0, 6, 0);
        int _enemCount = 0;

        // Debug.Log("wave " + _wave + " | (" + waves.Length + ")");
        while (!_stopSpawning && _enemCount < waves.Length)
        {
            _spawnPosition.x = Random.Range(-10f, 10f);

            GameObject _newEnemy = Instantiate(_enemyPrefab[0], _spawnPosition, Quaternion.identity, _enemyContainer.transform);
            Enemy _EnemyType = _newEnemy.GetComponent<Enemy>();
            _EnemyType.SetType(waves[_enemCount]);

            SpecialSpawn(_enemCount);

            yield return new WaitForSeconds(_enemySpawnTime);
            _enemCount++;

        }
        //next wave
        yield return new WaitForSeconds(3.0f);
        StartSpawning(_wave);
        _enemySpawnTime = Mathf.Max(3f - _wave * 0.2f, 1f);
    }

    IEnumerator Enemy3SpawnRoutine()
    {
        Vector3 _spawnPosition = new Vector3(-10, 0, 0);
        _spawnPosition.y = Random.Range(2f, 6f);
        int _count = 0;
        while (_count < 10)
        {
            GameObject newEnemy = Instantiate(_enemyPrefab[2], _spawnPosition, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            _count++;
            yield return new WaitForSeconds(0.05f);
        }
    }


    IEnumerator PowerupSpawnRoutine()
    {
        yield return new WaitForSeconds(8.0f);

        Vector3 _spawnPosition = new Vector3(0, 6, 0);
        while (!_stopSpawning)
        {
            _spawnPosition.x = Random.Range(-10f, 10f);
            bool negativeSpawn = false;
            int _randomizedID;
            float _rng = Random.Range(0f, 1f);
            if (_rng < 0.1)
            {
                _randomizedID = Random.Range(0, _negPowerup.Length);
                negativeSpawn = true;
            }
            else if (_rng < 0.2)
            {
                _randomizedID = 7; // 1up
            }
            else if (_rng < 0.3)
            {
                _randomizedID = 5; //large laser
            }
            else if (_rng < 0.5)
            {
                _randomizedID = 3; //ammo
            }
            else
            {
                _randomizedID = Random.Range(0, 3); //powerup
            }
            if (negativeSpawn)
            {
                Instantiate(_negPowerup[_randomizedID], _spawnPosition, Quaternion.identity, gameObject.transform);
            }
            else
            {
                Instantiate(_powerup[_randomizedID], _spawnPosition, Quaternion.identity, gameObject.transform);
            }
            yield return new WaitForSeconds(Random.Range(6f, 10f));
        }
    }


    ///// public methods /////

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }

    public void StartSpawning(int _waveNum)
    {
        _wave = _waveNum + 1;
        // _wave = 10; // boss test
        SetWaveEnemies(_wave);
        _UIManager.WaveText(_wave);

        if (_stopSpawning == true)
        {
            _stopSpawning = false;
            StartCoroutine(PowerupSpawnRoutine());
        }
        if (_wave != 10)
        {
            StartCoroutine(EnemySpawnRoutine());
        }
        else
        {
            _boss.SetActive(true);
        }
    }

    public void Test()
    {
        StartCoroutine(Enemy3SpawnRoutine());
    }


}

