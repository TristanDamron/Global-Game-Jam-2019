using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField]
    private float spinSpeed = 4.0f;
    private void Update()
    {
        transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime);
    }
    void OnTriggerEnter2D(Collider2D c) {
        if (c.tag == "Player") {
            Destroy(gameObject);
            DynamicAudioController.Play("sfx_memoryCollect");
        }
    }
}
