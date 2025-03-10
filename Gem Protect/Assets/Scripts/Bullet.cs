using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public ShopSlot shopSlot;
    public float damage;


    void Start()
    {
        if (shopSlot != null)
            damage = shopSlot.damage;


        Destroy(this.gameObject, 5);
    }
    
    

}
