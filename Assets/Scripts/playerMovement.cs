using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{

    public float moveSpeed = 5f;

    public Rigidbody2D rb;

    public Transform spriteTransform;

    Vector2 movement;

    void Start()
    {
        spriteTransform = transform.GetChild(0);
    }

    void Update()
    {
        //pegar os inputs
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (movement.magnitude > 0.1f)
        {
            float angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg;
            spriteTransform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }    
    }

    void FixedUpdate()
    {
        //movimento
        rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);

    }
}
