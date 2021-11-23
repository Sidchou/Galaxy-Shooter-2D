using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [SerializeField]
    GameObject[] _menus;
    /*
    0 - main
    1 - audio
    2 - video
    3 - control
    4 - credit
 */

    // Start is called before the first frame update
    void OnEnable()
    {
      
    SwitchMenu(0);
    }

    public void SwitchMenu(int n) {
        for (int i = 0; i < _menus.Length; i++)
        {
            if (i == n)
            {
                _menus[i].SetActive(true);
            }
            else { 
                _menus[i].SetActive(false); 
            }
        }
    }
}
