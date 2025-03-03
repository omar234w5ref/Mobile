using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleGun : Weapon
{
    [Header("Gun Configurations")]
    [SerializeField] private GameObject bulletPrefab;

    [Header("Effects")]
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private string shootSfx;
    [SerializeField] private float camShakeIntensity;
    [SerializeField] private float camShakeLength;

    //----------------------------
    CameraFollow cam;
    AudioManager audioManager;
    private float nextshootTime;

    private Vector2 direction1;
    private void Start()
    {
        cam = FindObjectOfType<CameraFollow>();
        audioManager = FindObjectOfType<AudioManager>();
    }

    void Update()
    {

    }

    public override void Shoot(Vector3 direction)
    {

        if (Time.time >= nextshootTime)
        {
            cam.TriggerShake(camShakeLength, camShakeIntensity);
            audioManager.Play(shootSfx);

            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.transform.position,
                Quaternion.identity);
            ParticleSystem _muzzleFlash = Instantiate(muzzleFlash, bulletSpawnPoint.transform.position, transform.rotation);
            _muzzleFlash.Play();

            //bullet.GetComponent<Bullet>().shopSlot = shopSlot;

            Destroy(_muzzleFlash, 2f);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = direction * bulletSpeed;
            }
            nextshootTime = Time.time + shopSlot.shootingIntervel;


            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            bullet.transform.rotation = Quaternion.Euler(0, 0, angle - 90);

            Destroy(bullet, 5);
        }

    }
}
