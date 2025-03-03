using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public ShopSlot shopSlot; 
    public Transform bulletSpawnPoint;
    public float bulletSpeed;
    public abstract void Shoot(Vector3 direction);
}
