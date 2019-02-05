using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlayTrigger : MonoBehaviour
{
    public bool hideOnStart = true;
    public Overlay overlay;

    // Start is called before the first frame update
    void Start()
    {
        if (hideOnStart)
            GetComponent<SpriteRenderer>().enabled = false;
    }

    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.tag != "Player") return;
        overlay.gameObject.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D c)
    {
        if (c.tag != "Player") return;
        overlay.gameObject.SetActive(false);
    }
}
