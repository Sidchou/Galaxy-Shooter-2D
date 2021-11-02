using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;
    [SerializeField]
    private int powerupID = 0;
    /*
    0 = triple shot 
    1 = speed
    2 = sheild 
    */
    [SerializeField]
    private AudioClip _powerupAudio;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down*_speed*Time.deltaTime);
        if (transform.position.y < -5)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player") {
            Player player = other.transform.GetComponent<Player>();
            AudioSource.PlayClipAtPoint(_powerupAudio,transform.position);

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
                default:
                    break;
            }
 
            Destroy(gameObject);
        }
    }
}
