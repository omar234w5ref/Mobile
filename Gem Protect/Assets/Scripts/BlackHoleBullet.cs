using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleBullet : MonoBehaviour
{
    private Rigidbody2D rb;
    public float gravityRadius = 5f; // How far the pull effect reaches
    public float pullForce = 10f; // How strong the pull is
    public float lifeTime = 5f; // Bullet disappears after this time
    public List<string> affectedTags; // List of tags that get pulled in
    public float slowDownRate = 0.98f; // Reduces speed over time
    public float stopThreshold = 0.1f;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.mass = 1f; // Ensure it has mass
        }

        Destroy(gameObject, lifeTime); // Destroy after time expires
    }

    void FixedUpdate()
    {
        Collider2D[] objectsInRange = Physics2D.OverlapCircleAll(transform.position, gravityRadius);

        foreach (Collider2D col in objectsInRange)
        {
            if (affectedTags.Contains(col.tag)) // Only pull objects with matching tags
            {
                
                Rigidbody2D targetRb = col.GetComponent<Rigidbody2D>();
                if (targetRb != null)
                {
                    Vector2 direction = (transform.position - col.transform.position).normalized;
                    targetRb.AddForce(direction * pullForce * Time.fixedDeltaTime, ForceMode2D.Force);
                }
            }
        }
    }

    private void Update()
    {
        if (rb.velocity.magnitude > stopThreshold)
        {
            rb.velocity *= slowDownRate; // Gradually slow down
        }
        else
        {
            rb.velocity = Vector2.zero; // Stop the bullet completely
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            Destroy(collision.gameObject);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, gravityRadius);
    }
}
