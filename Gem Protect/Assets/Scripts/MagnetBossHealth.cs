using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetBossHealth : MonoBehaviour
{
    [Header("Boss Health Settings")]
    public int maxHealth = 100;
    private int currentHealth;
    public float hitCooldown = 1f; // Interval before the same hitbox can be hit again

    [Header("Hitboxes")]
    public GameObject[] hitboxes; // Assign hitboxes in the Inspector
    private Dictionary<GameObject, float> hitboxCooldowns = new Dictionary<GameObject, float>();

    [Header("Effects")]
    public GameObject damageEffect; // Assign a hit effect (particle, flash, etc.)
    public GameObject deathEffect; // Assign a death effect (explosion, etc.)
    private void Start()
    {
        currentHealth = maxHealth;

        // Initialize cooldowns for each hitbox
        foreach (GameObject hitbox in hitboxes)
        {
            hitboxCooldowns[hitbox] = 0f;
        }
    }

    private void Update()
    {
        // Update cooldowns for each hitbox
        List<GameObject> keys = new List<GameObject>(hitboxCooldowns.Keys);
        foreach (GameObject hitbox in keys)
        {
            if (hitboxCooldowns[hitbox] > 0)
            {
                hitboxCooldowns[hitbox] -= Time.deltaTime;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            // Check which hitbox was hit
            foreach (GameObject hitbox in hitboxes)
            {
                if (collision.gameObject.transform.IsChildOf(hitbox.transform))
                {
                    if (hitboxCooldowns[hitbox] <= 0)
                    {
                        TakeDamage(10, hitbox);
                        hitboxCooldowns[hitbox] = hitCooldown; // Start cooldown for this hitbox
                    }
                    Destroy(collision.gameObject); // Destroy bullet on impact
                    break;
                }
            }
        }
    }

    void TakeDamage(int damage, GameObject hitbox)
    {
        currentHealth -= damage;
        Debug.Log($"Boss took {damage} damage! Current health: {currentHealth}");

        // Play hit effect
        if (damageEffect != null)
            Instantiate(damageEffect, hitbox.transform.position, Quaternion.identity);

        // Play hit sound
        

        // Check if the boss is dead
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Boss Defeated!");

        // Play death effect
        if (deathEffect != null)
            Instantiate(deathEffect, transform.position, Quaternion.identity);

        // Play death sound
    

        // Destroy boss
        Destroy(gameObject);
    }
}
