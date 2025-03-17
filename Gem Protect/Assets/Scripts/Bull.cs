using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bull : MonoBehaviour
{
    private GameObject Gem;
    private GameObject player;
    public float speed = 5f;
    public float stunnduration;
    private SpriteRenderer spriteRenderer;

    private Animator anim;
    void Start()
    {
        player = GameObject.Find("Player");
        Gem = GameObject.Find("Gem");
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        Vector3 direction = player.transform.position - transform.position;
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        if (anim != null)
            anim.SetBool("Walking", true);
        // Flip the sprite based on the direction of movement
        spriteRenderer.flipX = direction.x < 0;
        
    }



    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //Stunn the player
            player.GetComponent<PlayerMovement>().stunned = true;
            player.GetComponent<PlayerAttack>().stunned = true;
            StartCoroutine(stunnTime(1));
            //Delete enemy
            Die();

        }
    }

    private IEnumerator stunnTime(float duration)
    {
        Debug.Log("Stunning");
        yield return new WaitForSeconds(1);
        Debug.Log("Waited" + 1);
        player.GetComponent<PlayerMovement>().stunned = false;
        player.GetComponent<PlayerAttack>().stunned = false;
        Destroy(this.gameObject);
    }


    public void Die()
    {
        this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        speed = 0;
    }
}
