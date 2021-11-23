using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
  
    [SerializeField]
    private GameObject _getSounds;
    private AudioSource _sounds;

    

    private float _musicVolume;
    private float _soundVolume;
    private bool _mute = false;


    // Start is called before the first frame update
    void Start()
    {

        _sounds = _getSounds.GetComponent<AudioSource>();
        if (_sounds == null)
        {
            Debug.LogError("sound list is null");
        }


    }

    // Update is called once per frame
    void Update()
    {

    }
    public void AdjustVolume(float val)
    {
        _sounds.volume = val;
    }
 
    public void Mute()
    {

    }
}
