using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallOfFire : MonoBehaviour
{

    [SerializeField] [Range(0.01f, 0.1f)] float fireWallSpeed = 0.01f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector2 (transform.position.x + fireWallSpeed, transform.position.y);
    }
}
