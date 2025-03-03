using UnityEngine;

public class Coin : MonoBehaviour
{
    private PlayerStats playerStats;
    public float speed = 2f; // Adjust the speed value as needed

    void Start()
    {
        playerStats = GameObject.Find("Player").GetComponent<PlayerStats>();
    }

    void Update()
    {
        Vector3 startPosition = transform.position;
        Vector3 endPosition = playerStats.transform.position;
        transform.position = Vector3.MoveTowards(startPosition, endPosition, speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerStats.AddCoins(100);
            FindObjectOfType<AudioManager>().Play("CoinPickUp");
            Destroy(gameObject);
        }
    }
}