using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RpgBullet : MonoBehaviour
{

    public GameObject ExplosionPartickle;
    public string explosionSound;
    public ShopSlot shopSlot;
    public CircleCollider2D oldCircleCollider2D;
    public CircleCollider2D newcircleCollider2D;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            
            if (ExplosionPartickle != null) { 

                Instantiate(ExplosionPartickle, transform.position, Quaternion.identity);

            }
            FindObjectOfType<AudioManager>().Play(explosionSound);

            FindObjectOfType<CameraFollow>().TriggerShake(0.2f,.2f);

            collision.gameObject.GetComponent<EnemyHealth>().TakeHealth((int)shopSlot.damage);
            
            Destroy(this.gameObject);
        }
    }
}
