using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public class EnemyType
{
    public GameObject enemyPrefab;
    public int cost;
    public float spawnProbability;
}

[System.Serializable]
public class Wave
{
    public int coins;
    public float enemySpeed;
    public float spawnInterval;
    public List<EnemyType> enemyTypes; // Specific enemies for this wave
}

[System.Serializable]
public class WaveParent
{
    public List<Wave> waves = new List<Wave>();

    public WaveParent()
    {
        for (int i = 0; i < 10; i++)
        {
            waves.Add(new Wave());
        }
    }
}

public class EnemySpawner : MonoBehaviour
{
    public WaveParent waveParent = new WaveParent();
    public float waveInterval = 5.0f; // Time between waves in seconds
    public Transform[] spawnPoints;
    public TextMeshProUGUI waveText;
    public float minDistanceFromGem = 5.0f;

    private float timer = 0.0f;
    private float waveTimer = 0.0f;
    private int currentWaveIndex = 0;
    private int currentCoins;
    public bool isWaveActive = false;
    private GameObject gem;
    private List<GameObject> activeEnemies = new List<GameObject>();

    [Header("Shop")]
    public GameObject Shop;
    public GameObject nextWaveButton;

    void Start()
    {
        gem = GameObject.Find("Gem");

        if (waveParent.waves.Count > 0)
        {
            currentCoins = waveParent.waves[currentWaveIndex].coins;
            waveText.text = "Wave: " + (currentWaveIndex + 1);
        }
    }

    void Update()
    {
        if (isWaveActive)
        {
            nextWaveButton.SetActive(false);
            timer += Time.deltaTime;

            if (timer >= waveParent.waves[currentWaveIndex].spawnInterval && currentCoins > 0)
            {
                SpawnEnemy();
                timer = 0.0f;
            }

            if (currentCoins <= 0)
            {
                isWaveActive = false;
                waveTimer = 0.0f;
            }
        }
        else
        {
            waveTimer += Time.deltaTime;

            if (waveTimer >= waveInterval && currentWaveIndex < waveParent.waves.Count - 1 && AllEnemiesDead())
            {
                nextWaveButton.SetActive(true);
            }
        }
    }

    void SpawnEnemy()
    {
        EnemyType selectedEnemy = SelectEnemyType();
        if (selectedEnemy != null && currentCoins >= selectedEnemy.cost)
        {
            Transform spawnPoint = GetValidSpawnPoint();
            if (spawnPoint != null)
            {
                GameObject enemy = Instantiate(selectedEnemy.enemyPrefab, spawnPoint.position, spawnPoint.rotation);
                activeEnemies.Add(enemy);
                currentCoins -= selectedEnemy.cost;
            }
        }
    }

    Transform GetValidSpawnPoint()
    {
        List<Transform> validSpawnPoints = new List<Transform>();

        foreach (Transform spawnPoint in spawnPoints)
        {
            if (Vector3.Distance(spawnPoint.position, gem.transform.position) >= minDistanceFromGem)
            {
                validSpawnPoints.Add(spawnPoint);
            }
        }

        if (validSpawnPoints.Count > 0)
        {
            int spawnIndex = Random.Range(0, validSpawnPoints.Count);
            return validSpawnPoints[spawnIndex];
        }

        return null;
    }

    EnemyType SelectEnemyType()
    {
        List<EnemyType> currentWaveEnemies = waveParent.waves[currentWaveIndex].enemyTypes;
        
        if (currentWaveEnemies == null || currentWaveEnemies.Count == 0) return null;

        float totalProbability = 0f;
        foreach (var enemyType in currentWaveEnemies)
        {
            totalProbability += enemyType.spawnProbability;
        }

        float randomPoint = Random.value * totalProbability;
        foreach (var enemyType in currentWaveEnemies)
        {
            if (randomPoint < enemyType.spawnProbability)
            {
                return enemyType;
            }
            else
            {
                randomPoint -= enemyType.spawnProbability;
            }
        }
        return null;
    }

    public void LoadNextWave()
    {
        currentWaveIndex++;
        currentCoins = waveParent.waves[currentWaveIndex].coins;
        isWaveActive = true;
        waveText.text = "Wave: " + (currentWaveIndex + 1);
    }

    bool AllEnemiesDead()
    {
        activeEnemies.RemoveAll(enemy => enemy == null);
        return activeEnemies.Count == 0;
    }
}
