using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Overlay : MonoBehaviour
{

    // public string overlayText;

    static public bool isActive;

    private Text text;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        if (isActive)
        {
            Debug.LogError("TWO INSTANCES OF OVERLAY CONTROLLER ACTIVE!");
            gameObject.SetActive(false);
        }
        isActive = true;
        text = GetComponentInChildren<Text>();
        /*
        if (text)
            text.text = overlayText;
            */
    }
    private void OnDisable()
    {
        isActive = false;
    }
}
