using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MO_PlayerController : MonoBehaviour
{

    public MO_Yoyo yoyo;
    [SerializeField]
    private float speed_;
    [SerializeField]
    private float jumpForce_ = 600.0f;
    [SerializeField]
    private bool jumping_;
    [SerializeField]
    private Transform respawnLocation_;
    [SerializeField]
    private GameObject holding_;
    [System.NonSerialized]
    public Rigidbody2D rb_;
    private ParticleSystem particles_;

    private PlayerState playerState = PlayerState.isIdle;

    public enum PlayerState
    {
        isJumping,
        isIdle,
        isWalking,
        isYoyoing
    }

    void Start()
    {
        rb_ = GetComponent<Rigidbody2D>();
        particles_ = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if (playerState != PlayerState.isYoyoing)
        {
            var pos = new Vector3();
            pos.x = transform.position.x + Input.GetAxis("Horizontal") * Time.deltaTime * speed_;
            pos.y = transform.position.y;

            if (!jumping_ && Input.GetAxis("Jump") != 0f && rb_.velocity.y == 0)
                Jump(jumpForce_);

            // Restrict the upward velocity of the player
            if (rb_.velocity.y >= 7f)
                rb_.velocity = new Vector2(rb_.velocity.x, 7f);

            pos.z = transform.position.z;
            transform.position = pos;

            if (holding_ != null)
                holding_.transform.position = transform.position;

            if (Input.GetJoystickNames().Length == 0) {
                if (Input.GetMouseButtonDown(0))
                {
                    yoyo.Launch();
                    playerState = PlayerState.isYoyoing;
                }
            } else if (Input.GetAxis("Yoyo") != 0f) {
                yoyo.Launch();
                playerState = PlayerState.isYoyoing;
            }
        }
        else if (playerState == PlayerState.isYoyoing)
        {
            if (Input.GetMouseButtonDown(0))
            {
                yoyo.Release();
                playerState = PlayerState.isIdle;
            } else if (Input.GetAxis("Yoyo") != 0f) {
                yoyo.Release();
                playerState = PlayerState.isIdle;
            }            
        }
    }

    void OnCollisionEnter2D(Collision2D c) {
        if (c.gameObject.layer == LayerMask.NameToLayer("World")) {
            jumping_ = false;
        } else if (c.gameObject.layer == LayerMask.NameToLayer("Deadzone")) {
            transform.position = respawnLocation_.position;
            particles_.Play();
        }
    }

    void OnTriggerEnter2D(Collider2D c) {
        if (c.gameObject.name == "Collectable") {
            holding_ = c.gameObject;
        }
    }

    public void Jump(float thrust_) {
        rb_.AddForce(transform.up * thrust_);

        jumping_ = true;
    }

    public void QuitYoyo()
    {
        playerState = PlayerState.isIdle;
    }
}
