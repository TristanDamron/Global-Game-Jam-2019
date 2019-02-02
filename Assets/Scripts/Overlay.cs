using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Overlay : MonoBehaviour
{
    static public Overlay activeOverlay;

    private bool isInitialized = false;

    private void Start()
    {
        gameObject.SetActive(false);
        isInitialized = true;
    }

    private void OnEnable()
    {
        if (!isInitialized) return;

        if (activeOverlay)
        {
            Debug.LogError("TWO INSTANCES OF OVERLAY CONTROLLER ACTIVE!\n" +
                           "Active Overlay: " + activeOverlay.name + "\n" +
                           "New Overlay: " + this.name);
            // gameObject.SetActive(false);
            // return;
            activeOverlay.gameObject.SetActive(false);
        }
        activeOverlay = this;
    }
    private void OnDisable()
    {
        activeOverlay = null;
    }
}
