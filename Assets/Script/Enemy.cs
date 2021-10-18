using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4;
    // Start is called before the first frame update
    void Start()
    {
      
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
        Debug.Log(other);
        if (other.tag == "Player"){
            playerCollision(other);
        }

        if (other.tag == "Laser"){
            laserCollision(other);
        }
    }
    void playerCollision(Collider2D other){
        Player player = other.transform.GetComponent<Player>();
        if(player!= null){
            player.takeDamage();
        }
        Destroy(this.gameObject);
    }
    void laserCollision(Collider2D other){
        Destroy(other.gameObject);
        Destroy(this.gameObject);
    } 
}
