using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField]
    private float speed_;
    [SerializeField]
    private bool jumping_;
    [SerializeField]
    private Transform respawnLocation_;
    [SerializeField]
    private GameObject holding_;
    private Animator animator_;
    private Rigidbody2D rb_;
    private ParticleSystem particles_;
    private int direction_;

    void Start()
    {
        rb_ = GetComponent<Rigidbody2D>();
        particles_ = GetComponent<ParticleSystem>();
        animator_ = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        var pos = new Vector3();
        pos.x = transform.position.x + Input.GetAxis("Horizontal") * Time.deltaTime * speed_;   
        pos.y = transform.position.y;

        pos.z = transform.position.z;
        transform.position = pos;

        if (Input.GetAxis("Horizontal") != 0f) {
            animator_.Play("Walk");
            direction_ = (int)Input.GetAxisRaw("Horizontal");
        } else {
            animator_.Play("Idle");
        }

        if (!jumping_ && Input.GetAxis("Jump") != 0f && rb_.velocity.y == 0) {
            Jump(200f);
        }        

        // Restrict the upward velocity of the player
        if (rb_.velocity.y >= 7f)
            rb_.velocity = new Vector2(rb_.velocity.x, 7f);

        if (holding_ != null) 
            holding_.transform.position = transform.position;

        // Rotate the 3D model
        transform.GetChild(0).rotation = Quaternion.Euler(new Vector3(0f, 90f * direction_, 0f));
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
}
