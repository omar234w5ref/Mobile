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
            ShopSlot childShopSlot = child.GetComponent<MashineGun>()?.shopSlot ?? child.GetComponent<Pistol>()?.shopSlot;
            if (childShopSlot != null && FindObjectOfType<Shop>().GetBaseGunName(childShopSlot.Name) == baseGunName)
            {
                oldGuns.Add(child);
            }
        }

        // Destroy the old guns
        foreach (Transform oldGun in oldGuns)
        {
            Destroy(oldGun.gameObject);
        }

        // Now equip the new upgraded gun
        if (Inhalt.transform.childCount < 4) // Ensure we don't exceed the limit
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
        }
    }
   
}