using UnityEngine;

public class Enemy : MonoBehaviour
{
    private GameObject Gem;
    private GameObject player;
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
            Destroy(this.gameObject);
        }
    }


}