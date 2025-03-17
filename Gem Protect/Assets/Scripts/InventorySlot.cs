using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{

    [Header("GameObjects")]
    public GameObject gunHolder;
    public GameObject inventorySlotImage;
    public GameObject deleteButton;
    [Header("ShopSlot")]
    public ShopSlot shopslot;


    //------------------------------
    private PlayerAttack playerAttack;
    private PlayerStats playerStats;
    
    void Start()
    {
        playerAttack = FindObjectOfType<PlayerAttack>();
        playerStats = FindObjectOfType<PlayerStats>();
    }

    void Update()
    {
        if(shopslot!= null)
        {
            deleteButton.SetActive(true);
        }
        else
        {
            deleteButton.SetActive(false);
        }
    }


    public void RemoveShoplot()
    {
        foreach (Weapon weapon in gunHolder.GetComponentsInChildren<Weapon>())
        {
            if (weapon.shopSlot == shopslot)
            { 
                Destroy(weapon.gameObject);
            }
            else { break; }

        }
        inventorySlotImage.GetComponent<Image>().enabled = false;
        playerStats.RemoveBoughtGun(shopslot);
    }
}
