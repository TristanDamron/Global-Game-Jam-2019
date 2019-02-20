using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingCushion : MonoBehaviour
{

    [SerializeField] float cushionDropTime = 1f;
    [SerializeField] float cushionDestroyTime = 3f;
 
    public IEnumerator DropCushion()
    {
        yield return new WaitForSeconds(cushionDropTime);
        GetComponent<Rigidbody2D>().isKinematic = false;
        GetComponent<BoxCollider2D>().isTrigger = true;
    }

    public IEnumerator DestroyCushion()
    {
        yield return new WaitForSeconds(cushionDestroyTime);
        Destroy(gameObject);
    }


    void OnCollisionEnter2D(Collision2D player)
    {
        StartCoroutine(DropCushion());
        StartCoroutine(DestroyCushion());
    }
}
