using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4;
    private Player _player;
 
    private Animator _animator;
    private bool _exploding = false;

    private AudioSource _exlopsionAudio;

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
        transform.Translate(Vector3.down*_speed*Time.deltaTime);
         
        if (transform.position.y < -4) {
            float _x = Random.Range(-8f, 8f);
            // transform.position = new Vector3(_x, 6, 0);
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
