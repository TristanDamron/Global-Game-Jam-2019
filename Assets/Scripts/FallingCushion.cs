using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingCushion : MonoBehaviour
{

    [SerializeField] float cushionDropTime = 1f;
    [SerializeField] float cushionDestroyTime = 15f;
    [SerializeField] float cushionRespawnTime = 10f;
    Vector2 startPos;


    private void Start()
    {
        startPos = transform.position;
    }

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

    public IEnumerator RespawnCusion()
    {
        yield return new WaitForSeconds(cushionRespawnTime);
        GetComponent<Rigidbody2D>().isKinematic = true;
        GetComponent<BoxCollider2D>().isTrigger = false;
        Instantiate(gameObject, startPos, Quaternion.identity);
    }


    void OnCollisionEnter2D(Collision2D player)
    {
        StartCoroutine(DropCushion());
        StartCoroutine(DestroyCushion());
        StartCoroutine(RespawnCusion());
    }
}
