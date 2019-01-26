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
    private Rigidbody2D rb_;
    private ParticleSystem particles_;

    void Start()
    {
        rb_ = GetComponent<Rigidbody2D>();
        particles_ = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        var pos = new Vector3();
        pos.x = transform.position.x + Input.GetAxis("Horizontal") * Time.deltaTime * speed_;   
        pos.y = transform.position.y;

        if (!jumping_ && Input.GetAxis("Jump") != 0f) {
            Jump(200f);
        }        

        pos.z = transform.position.z;
        transform.position = pos;

        if (holding_ != null) 
            holding_.transform.position = transform.position;
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
