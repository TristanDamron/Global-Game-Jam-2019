using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpThruPlatform : MonoBehaviour
{
    [SerializeField]
    private bool _top;

    [SerializeField]
    private bool _bottom;
    [SerializeField]
    private BoxCollider2D _collider;

    void OnTriggerEnter2D(Collider2D c) {
        if (c.tag == "Player" && _bottom) {
            _collider.isTrigger = true;
        }
    }

    void OnTriggerExit2D(Collider2D c) {
        if (c.tag == "Player" && _top) {
            _collider.isTrigger = false;
        }
    }
}
