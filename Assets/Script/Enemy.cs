using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4;
    [SerializeField]
    private float _rotate = 0;

    private Player _player;
 
    private Animator _animator;
    private bool _exploding = false;

    private AudioSource _exlopsionAudio;

    [SerializeField]
    private int _enemyID = 0;
    



    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _animator = GetComponent<Animator>();
        _exlopsionAudio = GetComponent<AudioSource>();

        NullCheck();
    }

    void NullCheck() {
        if (_player == null)
        {
            Debug.LogError("player is null");
        }
        if (_exlopsionAudio == null)
        {
            Debug.LogError("audio source is Null");
        }
        if (_animator == null)
        {
            Debug.LogError("animator is null");
        }

    }

    // Update is called once per frame
    void Update()
    {
     if (!_exploding)
     {
         
     EnemyMovement();
     }
             BottomEdge();
        SideEdge();
    }

    void EnemyMovement(){
        Vector3 _translation = Vector3.zero; 


        switch (_enemyID)
        {
            case 0:

            break;
            case 1:
            _translation =Vector3.down*_speed*Time.deltaTime;
            
            break;
            case 2:
            transform.Rotate(Vector3.forward*_rotate*Time.deltaTime,Space.Self);

            float _move = transform.localRotation.z+1f; 
            _move = (_move + 0.5f)%2f -1f;
            _move = Mathf.Pow(2*_move,2f);
            _translation = Vector3.down*_speed*Time.deltaTime;
            _translation.y -= _move*Time.deltaTime*2f;

            break;
            default:
            break;
        }

        transform.Translate(_translation);
        BottomEdge();
        SideEdge();
    }

    void BottomEdge() {
        if (transform.position.y < -5) {
            float _x = Random.Range(-8f, 8f);
            // transform.position = new Vector3(_x, 6, 0); //reset to top
            Destroy(gameObject);
        }

    }

       void SideEdge() {
        if (Mathf.Abs(transform.position.x) > 15) {
            Destroy(gameObject);
        }

    }
    private void OnTriggerEnter2D(Collider2D other){
        if (!_exploding)
        {
            if (other.tag == "Player")
            {
                PlayerCollision(other);
            }

            if (other.tag == "Laser")
            {
                LaserCollision(other);
            }

            if (other.tag == "Blast") {
                ExplosionAnimation();
            }
        }
    }
    private void PlayerCollision(Collider2D other){
        Player player = other.transform.GetComponent<Player>();
        if(player!= null){
            player.TakeDamage();
        }
        ExplosionAnimation();
    }
    private void LaserCollision(Collider2D other){
        Destroy(other.gameObject);

        if (_player)
        {
            _player.UpdateScore(10);
        }
        ExplosionAnimation();
    }

    private void ExplosionAnimation()
    {
        _exploding = true;
        _exlopsionAudio.Play();
        _animator.SetTrigger("EnemyDestroyed");
        _speed = 0; 

        Destroy(this.gameObject, 2.5f);
    }


}
