using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RockyEnemy : MonoBehaviour
{
    private GameObject Gem;
    private GameObject player;
    public GameObject smallRockEnemy;
    public float speed = 5f;
    private SpriteRenderer spriteRenderer;
    public GameObject CoinsPrefab;

    private Animator anim;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
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
            collision.gameObject.GetComponent<PlayerHealth>().TakeHealth(10);
             SpawnEnemys(smallRockEnemy, Random.Range(2, 4), 3f, 1); // Spawn 1-3 enemies with force
            Death();
        }
    }


    public void Death()
    {
       this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        Destroy(this.gameObject, 1.5f);
    }


    public void SpawnEnemys(GameObject enemy, int amount, float force, float duration)
    {

        for (int i = 0; i < amount; i++)
        {
            GameObject smallEnemy = Instantiate(enemy, transform.position, Quaternion.identity);
            smallEnemy.GetComponent<Enemy>().enabled = false;
            smallEnemy.GetComponent<BoxCollider2D>().enabled = false;
            Rigidbody2D rb = smallEnemy.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // Generate a random direction
                Vector2 randomDirection = Random.insideUnitCircle.normalized;

                // Apply force in the random direction
                rb.AddForce(randomDirection * force, ForceMode2D.Impulse);

                // Start coroutine to stop movement after the given duration
                StartCoroutine(StopEnemyAfterTime(smallEnemy, rb, duration));
            }

        }
    }

    private IEnumerator StopEnemyAfterTime(GameObject enemy, Rigidbody2D rb, float duration)
    {
        yield return new WaitForSeconds(duration);

        // Stop movement by setting velocity to zero
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        enemy.GetComponent<Enemy>().enabled = true;
        enemy.GetComponent<BoxCollider2D>().enabled = false;

    }

}
