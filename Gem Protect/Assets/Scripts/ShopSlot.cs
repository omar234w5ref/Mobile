using UnityEngine;

public enum ItemType
{
    Gun,
    GemPowerUp,PlayerPowerUp,
    SpecialItem
}

[CreateAssetMenu(fileName = "New Shop Slot", menuName = "Shop/Shop Slot")]
public class ShopSlot : ScriptableObject
{
    public string Name;
    public ItemType itemType;
    public GameObject slotPrefab;
    public float spawnProbability; // Probability of this slot being spawned
    public float cost;

    // Gun-specific properties
    public int gunLevel;
    public Sprite itemSprite;
    public GameObject gunPrefab; // Reference to the gun prefab

    [Header("Gun Prefrances")]
    public float damage;
    public float shootingIntervel;

    // Special item-specific properties
    public string specialItemDescription;

}