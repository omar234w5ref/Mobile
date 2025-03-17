using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Joystick joystick;
    public Transform gunHolder;
    public float gunOffsetMagnitude = 1f;

    public List<GameObject> weapons;
    public GameObject crossHair;
    public bool stunned;

    void Start()
    {
        // Cache the initial list of weapons attached to the gunHolder.
        UpdateWeaponsList();
    }

    void Update()
    {
        // Update the weapons list only when necessary.
        // If your weapons change rarely, call UpdateWeaponsList() only when they are added or removed.
        // UpdateWeaponsList();


        if (joystick != null && stunned == false)
        {
            float horizontal = joystick.Horizontal;
            float vertical = joystick.Vertical;

            Vector3 direction = new Vector3(horizontal, vertical, 0);
            Vector3 shootingDir = direction.normalized;

            if (direction.magnitude >= 0.1f)
            {
                RotateGun(direction);
                UpdateGunPosition(direction);
                FlipGun(direction);
                ShootAllWeapons(shootingDir);
                PositionCrossHair(direction);
            }
            
            

        }
    }

    void PositionCrossHair(Vector3 pos)
    {
        Vector3 newPos = pos * 5;
        crossHair.transform.localPosition = newPos;
    }

    void RotateGun(Vector3 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        gunHolder.rotation = Quaternion.Euler(0, 0, angle);
    }

    void UpdateGunPosition(Vector3 direction)
    {
        gunHolder.position = transform.position + direction * gunOffsetMagnitude;
    }

    void FlipGun(Vector3 direction)
    {
        Vector3 newScale = gunHolder.localScale;
        newScale.y = direction.x < 0 ? -Mathf.Abs(newScale.y) : Mathf.Abs(newScale.y);
        gunHolder.localScale = newScale;
    }

   void ShootAllWeapons(Vector3 direction)
{
    // Remove any null references from the list.
    weapons.RemoveAll(w => w == null);

    foreach (GameObject weapon in weapons)
    {
        weapon.GetComponent<Weapon>().Shoot(direction);
    }
}


    // Call this when your weapons change (e.g., when a new weapon is equipped).
    public void UpdateWeaponsList()
    {
        Debug.Log("Update Weapon List");

        weapons = new List<GameObject>();

        foreach (Weapon weapon in gunHolder.GetComponentsInChildren<Weapon>())
        {
            weapons.Add(weapon.gameObject);
        }
    }
}
