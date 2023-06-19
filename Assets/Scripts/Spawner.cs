using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject enemy;

    public bool inRange;

    public Transform transform;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("spawnEnemy", 8f);
    }

    // Update is called once per frame
    void Update()
    {
    
    }

    void spawnEnemy()
    {
        if(inRange)
        {
            Instantiate(enemy, new Vector3(-6.98f,20.45f,0f), Quaternion.identity);
           
        }
        Invoke("spawnEnemy", 8f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            inRange = true;
            Debug.Log("Range");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {   
        if(other.tag == "Player")
        {
            inRange = false;
            Debug.Log("OutOfRange");
        }
    }
}
