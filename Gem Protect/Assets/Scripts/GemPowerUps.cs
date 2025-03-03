using System.Collections.Generic;
using UnityEngine;

public class GemPowerUps : MonoBehaviour
{
    public List<ShopSlot> powerUpSlots = new List<ShopSlot>();

    [Header("ShieldPowerUp")]
    [SerializeField] private float rotationSpeed = 90f; // degrees per second
    [SerializeField] private float shieldOffset = 1f;     
    [SerializeField] private GameObject shieldPrefab;

    private GameObject shieldInstance;
    private float orbitAngle = 0f;

    void Update()
    {
        bool hasGemShield = false;
        foreach (ShopSlot shopSlot in powerUpSlots)
        {
            if (shopSlot.Name == "GemShield")
            {
                hasGemShield = true;
                break;
            }
        }

        if (hasGemShield)
        {
            if (shieldInstance == null)
            {
                orbitAngle = 0f; // Reset orbit angle when shield is created.
                Vector3 spawnPosition = transform.position + new Vector3(shieldOffset, 0f, 0f);
                shieldInstance = Instantiate(shieldPrefab, spawnPosition, Quaternion.identity);
            }
            else
            {
                orbitAngle += rotationSpeed * Time.deltaTime;

                float radians = orbitAngle * Mathf.Deg2Rad;
                Vector3 offset = new Vector3(Mathf.Cos(radians), Mathf.Sin(radians), 0) * shieldOffset;

                shieldInstance.transform.position = transform.position + offset;
                shieldInstance.transform.rotation = Quaternion.identity;
            }
        }
        else
        {
            if (shieldInstance != null)
            {
                Destroy(shieldInstance);
                shieldInstance = null;
            }
        }
    }
}
