using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceyCushions : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D c) {
        if (c.gameObject.tag == "Player") {
            AudioController.PlayBounce();
        }
    }
}
