using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallOfFire : MonoBehaviour
{

    [SerializeField] float fireWallSpeed = 0.6f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector2 (transform.position.x + (fireWallSpeed * Time.deltaTime), transform.position.y);
    }
}
