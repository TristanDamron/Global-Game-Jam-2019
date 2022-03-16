using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YoyoReset : MonoBehaviour
{
    [SerializeField] private float spinSpeed = 4.0f;
    public bool isActive = true;

    private void Update()
    {
        transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime);
            }

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
