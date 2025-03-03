using UnityEngine;
using System.Collections;

public class MagnetKingBoss : MonoBehaviour
{
    [Header("Gem & Magnet Properties")]
    public Transform gem;
    public float pullStrength = 5f;
    public float pullRange = 10f;
    public float stunDuration = 3f;
    public float returnSpeed = 2f;
    public float followSpeed = 1.5f; // Base speed for following the gem
    public float acceleration = 0.5f; // How fast the boss accelerates
    public float deceleration = 0.7f; // How fast the boss slows down
    public float minDistanceFromGem = 1.5f; // The minimum distance before stopping near the gem

    [Header("Dash Attack Settings")]
    public float dashSpeed = 5f;
    public float dashCooldown = 5f;
    private float nextDashTime = 0f;
    
    [Header("Shockwave Settings")]
    public float knockbackForce = 5f;
    public GameObject shockwaveEffect;

    [Header("Line Renderer Settings")]
    public LineRenderer lineRenderer;
    public GameObject magnetPullHitbox;
    public Color startColor = Color.blue;
    public Color endColor = Color.cyan;
    public float startWidth = 0.1f;
    public float endWidth = 0.2f;

    [Header("Borders & Collision")]
    public LayerMask borderLayer;

    private Rigidbody2D rb;
    private bool isStunned = false;
    private bool isReturning = false;
    private Vector2 lastPosition;
    private Vector2 velocity = Vector2.zero; // Used for smooth acceleration/deceleration

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lastPosition = transform.position; // Save initial position

        if (lineRenderer == null)
            lineRenderer = gameObject.AddComponent<LineRenderer>();

        SetupLineRenderer();
    }

    void Update()
    {
        if (gem == null) return;

        if (!isStunned)
            UpdateMagnetPullLine();
    }

    void FixedUpdate()
    {
        if (isReturning)
        {
            ReturnToLastPosition();
        }
        else if (!isStunned)
        {
            float distanceToGem = Vector2.Distance(transform.position, gem.position);

            if (distanceToGem < pullRange)
            {
                ApplyMagnetPull();
            }
            else
            {
                FollowGem();
            }

            if (Time.time >= nextDashTime && distanceToGem > pullRange * 1.5f)
            {
                DashTowardGem();
                nextDashTime = Time.time + dashCooldown;
            }
        }
    }

    void SetupLineRenderer()
    {
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = startWidth;
        lineRenderer.endWidth = endWidth;
        lineRenderer.startColor = startColor;
        lineRenderer.endColor = endColor;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.enabled = false;
    }

    void UpdateMagnetPullLine()
    {
        if (gem == null || isReturning || isStunned)
        {
            lineRenderer.enabled = false;
            if (magnetPullHitbox != null) magnetPullHitbox.SetActive(false);
            return;
        }

        float distance = Vector2.Distance(transform.position, gem.position);
        if (distance < pullRange)
        {
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, gem.position);

            // Move hitbox to match the pull line
            if (magnetPullHitbox != null)
            {
                magnetPullHitbox.SetActive(true);
                magnetPullHitbox.transform.position = (transform.position + gem.position) / 2;
                Vector2 direction = (gem.position - transform.position).normalized;
                magnetPullHitbox.transform.up = direction;
                magnetPullHitbox.transform.localScale = new Vector3(0.2f, distance, 1);
            }
        }
        else
        {
            lineRenderer.enabled = false;
            if (magnetPullHitbox != null) magnetPullHitbox.SetActive(false);
        }
    }

    void ApplyMagnetPull()
    {
        if (gem == null || isStunned) return;

        Vector2 pullDirection = (transform.position - gem.position).normalized;
        rb.velocity = Vector2.zero;
        rb.AddForce(-pullDirection * pullStrength, ForceMode2D.Force);
    }

    void FollowGem()
    {
        if (gem == null) return;

        Vector2 direction = (gem.position - transform.position).normalized;
        float distance = Vector2.Distance(transform.position, gem.position);

        if (distance > minDistanceFromGem) // Prevents overlapping with the gem
        {
            // Smooth acceleration and deceleration
            rb.velocity = Vector2.SmoothDamp(rb.velocity, direction * followSpeed, ref velocity, acceleration);
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    void DashTowardGem()
    {
        if (gem == null) return;

        Debug.Log("Boss is dashing toward the gem!");
        Vector2 dashDirection = (gem.position - transform.position).normalized;
        rb.velocity = dashDirection * dashSpeed;
    }

    public void StunMagnetPull()
    {
        Debug.Log("Magnet Pull Stunned by Player!");
        StartCoroutine(DisableMagnetTemporarily());
    }

    IEnumerator DisableMagnetTemporarily()
    {
        isStunned = true;
        lineRenderer.enabled = false;
        if (magnetPullHitbox != null) magnetPullHitbox.SetActive(false);

        yield return new WaitForSeconds(stunDuration);

        isStunned = false;
        Debug.Log("Boss recovered! Magnet Pull Reactivated!");
    }

    void TriggerShockwave()
    {
        Debug.Log("Shockwave Activated! Boss knocked back!");

        if (shockwaveEffect != null)
        {
            Instantiate(shockwaveEffect, gem.position, Quaternion.identity);
        }

        lastPosition = transform.position;

        Vector2 knockbackDirection = (transform.position - gem.position).normalized;
        rb.velocity = Vector2.zero;
        rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);

        StartCoroutine(StunBoss());
    }

    IEnumerator StunBoss()
    {
        isStunned = true;
        rb.velocity *= 0.3f; // Slow down before stopping
        yield return new WaitForSeconds(stunDuration);
        isStunned = false;
        Debug.Log("Boss recovered from stun!");

        StartReturning();
    }

    void StartReturning()
    {
        isReturning = true;
        rb.velocity = Vector2.zero;
    }

    void ReturnToLastPosition()
    {
        Vector2 direction = (lastPosition - (Vector2)transform.position).normalized;
        rb.velocity = direction * returnSpeed;

        if (Vector2.Distance(transform.position, lastPosition) < 0.5f)
        {
            rb.velocity = Vector2.zero;
            isReturning = false;
        }
    }
}
