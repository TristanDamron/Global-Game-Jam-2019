﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D c) {
        if (c.tag == "Player") {
            Destroy(gameObject);
        }
    }
}