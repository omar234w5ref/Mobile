using UnityEngine;
using System.Collections.Generic;

public class Pistol : Weapon
{
    [SerializeField] public ShopSlot shopSlot;
    public GameObject bulletPrefab;
    private float nextShootTime = 0f;

    // Cached references to avoid repeated lookups.
    private CameraFollow camFollow;
    private AudioManager audioManager;

    // Simple object pool for bullets.
    private static Queue<GameObject> bulletPool = new Queue<GameObject>();
    private static int poolSize = 20;

    void Awake()
    {
        // Cache the CameraFollow component from the main camera and AudioManager.
        camFollow = Camera.main.GetComponent<CameraFollow>();
        audioManager = FindObjectOfType<AudioManager>();

        // Initialize the bullet pool if it's empty.
        if (bulletPool.Count == 0)
        {
            for (int i = 0; i < poolSize; i++)
            {
                GameObject bullet = Instantiate(bulletPrefab);
                bullet.SetActive(false);
                bulletPool.Enqueue(bullet);
            }
        }
    }

    public override void Shoot(Vector3 direction)
    {
        if (Time.time >= nextShootTime)
        {
            // Use cached references to trigger effects.
            camFollow?.TriggerShake(0.1f, 0.05f);
            audioManager?.Play("PistolShot");

            // Get a bullet from the pool.
            GameObject bullet = GetBulletFromPool();
            bullet.transform.position = bulletSpawnPoint.position;
            bullet.transform.rotation = bulletSpawnPoint.rotation;
            bullet.SetActive(true);

            // Set the bullet velocity.
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = direction * bulletSpeed;
            }

            // Set the next allowed shoot time using the shooting interval from the ShopSlot.
            nextShootTime = Time.time + shopSlot.shootingIntervel;
        }
    }

    // Retrieves a bullet from the pool, or instantiates a new one if needed.
    private GameObject GetBulletFromPool()
    {
        if (bulletPool.Count > 0)
        {
            return bulletPool.Dequeue();
        }
        else
        {
            // Pool is empty; instantiate a new bullet.
            return Instantiate(bulletPrefab);
        }
    }

    // Call this method (for example, from a bullet script) when a bullet should be recycled.
    public static void ReturnBulletToPool(GameObject bullet)
    {
        bullet.SetActive(false);
        bulletPool.Enqueue(bullet);
    }
}
