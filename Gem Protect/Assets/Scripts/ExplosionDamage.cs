using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionDamage : MonoBehaviour
{
    public int damage;

    private void Start()
    {
        Destroy(this.gameObject, 1f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       
            foreach (Transform child in collision.transform)
            {
                if (child.gameObject.tag == "Enemy")
                {
                    child.gameObject.GetComponent<EnemyHealth>().TakeHealth(damage);
                }
            }
        
        
    }
}
