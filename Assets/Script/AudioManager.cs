using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    private float _soundVolume;
    private bool _mute = false;


    // Start is called before the first frame update
    void Start()
    {

        AdjustVolume(0.75f);


    }

    // Update is called once per frame
    void Update()
    {

    }
    public void AdjustVolume(float val)
    {
        _soundVolume = val;
        AudioSource[] _audioSource = GameObject.FindObjectsOfType(typeof(AudioSource)) as AudioSource[];

        foreach (AudioSource item in _audioSource)
        {
            if (item.name != "Background Audio")
            {
                item.volume = val;
            }
        }

    }



    public void Mute(bool _muted)
    {
        _mute = _muted;
        AudioSource[] _audioSource = GameObject.FindObjectsOfType(typeof(AudioSource)) as AudioSource[];

        foreach (AudioSource item in _audioSource)
        {
            item.mute = _muted;
        }
    }

    public float GetVolume()
    {
        return _soundVolume;
    }
    public bool GetMuted()
    {
        return _mute;
    }
}
