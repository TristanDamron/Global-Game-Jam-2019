using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallOfFire : MonoBehaviour
{
    [SerializeField] float fireWallSpeed = 0.6f;
    [SerializeField] float respawnDistance = 7f;
    float wallXPos;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        wallXPos = transform.position.x;
        transform.position = new Vector3 (transform.position.x + (fireWallSpeed * Time.deltaTime), transform.position.y, transform.position.z);
        if (wallXPos >= 113f)
        {
            gameObject.layer = 11;
        }
        if (wallXPos >= 237f)
        {
            gameObject.layer = 12;
        }

    }

    public void RespawnWall ()
    {
        PlayerController player = FindObjectOfType<PlayerController>();
        transform.position = new Vector3 (player.transform.position.x - respawnDistance, transform.position.y, transform.position.z);
    }
}
