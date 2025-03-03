using System.Collections;
using UnityEngine;

public class PenGuin : MonoBehaviour
{
    private GameObject target;
    private Animator anim;
    private float distance;
    [SerializeField] private float minDistance;
    [SerializeField] private float speed;
    private bool isSpawningEnemys = false;
    
    public GameObject smallEnemy;
    private float origspeed;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Gem");
        anim = GetComponent<Animator>();
        StartCoroutine(SpawnEnemiesRandomly());
        origspeed = speed;
    }

    void Update()
    {
        distance = Vector2.Distance(transform.position, target.transform.position);
        
        if (distance > minDistance)
        {
            // Move towards the gem
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
        }
        else
        {
            // Stop spawning when close and explode instead
            
        }

        Animations();
    }

    public void Animations()
    {
        anim.SetBool("EnemySpawn", isSpawningEnemys);
        if (isSpawningEnemys)
        {
            speed = 0;
        }
        else
        {
            speed = origspeed;
        }
    }

    private IEnumerator SpawnEnemiesRandomly()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(2f, 5f)); // Random delay between spawns

            if (distance > minDistance) // Only spawn when still chasing
            {
                isSpawningEnemys = true;
                yield return new WaitForSeconds(.8f);
                SpawnEnemys(smallEnemy, Random.Range(1, 4), 2f,1); // Spawn 1-3 enemies with force
                yield return new WaitForSeconds(.2f); // Allow animation to play
                isSpawningEnemys = false;
            }
        }
    }

    

    public void SpawnEnemys(GameObject enemy, int amount, float force, float duration)
    {
        
        for (int i = 0; i < amount; i++)
        {
            GameObject smallEnemy = Instantiate(enemy, transform.position, Quaternion.identity);
            smallEnemy.GetComponent<Enemy>().enabled = false;
            Rigidbody2D rb = smallEnemy.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // Generate a random direction
                Vector2 randomDirection = Random.insideUnitCircle.normalized;

                // Apply force in the random direction
                rb.AddForce(randomDirection * force, ForceMode2D.Impulse);

                // Start coroutine to stop movement after the given duration
                StartCoroutine(StopEnemyAfterTime(smallEnemy,rb, duration));
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
    }

}
