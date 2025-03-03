using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemMovement : MonoBehaviour
{
    public float speed = 5f; // Normal movement speed
    public Vector2 areaSize = new Vector2(10f, 10f);
    private Vector3 targetPosition;
    private Rigidbody2D rb;

    [Header("Magnet Effect")]
    public bool isMagnetized = false; // Controlled by Magnet Boss
    public float magnetPullForce = 10f; // How strong the pull is
    private Transform magnetSource; // Where the pull comes from

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get Rigidbody
        SetRandomTargetPosition();
    }

    void Update()
    {
        if (!isMagnetized) 
        {
            MoveToTargetPosition();
        }
    }

    void FixedUpdate()
    {
        if (isMagnetized && magnetSource != null)
        {
            ApplyMagnetPull();
        }
    }

    void SetRandomTargetPosition()
    {
        float randomX = Random.Range(-areaSize.x / 2, areaSize.x / 2);
        float randomY = Random.Range(-areaSize.y / 2, areaSize.y / 2);
        targetPosition = new Vector3(randomX, randomY, transform.position.z);
    }

    void MoveToTargetPosition()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        if (transform.position == targetPosition)
        {
            SetRandomTargetPosition();
        }
    }

    void ApplyMagnetPull()
    {
        if (magnetSource == null) return;

        Vector2 pullDirection = (magnetSource.position - transform.position).normalized;
        rb.AddForce(pullDirection * magnetPullForce * Time.fixedDeltaTime, ForceMode2D.Force);
    }

    // This function is called by the Magnet Boss to start pulling
    public void ActivateMagnet(Transform source, float strength)
    {
        isMagnetized = true;
        magnetSource = source;
        magnetPullForce = strength;
    }

    // This function is called when the magnet effect should stop
    public void DeactivateMagnet()
    {
        isMagnetized = false;
        magnetSource = null;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(areaSize.x, areaSize.y, 0));
    }
}
