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
    */
        [SerializeField]
    private GameObject _laserRounds;
    [SerializeField]
    private GameObject _blast;

    private int _wave = 0;
    int _enemCount = 0;


    private UIManager _UIManager;

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

    // Update is called once per frame
    void Update()
    {
        
    }

    ///// coroutine /////

    IEnumerator EnemySpawnRoutine(){
        yield return new WaitForSeconds(4.0f);
        Vector3 _spawnPosition = new Vector3(0, 6, 0);

        bool _stopSpawning = _wave%10 == 0;
        while(!_stopSpawning){
            _spawnPosition.x = Random.Range(-10f,10f);
            GameObject newEnemy = Instantiate(_enemyPrefab[0],_spawnPosition,Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
 
            yield return new WaitForSeconds(4.0f);
            _enemCount ++;
            if (_enemCount >10)
            {
                _enemCount = 0 ;
                _wave++;
            yield return new WaitForSeconds(4.0f);
            } //nextwave

            if(_wave == 5){
            StartCoroutine(Enemy2SpawnRoutine());

            }
        }
    }

        IEnumerator Enemy2SpawnRoutine(){
            yield return new WaitForSeconds(4.0f);
            Vector3 _spawnPosition = new Vector3(-10, 0, 0);
            _spawnPosition.y = Random.Range(-0f,4f);
            int _count = 0;
            while(_count < 10){

                GameObject newEnemy = Instantiate(_enemyPrefab[1],_spawnPosition,Quaternion.identity);
                newEnemy.transform.parent = _enemyContainer.transform;
                _count ++; 
                yield return new WaitForSeconds(0.05f);
            }
        }
    IEnumerator PowerupSpawnRoutine()
    {
        yield return new WaitForSeconds(6.0f);

        Vector3 _spawnPosition = new Vector3(0, 6, 0);
        while (_wave > 0)
        {
            _spawnPosition.x = Random.Range(-10f, 10f);
            int _randomizedID = Random.Range(0,4);
            GameObject newToken = Instantiate(_powerup[_randomizedID], _spawnPosition, Quaternion.identity);
            newToken.transform.parent = gameObject.transform;
            yield return new WaitForSeconds(Random.Range(3f, 7f));
        }
    }

    IEnumerator LaserRoundsRoutine() 
    {
        yield return new WaitForSeconds(2.0f);

        Vector3 _spawnPosition = new Vector3(0, 6, 0);
        while (_wave > 0)
        {
            _spawnPosition.x = Random.Range(-10f, 10f);
            GameObject newToken = Instantiate(_powerup[3], _spawnPosition, Quaternion.identity);
            newToken.transform.parent = gameObject.transform;
            yield return new WaitForSeconds(Random.Range(4f, 6f));
        }


    }

    IEnumerator SecondarySpawnRoutine()
    {
        yield return new WaitForSeconds(10.0f);
        int _randomizedID = Random.Range(4,6);
        Vector3 _spawnPosition = new Vector3(0, 6, 0);
        while (_wave > 0)
        {
            _spawnPosition.x = Random.Range(-10f, 10f);
            GameObject newToken = Instantiate(_powerup[_randomizedID], _spawnPosition, Quaternion.identity);
            newToken.transform.parent = gameObject.transform;
            yield return new WaitForSeconds(Random.Range(20f, 40f));
        }


    }

    ///// public methods /////

    public void OnPlayerDeath(){
        _wave = 0;
    }

    public void StartSpawning() {
        _wave = 1;
        _UIManager.WaveText(_wave);
        StartCoroutine(EnemySpawnRoutine());
        StartCoroutine(PowerupSpawnRoutine());
        StartCoroutine(LaserRoundsRoutine());
        StartCoroutine(SecondarySpawnRoutine());
    }

public void Test(){

    StartCoroutine(Enemy2SpawnRoutine());
}


}

