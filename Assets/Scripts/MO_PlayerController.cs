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
    private float walkSpeedMax_;
    [SerializeField]
    private float walkFriction_;
    [SerializeField]
    private float airSpeed_;
    [SerializeField]
    private float airSpeedMax_;
    [SerializeField]
    private float airFriction_;

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
    private PlayerState playerState;
    private float jumpCounter;
    [SerializeField]
    private float jumpDuration;

    private Vector3 top_;
    private Vector3 right_;
    private Vector3 bottom_;
    private Vector3 left_;

    private bool yoyoUsed;

    public enum PlayerState
    {
        AIRBORNE,
        GROUNDED,
        YOYO,
    }

    void Start()
    {
        rb_ = GetComponent<Rigidbody2D>();
        particles_ = GetComponent<ParticleSystem>();
        animator_ = GetComponentInChildren<Animator>();
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        Vector3 colliderCenter = collider.bounds.center;
        // Vector3 colliderSize = collider.size;
        Vector3 colliderMin = collider.bounds.min;
        Vector3 colliderMax = collider.bounds.max;
        bottom_ = new Vector3(colliderCenter.x, colliderCenter.y - colliderMax.y, colliderCenter.z);
        top_ = new Vector3(colliderCenter.x, colliderCenter.y - colliderMin.y, colliderCenter.z);
        right_ = new Vector3(colliderCenter.x - colliderMin.x, colliderCenter.y - colliderMax.y / 2, colliderCenter.z);
        left_ = new Vector3(colliderCenter.x - colliderMax.x, colliderCenter.y - colliderMax.y / 2, colliderCenter.z);
        // bottom_ = new Vector3(colliderCenter.x, colliderCenter.y, colliderCenter.z);
        // top_ = new Vector3(colliderCenter.x, colliderCenter.y, colliderCenter.z);
        // right_ = new Vector3(colliderCenter.x, colliderCenter.y, colliderCenter.z);
        // left_ = new Vector3(colliderCenter.x, colliderCenter.y, colliderCenter.z);
    }

    void Update()
    {
        Debug.DrawRay(transform.position + bottom_, Vector3.down * 2.0f, Color.red);
        Debug.DrawRay(transform.position + top_, Vector3.up * 2.0f, Color.green);
        Debug.DrawRay(transform.position + left_, Vector3.left * 2.0f, Color.cyan);
        Debug.DrawRay(transform.position + right_, Vector3.right * 2.0f, Color.blue);
        Vector3 moveInput = Vector3.zero;
        float horizontalInput = Input.GetAxis("Horizontal");

        // switch
        if (horizontalInput < 0)
            animator_.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, -90, 0));
        else if (horizontalInput > 0)
            animator_.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));

        // check if in air
        bool inAir = IsAirborne();
        //DebugUI.instance.isAirborne = inAir;

        if (inAir)
        {
            if (playerState != PlayerState.YOYO)
                playerState = PlayerState.AIRBORNE;
        }
        else
        {
            // might not be necessary
            if (playerState == PlayerState.YOYO)
                QuitYoyo();
            playerState = PlayerState.GROUNDED;
        }

        switch (playerState)
        {
            case PlayerState.AIRBORNE:
                AirborneUpdate(horizontalInput);
                break;
            case PlayerState.GROUNDED:
                GroundedUpdate(horizontalInput);
                break;
            case PlayerState.YOYO:
                horizontalInput = 0.0f;
                if (YoyoInput()) QuitYoyo();
                break;
        }

        /*
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
            if (Input.GetMouseButtonDown(0) || Input.GetAxisRaw("Yoyo") != 0f)
            {
                yoyo.Release();
                playerState = PlayerState.IDLE;
            }
        }
        */
        // DebugUI.instance.playerVelocity = rb_.velocity;
    }

    void GroundedUpdate(float horizontalInput)
    {
        if (YoyoInput()) {
            LaunchYoyo();
            return;
        }

        if (Input.GetAxis("Jump") != 0.0f) {
            Jump(jumpForce_);
        }

        if (horizontalInput != 0)
            animator_.Play("Walk");
        else
            animator_.Play("Idle");
        AudioController.PlaySFX("sfx_footstep01");

        Vector3 pos = transform.localPosition;
        pos.x = horizontalInput * walkSpeed_ * Time.deltaTime;
        transform.localPosition = pos;
        // velocity.x += horizontalInput * walkSpeed;

        /*
        // Apply friction
        if (Mathf.Abs(velocity.x) > walkFriction_)
            velocity.x -= walkFriction_ * -Mathf.Sign(horizontalInput);
        else
            velocity.x = 0;

        // Limit to max speed
        if (Mathf.Abs(velocity.x) > walkSpeedMax_)
            velocity.x = walkSpeedMax_ * Mathf.Sign(velocity.x);
            */

    }

    void AirborneUpdate(float horizontalInput)
    {
        animator_.Play("Idle");
        if (YoyoInput()) LaunchYoyo();

        Vector3 velocity = rb_.velocity;
        velocity.x = horizontalInput * airSpeed_;
        velocity.y -= extraJumpGravity_ * Time.deltaTime;

        if (CollisionCheck(left_, Vector3.left, 0.2f))
        {
            velocity.x = 0.1f;
        }
        else if (CollisionCheck(right_, Vector3.left, 0.2f))
        {
            velocity.x = -0.1f;
        }

        /*
        // Apply friction
        if (Mathf.Abs(velocity.x) > airFriction_)
            velocity.x -= airFriction_ * -Mathf.Sign(horizontalInput);
        else
            velocity.x = 0;

        // Limit to max speed
        if (Mathf.Abs(velocity.x) > airSpeedMax_)
            velocity.x = airSpeedMax_ * Mathf.Sign(velocity.x);
            */

        rb_.velocity = velocity;
    }

    bool YoyoInput()
    {
        if (!yoyo) return false;
        return (Input.GetMouseButtonDown(0) || Input.GetAxisRaw("Yoyo") != 0f);
    }

    void OnCollisionEnter2D(Collision2D c) {
        bool collideBottom;
        bool collideTop;
        bool collideLeft;
        bool collideRight;
        float checkDistance = 0.2f;
        float pushVelocity = 200.0f;
        if (c.gameObject.layer == LayerMask.NameToLayer("World")) {
            /*
            animator_.Play("Jump");
            playerState = PlayerState.GROUNDED;
            AudioController.PlaySFX("sfx_land");                        
            */

            // if (playerState == PlayerState.YOYO)
            //    QuitYoyo();

            collideBottom = CollisionCheck(bottom_, Vector3.down, checkDistance);
            collideTop = CollisionCheck(top_, Vector3.up, checkDistance);
            collideLeft = CollisionCheck(left_, Vector3.left, checkDistance);
            collideRight = CollisionCheck(right_, Vector3.right, checkDistance);

            if (collideTop) rb_.AddForce(Vector3.down * pushVelocity);
            if (collideLeft) rb_.AddForce(Vector3.right * pushVelocity);
            if (collideRight) rb_.AddForce(Vector3.left * pushVelocity);


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
        jumpCounter++;

        if (jumpCounter < jumpDuration) {
            Vector3 pos = transform.localPosition;
            pos.y += thrust_ * Time.deltaTime;
            transform.localPosition = pos;
            Debug.Log("Jump!");
        }
        AudioController.PlaySFX("sfx_jump");
    }

    public bool IsAirborne()
    {
        Debug.DrawRay(transform.position + bottom_, Vector3.down * 0.2f);
        return !CollisionCheck(bottom_, Vector3.down, 0.2f);
    }

    private bool CollisionCheck(Vector3 origin, Vector3 direction, float distance)
    {
        origin = transform.localPosition + origin;
        direction = transform.localPosition + direction;
        RaycastHit2D hit = Physics2D.Raycast(origin, direction, distance, 1 << LayerMask.NameToLayer("World"));
        if (hit && hit.collider)
            return true;
        return false;
    }

    public void LaunchYoyo()
    {
        yoyo.Launch();
        playerState = PlayerState.YOYO;
    }

    public void QuitYoyo()
    {
        yoyo.Release();
        playerState = PlayerState.AIRBORNE;
    }
}
