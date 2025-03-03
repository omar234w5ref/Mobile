using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ShopSlot))]
public class ShopSlotEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ShopSlot shopSlot = (ShopSlot)target;
        shopSlot.Name = EditorGUILayout.TextField("Name", shopSlot.Name);
        shopSlot.itemType = (ItemType)EditorGUILayout.EnumPopup("Item Type", shopSlot.itemType);
        shopSlot.slotPrefab = (GameObject)EditorGUILayout.ObjectField("Slot Prefab", shopSlot.slotPrefab, typeof(GameObject), false);
        shopSlot.spawnProbability = EditorGUILayout.Slider("Spawn Probability", shopSlot.spawnProbability, 0f, 1f);
        EditorGUILayout.Space();

        if (shopSlot.itemType == ItemType.Gun)
        {
            shopSlot.gunLevel = EditorGUILayout.IntField("Gun Level", shopSlot.gunLevel);
            shopSlot.itemSprite = (Sprite)EditorGUILayout.ObjectField("Gun Sprite", shopSlot.itemSprite, typeof(Sprite), false);
            shopSlot.gunPrefab = (GameObject)EditorGUILayout.ObjectField("Gun Prefab", shopSlot.gunPrefab, typeof(GameObject), false);
            shopSlot.damage = (float)EditorGUILayout.FloatField("Damage", shopSlot.damage);
            shopSlot.shootingIntervel = (float)EditorGUILayout.FloatField("Shooting Interval", shopSlot.shootingIntervel);           
        }
        else if (shopSlot.itemType == ItemType.GemPowerUp || shopSlot.itemType == ItemType.PlayerPowerUp)
        {
            shopSlot.itemSprite = shopSlot.itemSprite = (Sprite)EditorGUILayout.ObjectField("PowerUp Sprite", shopSlot.itemSprite, typeof(Sprite), false);
            shopSlot.specialItemDescription = EditorGUILayout.TextField("Special Item Description", shopSlot.specialItemDescription);
        }

        EditorUtility.SetDirty(shopSlot);
    }
}