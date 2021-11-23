using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class AudioMenu : MonoBehaviour
{
    [SerializeField]
    private Slider _bgSlider;
    [SerializeField]
    private Text _bgTitle;
    private string _bgTexts;

    [SerializeField]
    private Slider _fxSlider;
    [SerializeField]
    private Text _fxTitle;
    private string _fxTexts;

    

 

    // Start is called before the first frame update
    void Start()
    {

        _bgTexts = _bgTitle.text;
        _fxTexts = _fxTitle.text;
        UpdateBGText(_bgSlider.value);
        UpdateFXText(_fxSlider.value);

    }


    // Update is called once per frame
    void Update()
    {


   }

    public void UpdateBGText(float val) {

        string display = _bgTexts + Mathf.Floor(val * 100).ToString();
        _bgTitle.text = display;

    }

    public void UpdateFXText(float val)
    {

        string display = _fxTexts + Mathf.Floor(val * 100).ToString();
        _fxTitle.text = display;

    }

    public void Mute(bool muted) {
        EventSystem.current.SetSelectedGameObject(null);
        _bgSlider.interactable = !muted;
        _fxSlider.interactable = !muted;

    }
}
    




