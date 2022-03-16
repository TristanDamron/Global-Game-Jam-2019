using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private Transform target_;
    [SerializeField]
    private float delay_;

    private Vector3 targetOffset;

    void Start() {
        target_ = GameObject.FindGameObjectWithTag("Player").transform;
        targetOffset = transform.position - target_.transform.position;
    }

    void Update()
    {
        Vector3 moveToPos = target_.position + targetOffset;
        transform.position = Vector3.Lerp(transform.position, moveToPos, Time.deltaTime / delay_);
    }
}
