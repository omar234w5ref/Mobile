using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 10f;
    public Joystick joystick;
    private SpriteRenderer spriteRenderer;
    private Animator anim;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (joystick != null)
        {
            float horizontal = joystick.Horizontal;
            float vertical = joystick.Vertical;

            Vector3 direction = new Vector3(horizontal, vertical, 0).normalized;

            if (direction.magnitude >= 0.1f)
            {
                anim.SetBool("Walk", true);
                float adjustedSpeed = speed * direction.magnitude; // Adjust speed based on joystick distance from center
                transform.Translate(direction * adjustedSpeed * Time.deltaTime, Space.World);
            }
            else
            {
                anim.SetBool("Walk", false);
                float closeToCenterSpeed = speed * 0.2f; // Set a different speed if the handle is close to the center
                transform.Translate(direction * closeToCenterSpeed * Time.deltaTime, Space.World);
            }



            spriteRenderer.flipX = horizontal < 0;
           
        }
    }

    public void Flip()
    {
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "EnemyBullet")
        {
            Destroy(collision.gameObject);
        }
    }
}
