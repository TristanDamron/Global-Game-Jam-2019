﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public MO_Yoyo yoyo;
    public Rigidbody2D rb_;        
    [SerializeField]
    private float speed_;
    [SerializeField]
    private float jumpTimer_;
    [SerializeField]
    private float jumpDuration_;
    [SerializeField]
    private bool jumping_;
    [SerializeField]
    private float yVelocityUpperLimit_;
    [SerializeField]
    private bool yoyoing_;
    

    void Start() {
        rb_ = GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {      
        if (Input.GetAxis("Horizontal") != 0f) {
            Vector3 pos = transform.position;
            pos.x = pos.x + Input.GetAxis("Horizontal") * speed_ * Time.deltaTime;
            transform.position = pos;
        }

        if (Input.GetAxis("Jump") != 0) {
            Jump(10);
        }

        // Restrict the upward velocity of the player
        if (rb_.velocity.y >= yVelocityUpperLimit_) {
            rb_.velocity = new Vector2(rb_.velocity.x, yVelocityUpperLimit_);       
            Debug.Log("Reached maximum jump height"); 
        }

        if (YoyoInput() && !yoyoing_) {
            LaunchYoyo(); 
        } else if (YoyoInput() && yoyoing_) {
            QuitYoyo();
        }
    }

    void Jump(float thrust_) {
        jumpTimer_++;

        if (jumpTimer_ < jumpDuration_) {
            Vector3 pos = transform.position;
            pos.y += thrust_ * Time.deltaTime;
            transform.position = pos;
        }
    }

    void OnCollisionEnter2D(Collision2D c) {
        if (c.gameObject.layer == LayerMask.NameToLayer("World")) {
            jumping_ = false;
            jumpTimer_ = 0f;
        }
    }

    bool YoyoInput()
    {
        if (!yoyo) return false;
        return (Input.GetMouseButtonDown(0) || Input.GetAxisRaw("Yoyo") != 0f);
    }

    public void LaunchYoyo()
    {
        yoyo.Launch();
        yoyoing_ = true;
    }    

    public void QuitYoyo()
    {
        yoyo.Release();
        yoyoing_ = false;
    }
}
