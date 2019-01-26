using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private Transform target_;
    [SerializeField]
    private float delay_;

    void Start() {
        target_ = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        var moveToPos = new Vector3(target_.position.x, target_.position.y, transform.position.z);        
        transform.position = Vector3.Lerp(transform.position, moveToPos, Time.deltaTime / delay_);
    }
}
