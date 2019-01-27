using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextBoxTrigger : MonoBehaviour
{
    public string[] textboxesContents;
    public bool hideOnStart = false;

    void Start()
    {
        if (hideOnStart)
            GetComponent<SpriteRenderer>().enabled = false;
    }

    void OnTriggerEnter2D(Collider2D c)
    {
        if (!c.GetComponent<PlayerController>()) return;
        TextBoxController.CreateTextBoxes(textboxesContents);
        Destroy(this.gameObject);
    }
}
