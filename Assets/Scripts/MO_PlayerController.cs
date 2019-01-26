using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MO_PlayerController : MonoBehaviour
{
    public MO_Yoyo yoyo;
    public float yVelocityUpperLimit_ = 20.0f;
    public float extraJumpGravity_ = 12.0f;
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
    private Animator animator_;
    private PlayerState playerState = PlayerState.AIRBORNE;

    public enum PlayerState
    {
        AIRBORNE,
        GROUNDED,
        YOYO,
        IDLE
    }

    void Start()
    {
        rb_ = GetComponent<Rigidbody2D>();
        particles_ = GetComponent<ParticleSystem>();
        animator_ = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (playerState == PlayerState.AIRBORNE)
        {
            if (rb_.velocity.y == 0)
                playerState = PlayerState.GROUNDED;
            else
            {
                Vector3 velocity = rb_.velocity;
                velocity.y -= extraJumpGravity_ * Time.deltaTime;
                rb_.velocity = velocity;
            }
        }
        if (playerState != PlayerState.YOYO)
        {
            if (Input.GetAxis("Horizontal") != 0f && playerState != PlayerState.AIRBORNE) {
                animator_.Play("Walk");
            } else if (playerState != PlayerState.AIRBORNE) {
                animator_.Play("Idle");
            }

            var pos = new Vector3();
            pos.x = transform.position.x + Input.GetAxis("Horizontal") * Time.deltaTime * speed_;
            pos.y = transform.position.y;

            if (playerState != PlayerState.AIRBORNE &&
                Input.GetAxis("Jump") != 0f &&
                rb_.velocity.y == 0)
                Jump(jumpForce_);

            // Restrict the upward velocity of the player
            if (rb_.velocity.y >= yVelocityUpperLimit_)
                rb_.velocity = new Vector2(rb_.velocity.x, yVelocityUpperLimit_);

            pos.z = transform.position.z;
            transform.position = pos;

            if (holding_ != null)
                holding_.transform.position = transform.position;

            if (Input.GetJoystickNames().Length == 0) {
                if (Input.GetMouseButtonDown(0))
                {
                    yoyo.Launch();
                    playerState = PlayerState.YOYO;
                }
            } else if (Input.GetAxis("Yoyo") != 0f) {
                yoyo.Launch();
                playerState = PlayerState.YOYO;
            }
        }
        else if (playerState == PlayerState.YOYO)
        {
            if (Input.GetMouseButtonDown(0))
            {
                yoyo.Release();
                playerState = PlayerState.IDLE;
            } else if (Input.GetAxis("Yoyo") != 0f) {
                yoyo.Release();
                playerState = PlayerState.IDLE;
            }            
        }
        // DebugUI.instance.playerVelocity = rb_.velocity;
    }

    void OnCollisionEnter2D(Collision2D c) {
        if (c.gameObject.layer == LayerMask.NameToLayer("World")) {
            playerState = PlayerState.GROUNDED;
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
        animator_.Play("Jump");
        Vector3 velocity = rb_.velocity;
        velocity.y += thrust_;
        rb_.velocity = velocity;

        playerState = PlayerState.AIRBORNE;
    }

    public void QuitYoyo()
    {
        playerState = PlayerState.AIRBORNE;
    }
}
