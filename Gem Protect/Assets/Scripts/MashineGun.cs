using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MashineGun : Weapon
{
    public GameObject bulletPrefab;
    public float shootingInterval = 0.5f; // Interval between shots in seconds
    private float nextShootTime = 0f;

    public override void Shoot(Vector3 direction)
    {
        if (Time.time >= nextShootTime)
        {
            Camera.main.GetComponent<CameraFollow>().TriggerShake(0.1f, 0.05f);
            FindObjectOfType<AudioManager>().Play("mashineGunShot");
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            bullet.GetComponent<Bullet>().shopSlot = shopSlot;
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = direction * bulletSpeed;
            }
            nextShootTime = Time.time + shopSlot.shootingIntervel;
        }
    }
}
