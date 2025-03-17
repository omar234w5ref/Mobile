using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BazookaBullet : MonoBehaviour
{

    public GameObject ExplosionPartickle;
    public string explosionSound;
    public ShopSlot shopSlot;
    public CircleCollider2D oldCircleCollider2D;
    public CircleCollider2D newcircleCollider2D;
    public GameObject damageRange;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            
            if (ExplosionPartickle != null) { 

                Instantiate(ExplosionPartickle, transform.position, Quaternion.identity);

            }


            if (damageRange != null)
            {
                Instantiate(damageRange, transform.position, Quaternion.identity);

            }
            FindObjectOfType<AudioManager>().Play(explosionSound);

            FindObjectOfType<CameraFollow>().TriggerShake(0.2f,.2f);

            collision.gameObject.GetComponent<EnemyHealth>().TakeHealth((int)shopSlot.damage);
            
            Destroy(this.gameObject);
        }
        if(collision.gameObject.tag == "Border")
        {
            if (ExplosionPartickle != null)
            {

                Instantiate(ExplosionPartickle, transform.position, Quaternion.identity);

            }

            if(damageRange != null)
            {
               Instantiate(damageRange, transform.position, Quaternion.identity);

            }
            FindObjectOfType<AudioManager>().Play(explosionSound);

            FindObjectOfType<CameraFollow>().TriggerShake(0.2f, .2f);

            
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Border")
        {
            if (ExplosionPartickle != null)
            {

                Instantiate(ExplosionPartickle, transform.position, Quaternion.identity);

            }
            FindObjectOfType<AudioManager>().Play(explosionSound);

            FindObjectOfType<CameraFollow>().TriggerShake(0.2f, .2f);

            collision.gameObject.GetComponent<EnemyHealth>().TakeHealth((int)shopSlot.damage);

            Destroy(this.gameObject);
        }
    }
}
