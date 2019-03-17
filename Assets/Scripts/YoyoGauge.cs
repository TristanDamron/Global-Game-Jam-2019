using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YoyoGauge : MonoBehaviour
{
    public bool yoyoOn = true;

    private void Update()
    {
        if (!yoyoOn)
        {
            GetComponent<Image>().color = Color.grey;
        }
        else { GetComponent<Image>().color = Color.white; }
    }

}
