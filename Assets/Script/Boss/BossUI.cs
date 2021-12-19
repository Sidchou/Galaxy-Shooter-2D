using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossUI : MonoBehaviour
{
    [SerializeField]
    private Image _healthBar1;
    [SerializeField]
    private Image _healthBar2;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(BossHealthBarInit());
    }
    IEnumerator BossHealthBarInit()
    {
        float _fillAnim = 0;
        while (_fillAnim < 2f)
        {
            _healthBar1.fillAmount = Mathf.Clamp(_fillAnim, 0f, 1f);
            _healthBar2.fillAmount = Mathf.Clamp(_fillAnim - 0.4f, 0f, 1f);
            yield return new WaitForEndOfFrame();
            _fillAnim += Time.deltaTime * 0.75f;
        }
    }
    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateBossHealth(float _bossHealth)
    {
        _healthBar1.fillAmount = Mathf.Clamp(_bossHealth, 0f, 1f);
        _healthBar2.fillAmount = Mathf.Clamp(_bossHealth - 1f, 0f, 1f);
    }
}
