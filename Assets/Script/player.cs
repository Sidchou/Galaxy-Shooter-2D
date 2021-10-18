using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // speed veriable
    [SerializeField]
    private float _speed = 5.0f;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private float _fireRate = 0.5f; 
    private float _nextFire = 0.0f;
    [SerializeField]
    private int _lives = 3 ;

    private SpawnManager _spawnManager;


    // Start is called before the first frame update
    void Start()
    {
        //initiallize position 
        transform.position = Vector3.zero;
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        if(_spawnManager == null){
            Debug.LogError("Spawn Manager is NULL");
        }
    }

    // Update is called once per frame
    void Update()
    {
       playerMovement();

       
          if (Input.GetKeyDown(KeyCode.Space)){
        fireLaser();
       }
 
    }

    void playerMovement(){
        characterKeyBinding();
        characterLimit();
    }
     void characterKeyBinding(){

        //user input
        float _horizontalInput = Input.GetAxis("Horizontal");
        transform.Translate(Vector3.right*_horizontalInput*_speed*Time.deltaTime);
          
        float _verticalInput = Input.GetAxis("Vertical");
        transform.Translate(Vector3.up*_verticalInput*_speed*Time.deltaTime);

     }
    void characterLimit(){
        
        //player xy limit
        Vector3 _setPLayerPosition = transform.position;
        //vertical limit
        float _yClamp = Mathf.Clamp(transform.position.y,-4f,4f);
        _setPLayerPosition.y = _yClamp;
        //horizontal wrap
        if (Mathf.Abs(transform.position.x)>10){
            _setPLayerPosition.x = -9.5f*Mathf.Sign(_setPLayerPosition.x);
            }
        //set position
        transform.position = _setPLayerPosition; 

    }
    
    void fireLaser(){
        if(Time.time > _nextFire){
            _nextFire = Time.time + _fireRate;
            Instantiate(_laserPrefab, transform.position + new Vector3(0,1.0f,0), Quaternion.identity);
        }
    }

    public void takeDamage(){
        _lives --;
        if (_lives<=0)
        { 
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }
    }

}