using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChargeBar : MonoBehaviour
{



    private Image _barFill;
   private Image _barFillThreshold;

    private float _threshold = 0.3f;

    [SerializeField]
    private GameObject _barTick;

    private float _barTickMax = 85f;
    private float _barTickMin = - 85f;


    // Start is called before the first frame update
    void Start()
    {
        _barFill = GameObject.Find("Bar").GetComponent<Image>();
        _barFillThreshold = GameObject.Find("BarThreshold").GetComponent<Image>();

        if (_barFill == null)
        {
            Debug.LogError("barfill is null");
        }

        if (_barFillThreshold == null)
        {
            Debug.LogError("barfill threshold is null");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    void ResizeChargeBarFill(float _charge)
    {


        float _fill = _charge * 0.95f + (1f - _charge) * 0.05f;

        _barFill.fillAmount = _fill;

        if (_charge <_threshold)
        {
            _barFillThreshold.fillAmount = _fill;
        }
        
    }

    void MoveChareBarTick(float _charge)
    {

        float _pos = _charge * _barTickMax + (1 - _charge) * _barTickMin;

        _barTick.transform.localPosition = Vector3.right * _pos;

    }

    public void UpdateChargeBar(float _charge)
    {
        ResizeChargeBarFill(_charge);
        MoveChareBarTick(_charge);
    }
}
