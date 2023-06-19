using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{

    public Animator animator;

    public float moveSpeed = 5f;

    public Rigidbody2D rb;

    public Transform spriteTransform;

    Vector2 movement;

    public bool andando = false;

    public Collider2D attackPoint;

    public float attackRange = 1f;

    public LayerMask enemyLayer;


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
            rb.rotation = angle;

            animator.SetTrigger("AndandoTrue");
        }

        if (movement.magnitude == 0)
        {
            animator.SetTrigger("AndandoFalse");
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }


    }

    void FixedUpdate()
    {
        //movimento
        rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
    }







    private void Attack()
    {
        animator.SetTrigger("Attack");

        // Atacar
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.transform.position, attackRange, enemyLayer);
        
        // Damage or interact with the hit enemies
        foreach (Collider2D enemy in hitEnemies)
        {
            
            // Perform attack action on the enemy
            Debug.Log("Attacked enemy: " + enemy.name);
        }
    }

        private void OnDrawGizmosSelected()
    {
        // Draw attack range gizmo in the scene view
        if (attackPoint != null)
        {
            Gizmos.DrawWireSphere(attackPoint.transform.position, attackRange);
        }
    }
}
