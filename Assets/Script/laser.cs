using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 7;

    // Update is called once per frame
    void Update()
    {
        
        transform.Translate(Vector3.up*_speed*Time.deltaTime);

        if(transform.position.y>6){
            Destroy(gameObject);
        }

    }
}