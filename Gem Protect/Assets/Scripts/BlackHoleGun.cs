using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleGun : Weapon
{
    [SerializeField] public ShopSlot shopSlot;
    public GameObject bulletPrefab;
    private float nextShootTime = 0f;
    public Joystick Joystick;
    private Animator anim;


    bool isShooting;
    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public override void Shoot(Vector3 direction)
    {
        isShooting = false;
        if (Time.time >= nextShootTime)
        {
            isShooting = true;
            Camera.main.GetComponent<CameraFollow>().TriggerShake(0.1f, 0.05f);
            FindObjectOfType<AudioManager>().Play("PistolShot");

            GameObject blackHole = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            Rigidbody2D rb = blackHole.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                rb.velocity = direction.normalized * bulletSpeed;
            }
           
            nextShootTime = Time.time + shopSlot.shootingIntervel;
        }
    }

    private void Update()
    {
        if(isShooting)
        {
            Animations();
        }
    }

    public void Animations()
    {
        if (anim == null) return; // Safety check

        anim.SetBool("Shoot", true);

        // Check if animation is almost complete
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("BlackHoleGunShoot") &&
            anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
        {
            anim.SetBool("Shoot", false);
            isShooting = false; // Reset after animation finishes
        }
    }

}
