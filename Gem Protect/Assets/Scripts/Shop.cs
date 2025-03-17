using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public GameObject ShopPanel;
    public GameObject Inhalt;
    public ShopSlot[] shopSlots;
    public GameObject[] inventoryGunSlot;
    public int maxSlots = 4;
    private PlayerStats playerStats;
    private PlayerAttack playerAttack;
    private EnemySpawner enemySpawner;
    public bool panelClose;
    private Dictionary<string, int> playerGunLevels = new Dictionary<string, int>(); // Tracks highest level owned per gun base name
    private bool normalShop = false;
    void Start()
    {
        enemySpawner = FindObjectOfType<EnemySpawner>();
        playerAttack = FindAnyObjectByType<PlayerAttack>();
        playerStats = FindAnyObjectByType<PlayerStats>();

        if (playerStats == null)
            Debug.LogError("PlayerStats component not found on Player!");

        // 🔥 Ensure we recognize any guns given at the start of the game!
        RefreshPlayerGunLevels();

    }

    

    public void OpenShop()
    {
        // ✅ Always refresh gun levels before updating shop items
        RefreshPlayerGunLevels();

        // ✅ Clear previous shop slots
        ClearShopSlots();

        // ✅ Spawn new shop slots (new available items)
        SpawnShopSlots();

        // ✅ Make sure the shop panel is visible
       
        
        ShopPanel.SetActive(true);

        

        Debug.Log("🛒 Shop Opened: Slots Refreshed");
    }


    void ClearShopSlots()
    {
        foreach (Transform child in Inhalt.transform)
        {
            Destroy(child.gameObject);
        }

        FindAnyObjectByType<AudioManager>().Play("ButtonClick");
    }

    public void CloseShop()
    {
        panelClose = true;
        ShopPanel.SetActive(false);

        // ✅ Clear slots so they refresh on next open
        ClearShopSlots();
    }


    /// <summary>
    /// 🔥 Ensures playerGunLevels includes ALL owned guns (both from PlayerStats & PlayerAttack)
    /// </summary>
    public void RefreshPlayerGunLevels()
    {
        playerGunLevels.Clear();

        // 🔹 Step 1: Check guns from PlayerStats (Bought Guns)
        foreach (ShopSlot ownedGun in playerStats.boughtGuns)
        {
            string baseGunName = GetBaseGunName(ownedGun.Name);

            if (!playerGunLevels.ContainsKey(baseGunName) || playerGunLevels[baseGunName] < ownedGun.gunLevel)
            {
                playerGunLevels[baseGunName] = ownedGun.gunLevel;
            }
        }

        // 🔹 Step 2: Check guns from PlayerAttack (Current Weapons Equipped)
        foreach (GameObject weaponObj in playerAttack.weapons)
        {
            Weapon weaponComponent = weaponObj.GetComponent<Weapon>();

            if (weaponComponent != null)
            {
                string baseGunName = GetBaseGunName(weaponComponent.shopSlot.Name);
                int weaponLevel = weaponComponent.shopSlot.gunLevel;

                if (!playerGunLevels.ContainsKey(baseGunName) || playerGunLevels[baseGunName] < weaponLevel)
                {
                    playerGunLevels[baseGunName] = weaponLevel;
                }
            }
        }

        Debug.Log("🔄 PlayerGunLevels Updated: " + string.Join(", ", playerGunLevels.Select(kv => $"{kv.Key}: {kv.Value}")));
    }

    void SpawnShopSlots()
    {
        List<ShopSlot> shuffledSlots = shopSlots.OrderBy(s => UnityEngine.Random.value).ToList();

        foreach (ShopSlot shopSlot in shuffledSlots)
        {
            if (Inhalt.transform.childCount >= maxSlots)
                break;

            string baseGunName = GetBaseGunName(shopSlot.Name);
            int requiredLevel = playerGunLevels.ContainsKey(baseGunName) ? playerGunLevels[baseGunName] + 1 : 1;

            if (shopSlot.itemType == ItemType.Gun && shopSlot.gunLevel == requiredLevel)
            {
                InstantiateSlot(shopSlot, () => BuyGun(shopSlot));
            }
            else if (shopSlot.itemType == ItemType.GemPowerUp || shopSlot.itemType == ItemType.PlayerPowerUp)
            {
                InstantiateSlot(shopSlot, () => BuyPowerUp(shopSlot));
            }
        }
    }

    void InstantiateSlot(ShopSlot shopSlot, Action buyAction)
    {
        GameObject slot = Instantiate(shopSlot.slotPrefab, Inhalt.transform);
        Slot slotComponent = slot.GetComponent<Slot>();
        if (slotComponent != null)
        {
            slotComponent.shopSlot = shopSlot;
            slotComponent.SetBuyAction(buyAction);
        }
    }

    public void BuyGun(ShopSlot newGun)
    {
        if (playerAttack == null || playerStats == null)
        {
            Debug.LogError("PlayerAttack or PlayerStats reference is missing!");
            return;
        }

        string baseGunName = GetBaseGunName(newGun.Name);

        // Remove the old gun (if one exists)
        if (playerGunLevels.ContainsKey(baseGunName))
        {
            Debug.Log($"Removing old gun with base name: {baseGunName}");
            RemoveOldGun(baseGunName);
        }

        // Update the highest level gun owned for that type
        playerGunLevels[baseGunName] = newGun.gunLevel;

        // Add the new gun to PlayerStats and instantiate it
        playerStats.boughtGuns.Add(newGun);
        GameObject newGunObj = Instantiate(newGun.gunPrefab, playerAttack.gunHolder);
        playerAttack.weapons.Add(newGunObj);

        Debug.Log($"Bought new gun: {newGun.Name} (Level {newGun.gunLevel})");

        // 🔄 Refresh gun levels so shop correctly updates
        RefreshPlayerGunLevels();

        // Show bought items in the shop
        ShowBoughtItems();

        // Equip the new gun
        FindObjectOfType<GunHolder>().EquipGun(newGun);

        // Ensure weapons list in PlayerAttack is updated
        playerAttack.UpdateWeaponsList();

        // Play sound effect
        FindAnyObjectByType<AudioManager>().Play("BuySfx");
    }

    private void RemoveOldGun(string baseGunName)
    {
        GameObject oldGunObject = playerAttack.weapons.FirstOrDefault(w =>
        {
            Weapon weaponComponent = w.GetComponent<Weapon>();
            return weaponComponent != null && GetBaseGunName(weaponComponent.shopSlot.Name) == baseGunName;
        });

        if (oldGunObject != null)
        {
            Debug.Log($"Removing old gun: {oldGunObject.name}");
            playerAttack.weapons.Remove(oldGunObject);
            Destroy(oldGunObject);
        }
        else
        {
            Debug.Log($"No old gun found with base name: {baseGunName}");
        }

        // Also remove from PlayerStats
        playerStats.boughtGuns.RemoveAll(gun => GetBaseGunName(gun.Name) == baseGunName);
    }

    public void BuyPowerUp(ShopSlot powerUpSlot)
    {
        Debug.Log("Bought powerup: " + powerUpSlot.Name);
    }

    public string GetBaseGunName(string gunName)
    {
        string[] parts = gunName.Split(' ');
        return parts.Length > 1 && int.TryParse(parts[^1], out _) ? string.Join(" ", parts.Take(parts.Length - 1)) : gunName;
    }

    public void ShowBoughtItems()
    {
        int slotIndex = 0;
        foreach (ShopSlot boughtGun in playerStats.boughtGuns)
        {
            if (slotIndex >= inventoryGunSlot.Length)
                break;


            
            Image inventorySlotImage = inventoryGunSlot[slotIndex].GetComponent<Image>();


            inventorySlotImage.enabled = true;
            inventorySlotImage.sprite = boughtGun.itemSprite;
            inventorySlotImage.SetNativeSize();
            InventorySlot inventorySlot = inventoryGunSlot[slotIndex].GetComponentInParent<InventorySlot>();
            inventorySlot.shopslot = boughtGun;
            slotIndex++;






        }
    }

    
        
    
}
