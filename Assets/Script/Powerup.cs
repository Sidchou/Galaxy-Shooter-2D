using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;
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
    */
    [SerializeField]
    private AudioClip _pickupAudio;


    [SerializeField]
    private float _audioVolume = 0.75f;
    [SerializeField]
    private GameObject _explosionPrefab;

    private Player _player;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if(_player == null){
            Debug.LogError("player is null");
        }

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down*_speed*Time.deltaTime);
        
        Magnetize();

        if (transform.position.y < -5)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player") {
            Player player = other.transform.GetComponent<Player>();

            AudioSource.PlayClipAtPoint(_pickupAudio,transform.position,_audioVolume);
            if (_debuff)
            {
                Debuffs(player);
            }else {
                Buffs(player);
            }
            
 
            Destroy(gameObject);
        }
        if (other.tag == "Enemy")
        {
            GameObject _explosion = Instantiate(_explosionPrefab,transform.position, Quaternion.identity);
            _explosion.transform.localScale = new Vector3(0.2f,0.2f,0.2f);
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
                default:
                    break;
            }
    }

     void Magnetize(){

        Vector3 _diff =_player.transform.position - transform.position; 
        float _dist = Vector3.Magnitude(_diff);
        // Debug.Log(_dist);
        if (Input.GetKey(KeyCode.C)&&_dist<3f){
            transform.Translate(Vector3.Normalize(_diff)/Mathf.Pow(_dist,2f)*Time.deltaTime);
        }
    }

}
