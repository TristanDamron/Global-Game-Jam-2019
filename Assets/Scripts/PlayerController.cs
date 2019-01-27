using System.Collections;
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
    [SerializeField]
    private Transform respawn_;
    [SerializeField]
    private float jumpSpeed_;
    private ParticleSystem particles_;
    private Animator animator_;
    

    void Start() {
        rb_ = GetComponent<Rigidbody2D>();
        particles_ = GetComponent<ParticleSystem>();
        animator_ = GetComponentInChildren<Animator>();
    }
    
    void Update()
    {      
        if (Input.GetAxis("Horizontal") != 0f) {
            Vector3 pos = transform.position;
            pos.x = pos.x + Input.GetAxis("Horizontal") * speed_ * Time.deltaTime;
            transform.position = pos;

            animator_.Play("Walk");
            AudioController.PlayFootsteps();

            if (Input.GetAxisRaw("Horizontal") == -1) {
                animator_.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, -90, 0));
            } else if (Input.GetAxisRaw("Horizontal") == 1) {
                animator_.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
            }   
        } else {
            animator_.Play("Idle");
        }

        if (Input.GetAxis("Jump") != 0) { 
            AudioController.PlayJump();    
            Jump(jumpSpeed_);
        }

        // Restrict the upward velocity of the player
        if (rb_.velocity.y >= yVelocityUpperLimit_) {
            rb_.velocity = new Vector2(rb_.velocity.x, yVelocityUpperLimit_);       
            Debug.Log("Reached maximum jump height"); 
        }

        if (YoyoInput() && !yoyoing_) {
            AudioController.PlayYoyo();
            LaunchYoyo(); 
            StartCoroutine(EndYoyo());
        } 
    }

    void Jump(float thrust_) {        
        jumpTimer_++;

        if (jumpTimer_ < jumpDuration_) {
            Vector3 pos = transform.position;
            pos.y += thrust_ - jumpTimer_;
            transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime);
        }
    }

    void OnCollisionEnter2D(Collision2D c) {
        if (c.gameObject.layer == LayerMask.NameToLayer("World")) {
            jumping_ = false;
            jumpTimer_ = 0f;
            animator_.Play("Jump"); 
            AudioController.PlaySFX("sfx_land");                
        } else if (c.gameObject.layer == LayerMask.NameToLayer("Deadzone")) {
            transform.position = respawn_.position;
            AudioController.PlaySpawn();         
            particles_.Play();
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

    IEnumerator EndYoyo() {
        yield return new WaitForSeconds(1f);
        QuitYoyo();
    }
}
