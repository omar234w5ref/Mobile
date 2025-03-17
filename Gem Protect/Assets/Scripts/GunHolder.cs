using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GunHolder : MonoBehaviour
{
    public GameObject Inhalt;
    public float gunSpacing = 1.0f; // Distance between each gun
    private Shop shop;
    private PlayerStats playerStats;
    public Joystick joystick;
    private void Start()
    {
        shop = FindObjectOfType<Shop>();
    }
    public void EquipGun(ShopSlot gunSlot)
    {
        // Remove the old version of the gun before equipping a new one
        string baseGunName = FindObjectOfType<Shop>().GetBaseGunName(gunSlot.Name);

        List<Transform> oldGuns = new List<Transform>();
        foreach (Transform child in Inhalt.transform)
        {
            ShopSlot childShopSlot = child.GetComponent<MashineGun>()?.shopSlot ?? child.GetComponent<SimpleGun>()?.shopSlot;
            if (childShopSlot != null && FindObjectOfType<Shop>().GetBaseGunName(childShopSlot.Name) == baseGunName)
            {
                oldGuns.Add(child);
            }
        }

        // Destroy the old guns
        foreach (Transform oldGun in oldGuns)
        {
            Debug.Log($"Destroying old gun: {oldGun.name}");
            Destroy(oldGun.gameObject);
        }

        // Now equip the new upgraded gun
        if (Inhalt.transform.childCount - oldGuns.Count < 4) // Ensure we don't exceed the limit
        {
            GameObject newGun = Instantiate(gunSlot.gunPrefab, Inhalt.transform);
            if (newGun.GetComponent<MashineGun>())
            {
                newGun.GetComponent<MashineGun>().shopSlot = gunSlot;
            }
            if (newGun.GetComponent<Pistol>())
            {
                newGun.GetComponent<Pistol>().shopSlot = gunSlot;
            }
            if (newGun.GetComponent<SimpleGun>())
            {
                newGun.GetComponent<SimpleGun>().shopSlot = gunSlot;
            }

            Debug.Log($"Equipped new gun: {newGun.name}");
            PositionGuns();
            FindObjectOfType<PlayerAttack>().UpdateWeaponsList();
        }
        else
        {
            Debug.Log("Cannot equip more than 4 guns.");
        }
    }




    private void PositionGuns()
    {
        int childCount = Inhalt.transform.childCount;
        float offset = (childCount - 1) * gunSpacing / 2.0f; // Calculate the offset to center the guns

        for (int i = 0; i < childCount; i++)
        {
            Transform gunTransform = Inhalt.transform.GetChild(i);
            Transform shootingPoint = gunTransform.Find("ShootingPoint"); // Assuming the shooting point is named "ShootingPoint"

            if (childCount == 1)
            {
                gunTransform.localPosition = Vector3.zero;
            }
            else if (childCount == 2)
            {
                gunTransform.localPosition = new Vector3((i * 2 - 1) * gunSpacing, 0, 0);
            }
            else if (childCount == 3)
            {
                if (i < 2)
                {
                    gunTransform.localPosition = new Vector3((i * 2 - 1) * gunSpacing, gunSpacing, 0);
                }
                else
                {
                    gunTransform.localPosition = new Vector3(0, -gunSpacing, 0);
                }
            }
            else if (childCount == 4)
            {
                gunTransform.localPosition = new Vector3((i % 2 * 2 - 1) * gunSpacing, (i / 2 * 2 - 1) * gunSpacing, 0);
            }

            // Adjust the position to center the guns
            gunTransform.localPosition -= new Vector3(offset, 0, 0);

            // Rotate the gun based on joystick input
            if (joystick != null)
            {
                float horizontal = joystick.Horizontal;
                float vertical = joystick.Vertical;

                Vector3 direction = new Vector3(horizontal, vertical, 0);
                if (direction.magnitude >= 0.1f)
                {
                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    gunTransform.rotation = Quaternion.Euler(0, 0, angle);
                }
            }
        }
    }
   
}