using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public MO_Yoyo yoyo;
    public Rigidbody2D rb_;        
    [SerializeField]
    private float speed_;
    // [SerializeField]
    // private float jumpTimer_;
    // [SerializeField]
    // private float jumpDuration_;
    [SerializeField]
    private bool canJump_;
    [SerializeField]
    private float yVelocityUpperLimit_;
    [SerializeField]
    private float extraFallingGravityRate_ = 1.0f;
    [SerializeField]
    private float extraFallingGravityMax_ = 10.0f;
    [SerializeField]
    private bool yoyoing_;
    [SerializeField]
    private Transform respawn_;
    [SerializeField]
    private float jumpSpeed_;
    [SerializeField]
    private float lowJumpMultiplier_ = 3.0f;
    [SerializeField]
    private float fallMultiplier_ = 1.4f;

    [SerializeField]
    private bool didJump_;
    private bool midAirShot;
    private ParticleSystem particles_;
    private Animator animator_;

    private float extraFallingGravity_ = 0.0f;

    void Start() {
        rb_ = GetComponent<Rigidbody2D>();
        particles_ = GetComponent<ParticleSystem>();
        animator_ = GetComponentInChildren<Animator>();
    }
    
    void Update()
    {      
        if (canJump_)
            animator_.SetBool("grounded", true);
        else
            animator_.SetBool("grounded", false);
        if (Input.GetAxis("Horizontal") != 0f) {
            Vector3 pos = transform.position;
            float walkspeed = Input.GetAxis("Horizontal") * speed_ * Time.deltaTime;
            pos.x += walkspeed;
            transform.position = pos;
            
            // animator_.Play("Walk");
            animator_.SetFloat("walkspeed", Mathf.Abs(Input.GetAxis("Horizontal")));
            
            if (canJump_) {
                AudioController.PlayFootsteps();
            }
            else animator_.SetBool("grounded", false);

            if (Input.GetAxisRaw("Horizontal") == -1) {
                animator_.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, -90, 0));
            } else if (Input.GetAxisRaw("Horizontal") == 1) {
                animator_.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
            }   
        } else {
            // animator_.Play("Idle");
            animator_.SetFloat("walkspeed", 0);
        }


        //faster falling
        if (rb_.velocity.y < 0 && !yoyoing_) {
            rb_.velocity += Vector2.up * Physics.gravity.y * (fallMultiplier_ - 1) * Time.deltaTime;
        }

        // jump
        if (Input.GetAxis("Jump") != 0 && canJump_) { 
            AudioController.PlayJump();    
            didJump_ = true;
            Jump(jumpSpeed_);
        }
        //control jump height by length of time jump button held
        if (didJump_ && !yoyoing_ && rb_.velocity.y > 0 && Input.GetAxis("Jump") == 0) {
            rb_.velocity += Vector2.up * Physics.gravity.y * (lowJumpMultiplier_ - 1) * Time.deltaTime;
        }

        // Restrict the upward velocity of the player
        if (rb_.velocity.y >= yVelocityUpperLimit_) {
            rb_.velocity = new Vector2(rb_.velocity.x, yVelocityUpperLimit_);       
            Debug.Log("Reached maximum jump height"); 
        }
        else
        {
            /*
            // falling and not yoyoing
            if (!yoyoing_ && rb_.velocity.y < 0.2f)
            {
                // apply extra fake gravity
                extraFallingGravity_ += extraFallingGravityRate_;
                if (extraFallingGravity_ > extraFallingGravityMax_)
                    extraFallingGravity_ = extraFallingGravityMax_;
                // transform.position.y -= extraFallingGravityRate_ * Time.deltaTime;
                Vector2 velocity = rb_.velocity;
                velocity.y += extraFallingGravity_;
            }
            */
        }

        if (YoyoInput() && !yoyoing_ && !midAirShot) {
            AudioController.PlayYoyo();
            LaunchYoyo(); 
            StartCoroutine(EndYoyo());
        } 
    }

    void Jump(float thrust_) {        
        rb_.velocity = new Vector2(rb_.velocity.x, jumpSpeed_);
        /*
        jumpTimer_ += Time.deltaTime;

        if (jumpTimer_ < jumpDuration_) {
            Vector3 pos = transform.position;
            pos.y += (thrust_ - jumpTimer_) * Time.deltaTime;
            // transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime);
            transform.position = pos;
        }
        */
    }

    void OnTriggerEnter2D(Collider2D c) {
        if (c.gameObject.layer == LayerMask.NameToLayer("World")) {
            canJump_ = true;
            didJump_ = false;
            Debug.Log("HIT GROUND!");
            // jumpTimer_ = 0f;
            animator_.Play("Jump"); 
            AudioController.PlaySFX("sfx_land");                
            midAirShot = false;
            extraFallingGravity_ = 0.0f;
        } else if (c.gameObject.layer == LayerMask.NameToLayer("Deadzone")) {
            QuitYoyo();
            transform.position = respawn_.position;
            AudioController.PlaySpawn();         
            particles_.Play();
        }
    }

    void OnTriggerExit2D(Collider2D c) {
        if (c.gameObject.layer == LayerMask.NameToLayer("World")) {
            canJump_ = false;
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
        midAirShot = true;
    }    

    public void QuitYoyo()
    {
        yoyo.Release();
        yoyoing_ = false;
    }

    IEnumerator EndYoyo() {
        yield return new WaitForSeconds(1f);
        QuitYoyo();
    }
}
