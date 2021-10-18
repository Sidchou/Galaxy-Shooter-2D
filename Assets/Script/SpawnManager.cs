using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;

    private bool _stopSpawning = false;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnRoutine(){
        Vector3 _spawnPosition = new Vector3(0, 6, 0);
        while(!_stopSpawning){
            _spawnPosition.x = Random.Range(-10f,10f);
            GameObject newEnemy = Instantiate(_enemyPrefab,_spawnPosition,Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(3.0f);
        }
    }

    public void OnPlayerDeath(){
        _stopSpawning = true;
    }
}
