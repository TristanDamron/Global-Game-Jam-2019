using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextScrollStopper : MonoBehaviour
{

    [SerializeField] Collider2D scrollStopperText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {


    }

    private void OnTriggerEnter2D(Collider2D otherObject)
    {
        if (otherObject = scrollStopperText)
        {
            FindObjectOfType<TextScroll>().scrollVector = Vector3.up * 0f;
        }
    }
}
