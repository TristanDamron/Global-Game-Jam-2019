using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YoyoReset : MonoBehaviour
{
    public bool isActive = true;
    private void OnTriggerEnter2D(Collider2D c)
    {
        if (!isActive) return;
        PlayerController pc = c.gameObject.GetComponent<PlayerController>();
        if (pc) pc.ResetYoyo(this);
        DynamicAudioController.Play("yoyo_reset");
        isActive = false;
        gameObject.SetActive(false);
    }
}
