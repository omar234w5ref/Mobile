using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revolver : Weapon
{
    public GameObject bulletPrefab;
    private float nextShootTime = 0f;
    public ParticleSystem MuzzleFlash;
    public override void Shoot(Vector3 direction)
    {
        if (Time.time >= nextShootTime)
        {
            Camera.main.GetComponent<CameraFollow>().TriggerShake(0.1f, 0.05f);
            FindObjectOfType<AudioManager>().Play("RevolverShoot");
            Instantiate(MuzzleFlash,bulletSpawnPoint.transform.position,Quaternion.identity);
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = direction * bulletSpeed;
            }
            nextShootTime = Time.time + shopSlot.shootingIntervel;
        }
    }
}
