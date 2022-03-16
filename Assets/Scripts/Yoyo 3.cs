using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yoyo : MonoBehaviour
{
    private LineRenderer lr_;
    private SpringJoint2D spring_;
    
    void Start()
    {
        lr_ = GetComponent<LineRenderer>(); 
        spring_ = GetComponent<SpringJoint2D>(); 
        lr_.positionCount = 1;             
    }
    
    void Update()
    {
        lr_.SetPosition(0, transform.position);
        if (Input.GetMouseButtonDown(0)) {
            DrawLine();
        }        
    }

    private void DrawLine() {         
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);		
        var hit2D = Physics2D.Raycast(transform.position, mousePos);

        if (hit2D.collider != null) {
            lr_.positionCount = 2;             
            lr_.SetPosition(1, hit2D.transform.position);
            var empty = Instantiate(new GameObject());
            empty.transform.position = hit2D.transform.position;
            var rb = empty.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0f;
            rb.freezeRotation = true;            
            spring_.connectedBody = empty.GetComponent<Rigidbody2D>();
        }
    }
}
