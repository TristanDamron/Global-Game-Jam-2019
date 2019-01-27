using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MO_PlayerController : MonoBehaviour
{
    public MO_Yoyo yoyo;
    public float yVelocityUpperLimit_ = 20.0f;
    public float extraJumpGravity_ = 12.0f;
    [SerializeField]
    private float walkSpeed_;
    [SerializeField]
    private float airSpeed_;
    [SerializeField]
    private float jumpForce_ = 600.0f;
    [SerializeField]
    private Transform respawnLocation_;
    [SerializeField]
    private GameObject holding_;
    [System.NonSerialized]
    public Rigidbody2D rb_;
    private ParticleSystem particles_;
    private Animator animator_;
    [SerializeField]
    private PlayerState playerState = PlayerState.AIRBORNE;

    private Vector3 top_;
    private Vector3 right_;
    private Vector3 bottom_;
    private Vector3 left_;

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
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        Vector3 colliderCenter = collider.bounds.center;
        Vector3 colliderMin = collider.bounds.min;
        Vector3 colliderMax = collider.bounds.min;
        bottom_ = new Vector3(colliderCenter.x, colliderMin.y, colliderCenter.z);
        top_ = new Vector3(colliderCenter.x, colliderMax.y, colliderCenter.z);
        right_ = new Vector3(colliderMax.x, colliderCenter.y, colliderCenter.z);
        left_ = new Vector3(colliderMin.x, colliderCenter.y, colliderCenter.z);
    }

    void Update()
    {
        Vector3 moveInput = Vector3.zero;

        if (playerState != PlayerState.YOYO && IsAirborne())
        {
            playerState = PlayerState.AIRBORNE;
        }

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
            float horizontalInput = Input.GetAxis("Horizontal");
            if (horizontalInput != 0f &&
                playerState != PlayerState.AIRBORNE) {
                if (horizontalInput < 0)
                    animator_.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, -90, 0));
                else if (horizontalInput > 0)
                    animator_.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
                animator_.Play("Walk");
                AudioController.PlaySFX("sfx_footstep01");
            } else if (playerState != PlayerState.AIRBORNE) {
                animator_.Play("Idle");
            }


            if (playerState != PlayerState.AIRBORNE &&
                Input.GetAxis("Jump") != 0f &&
                rb_.velocity.y == 0)
                Jump(jumpForce_);

            // Restrict the upward velocity of the player
            if (rb_.velocity.y >= yVelocityUpperLimit_)
                rb_.velocity = new Vector2(rb_.velocity.x, yVelocityUpperLimit_);

            Vector3 velocity = rb_.velocity;
            velocity.x = horizontalInput * walkSpeed_;
            // transform.position = pos;
            rb_.velocity = velocity;

            if (holding_ != null)
                holding_.transform.position = transform.position;

            if (Input.GetJoystickNames().Length == 0) {
                if (Input.GetMouseButtonDown(0))
                {
                    yoyo.Launch();
                    playerState = PlayerState.YOYO;
                }
            } else if (Input.GetAxisRaw("Yoyo") != 0f) {
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
            } else if (Input.GetAxisRaw("Yoyo") != 0f) {
                yoyo.Release();
                playerState = PlayerState.IDLE;
            }            
        }
        // DebugUI.instance.playerVelocity = rb_.velocity;
    }

    void OnCollisionEnter2D(Collision2D c) {
        if (c.gameObject.layer == LayerMask.NameToLayer("World")) {
            animator_.Play("Jump");
            playerState = PlayerState.GROUNDED;
            AudioController.PlaySFX("sfx_land");                        
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
        Vector3 velocity = rb_.velocity;
        velocity.y += thrust_;
        rb_.velocity = velocity;

        playerState = PlayerState.AIRBORNE;
        AudioController.PlaySFX("sfx_jump");
    }

    public bool IsAirborne()
    {
        RaycastHit2D hit = Physics2D.Raycast(bottom_, Vector3.down, 0.2f, 1 << LayerMask.NameToLayer("World"));
        if (hit && hit.collider) {            
            return false;
        }

        return true;
    }

    public void QuitYoyo()
    {
        playerState = PlayerState.AIRBORNE;
    }
}
