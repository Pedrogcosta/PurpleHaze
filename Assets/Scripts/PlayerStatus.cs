using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerStatus : MonoBehaviour
{
    //public Slider healthBar;
    public float currentHealth;
    public float maxHealth;
    public Collider2D collider;
    public LayerMask enemyLayer;
    public TMP_Text lifeDisplay;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealth();
    }

    void Update()
    {
        CheckDamage();
        if (currentHealth <= 0)
        {
            SceneManager.LoadScene("EndScreen");
        }
    }

    public void UpdateHealth()
    {
        lifeDisplay.text = "S2: " + currentHealth.ToString();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log("Take damage");
        UpdateHealth();
    }

    void CheckDamage()
    {

        Collider2D[] enemyContact = Physics2D.OverlapCircleAll(collider.transform.position, 1f, enemyLayer);

        foreach (Collider2D enemy in enemyContact)
        {
            TakeDamage(1);
            Debug.Log("Dano de = " + enemy.name);
        }
    }


    
    private void OnDrawGizmosSelected()
    {
        // Draw attack range gizmo in the scene view
        if (collider != null)
        {
            Gizmos.DrawWireSphere(collider.transform.position, 1f);
        }
    }
}
