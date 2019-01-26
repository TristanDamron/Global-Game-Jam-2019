using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField]
    private float speed_;
    [SerializeField]
    private bool jumping_;
    private Rigidbody2D rb_;

    void Start()
    {
        rb_ = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        var pos = new Vector3();
        pos.x = transform.position.x + Input.GetAxis("Horizontal") * Time.deltaTime * speed_;   
        pos.y = transform.position.y;

        if (!jumping_ && Input.GetAxis("Jump") != 0f) {
            Jump(5f);
        }        

        pos.z = transform.position.z;
        transform.position = pos;
    }

    void OnCollisionEnter(Collision c) {
        if (c.gameObject.layer == LayerMask.NameToLayer("World")) {
            jumping_ = false;
        }
    }

    public void Jump(float thrust_) {
        rb_.AddForce(transform.up * thrust_);   
        jumping_ = true;
    }
}
