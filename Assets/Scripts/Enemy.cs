using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float speed = 1f;
    public int health = 3;

    private Transform playerTransform;

    // Start is called before the first frame update
    void Start()
    {
     GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
        playerTransform = playerObject.GetComponent<Transform>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerTransform != null)
    {
        // Calculate the direction from the enemy to the player
        Vector3 direction = playerTransform.position - transform.position;
        
        // Normalize the direction to have a magnitude of 1
        direction.Normalize();
        
        // Move the enemy towards the player
        transform.position += direction * speed * Time.deltaTime;


        if(health == 0){
            Destroy(gameObject);
        }
    }
    }
}
