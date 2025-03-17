using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    private PlayerStats playerStats;
    public Image slotImage;
    public ShopSlot shopSlot;
    private bool isBought = false;
    private GunHolder gunHolder;
    private Shop shop;
    private System.Action buyAction;
    private GemPowerUps Gem;
    private PlayerPowerUps playerPowerUps;

    [SerializeField] private GameObject Left;
    [SerializeField] private GameObject Middle;
    [SerializeField] private GameObject Right;
    void Start()
    {
        Gem = FindObjectOfType<GemPowerUps>();
        playerPowerUps = FindObjectOfType<PlayerPowerUps>();
        shop = FindObjectOfType<Shop>();
        playerStats = GameObject.Find("Player").GetComponent<PlayerStats>();
        gunHolder = GameObject.Find("GunHolder").GetComponent<GunHolder>();

        if(shopSlot != null)
        {
            slotImage.sprite = shopSlot.itemSprite;
            slotImage.SetNativeSize();
        }

        if (shopSlot.gunLevel == 1)
        {
            Left.SetActive(true);
            Right.SetActive(false);
            Middle.SetActive(false);
        }
        if (shopSlot.gunLevel == 2)
        {
            Left.SetActive(true);
            Right.SetActive(true);
            Middle.SetActive(false);
        }

        if (shopSlot.gunLevel == 3)
        {
            Right.SetActive(true);
            Left.SetActive(true);
            Middle.SetActive(true);
        }

    }


    public void SetBuyAction(System.Action action)
    {
        buyAction = action;
    }


    public void Buy()
    {
        if (isBought) // Prevent duplicate purchases
        {
            Debug.Log("Slot already bought!");
            return;
        }

        isBought = true; // Mark as bought immediately to prevent double purchases
        FindAnyObjectByType<AudioManager>().Play("BuySfx");
        if (shopSlot.itemType == ItemType.Gun)
        {
            shop.ShowBoughtItems();

            string baseGunName = shop.GetBaseGunName(shopSlot.Name);
            ShopSlot oldGun = playerStats.boughtGuns.FirstOrDefault(gun => shop.GetBaseGunName(gun.Name) == baseGunName);
            if (oldGun != null)
            {
                playerStats.boughtGuns.Remove(oldGun);
                Debug.Log("Old gun removed from PlayerStats: " + oldGun.Name);
            }

            // Add new gun to PlayerStats
            playerStats.TakeCoins((int)shopSlot.cost);
            playerStats.AddBoughtGun(shopSlot);
            shop.ShowBoughtItems();

            // Equip the new gun
            gunHolder.EquipGun(shopSlot);

            // Ensure weapons list in PlayerAttack is updated
            PlayerAttack playerAttack = FindObjectOfType<PlayerAttack>();
            if (playerAttack != null)
            {
                playerAttack.UpdateWeaponsList(); // Refresh the weapon list
                shop.ShowBoughtItems();
            }
        }

        if (shopSlot.itemType == ItemType.PlayerPowerUp)
        {
            playerPowerUps.powerUpSlots.Add(shopSlot);
        }

        // Disable button to prevent multiple clicks
        GetComponent<Button>().interactable = false;
    }




}