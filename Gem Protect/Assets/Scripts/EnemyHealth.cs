using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;


public enum enemyType
{
    goose,Miner,PenGuin,other
};
public class EnemyHealth : MonoBehaviour
{
    public GameObject CoinsPrefab;
    public ParticleSystem DeathPartickle;
public ParticleSystem hitPartickle;
    public enemyType enemyType;
    public int maxHealth = 100;
    public int scoreGive = 5;
    [SerializeField] private int currentHealth;

    public GameObject[] powerUps;
    [Range(0,1)]
    public float powerUpSpawnProbabelity;
    private PlayerStats playerStats;
    void Awake()
    {
        playerStats = FindObjectOfType<PlayerStats>();
        currentHealth = maxHealth;
    }



    public void AddHealth(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }

    public void TakeHealth(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        bool particklespawned = false;
        Camera.main.GetComponent<CameraFollow>().TriggerShake(0.1f, 0.09f);

        //Death Effect
        if (particklespawned == false)
        {
            particklespawned = true;
            Instantiate(DeathPartickle, transform.position, quaternion.identity);

        }
        FindObjectOfType<AudioManager>().Play("EnemyDeath");
       
        //Power Ups
        bool spawned = false;
        float spawnChance = Random.Range(0f, 1f);
        if (spawnChance <= powerUpSpawnProbabelity && spawned == false)
        {
            spawned = true;
            if (powerUps.Length > 0)
            {
                // Select a random power-up
                int index = Random.Range(0, powerUps.Length);
                GameObject powerUpPrefab = powerUps[index];

                // Spawn the power-up at the enemy's position
                Instantiate(powerUpPrefab, transform.position, Quaternion.identity);
            }
        }

        //Add Score
        playerStats.AddScore(scoreGive);

        Destroy(gameObject);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Bullet")
        {           
            ParticleSystem hitPartikc=  Instantiate(hitPartickle, transform.position, quaternion.identity);
            Destroy(hitPartikc, 2f);
            Destroy(other.gameObject);
            float damageAmount = other.GetComponent<Bullet>().damage;
            TakeHealth((int)damageAmount);
            FindObjectOfType<AudioManager>().Play("EnemyHit");
        }
    }
 
}
