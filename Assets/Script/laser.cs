using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 7;
    [SerializeField]
    private bool _enemyLaser = false;

    // Update is called once per frame
    void Update()
    {
        
        transform.Translate(Vector3.up*_speed*Time.deltaTime);
        
        if(Mathf.Abs(transform.position.y)>8f){
        DestroyLaser();
        }

    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (_enemyLaser) {
            if (other.tag == "Player") {
                Player _player = other.GetComponent<Player>();
                if (_player != null)
                {
                    _player.TakeDamage();
                    DestroyLaser();
                }
            }
            if (other.tag == "Token") {
                Destroy(other.gameObject);
                DestroyLaser();
            }
        }
        if (other.tag == "Blast")
        {
            DestroyLaser();
        }
    }
    void DestroyLaser() {
          if (  transform.parent) {
                Destroy(transform.parent.gameObject);
            }
                Destroy(gameObject);
    }

    public void AssignEnemyLaser(){
        _enemyLaser = true;
    }


}
