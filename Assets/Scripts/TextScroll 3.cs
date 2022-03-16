﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextScroll : MonoBehaviour
{
    RectTransform rectTransform;
    public Vector3 scrollVector = Vector3.up * 35.0f;
    //[SerializeField] float scrollStopHeight = 1800f;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        // this.rectTransform.y -= scrollRate * Time.deltaTime;
        this.transform.position += scrollVector * Time.deltaTime;
        //if(this.transform.position.y >= scrollStopHeight)
       //{
        //    scrollVector = Vector3.up * 0f;
        //}
    }
}
