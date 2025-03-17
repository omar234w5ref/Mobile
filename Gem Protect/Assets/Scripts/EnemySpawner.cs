using System.Collections;
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
    public float spawnInterval;
    public List<EnemyType> enemyTypes; // Specific enemies for this wave
}

[System.Serializable]
public class Phase
{
    public string phaseName;
    public List<Wave> waves = new List<Wave>();
}

[System.Serializable]
public class WaveParent
{
    public List<Phase> phases = new List<Phase>();

    public WaveParent()
    {
        for (int i = 0; i < 2; i++) // Example: 2 Phases
        {
            Phase phase = new Phase { phaseName = "Phase " + (i + 1) };
            for (int j = 0; j < 5; j++) // Example: 5 Waves per Phase
            {
                phase.waves.Add(new Wave());
            }
            phases.Add(phase);
        }
    }
}

public class EnemySpawner : MonoBehaviour
{
    public bool shopOpened = false;
    public bool isMainShopUnlocked = false;
    private bool canOpenShop = false;

    public WaveParent waveParent = new WaveParent();
    public float waveInterval = 5.0f;
    public Transform[] spawnPoints;
    public TextMeshProUGUI waveText;
    public float minDistanceFromGem = 5.0f;

    private float timer = 0.0f;
    private float waveTimer = 0.0f;
    private int currentPhaseIndex = 0;
    private int currentWaveIndex = 0;
    private int currentCoins;
    public bool isWaveActive = false;
    private GameObject player;
    private List<GameObject> activeEnemies = new List<GameObject>();

    [Header("Shop")]
    public GameObject Shop;
    public GameObject nextWaveButton;

    [Header("Spawn Indicator")]
    public GameObject spawnIndicatorPrefab; // ✅ Drag and drop your indicator prefab here
    public float indicatorDuration = 1.5f; // ✅ Time before enemy spawns after indicator appears

    bool gameStarted = false;

    void Start()
    {
        player = GameObject.Find("Player");

        if (waveParent.phases.Count > 0 && waveParent.phases[0].waves.Count > 0)
        {
            currentCoins = waveParent.phases[currentPhaseIndex].waves[currentWaveIndex].coins;
            waveText.text = " Wave: " + (currentWaveIndex +1);
        }
    }

    void Update()
    {
        if (isWaveActive && gameStarted)
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            playerHealth.waveOver = false;

            nextWaveButton.SetActive(false);
            timer += Time.deltaTime;

            if (timer >= waveParent.phases[currentPhaseIndex].waves[currentWaveIndex].spawnInterval && currentCoins > 0)
            {
                StartCoroutine(ShowSpawnIndicator()); // ✅ Show spawn indicator before spawning
                timer = 0.0f;
            }

            if (currentCoins <= 0)
            {
                isWaveActive = false;
                waveTimer = 0.0f;
            }
        }
        else if(!isWaveActive)
        {
            waveTimer += Time.deltaTime;

            if (waveTimer >= waveInterval && AllEnemiesDead() && gameStarted && !shopOpened)
            {
                PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
                playerHealth.waveOver = true;

                shopOpened = true;
                canOpenShop = true;

                if (isMainShopUnlocked)
                {
                    Shop shop = FindObjectOfType<Shop>();
                    if (shop != null)
                    {
                        Debug.Log("🛒 Main Shop Opened");
                        shop.OpenShop();
                    }
                }

                nextWaveButton.SetActive(true);
            }
        }
    }

    public void LoadNextWave()
    {
        currentWaveIndex++;
        if (currentWaveIndex >= waveParent.phases[currentPhaseIndex].waves.Count)
        {
            currentWaveIndex = 0;
            currentPhaseIndex++;
        }

        currentCoins = waveParent.phases[currentPhaseIndex].waves[currentWaveIndex].coins;
        isWaveActive = true;
        shopOpened = false;
        canOpenShop = false;
        waveText.text = " Wave: " + (currentWaveIndex + 1);
    }

    IEnumerator ShowSpawnIndicator()
    {
        Transform spawnPoint = GetValidSpawnPoint();
        if (spawnPoint == null) yield break;

        GameObject indicator = Instantiate(spawnIndicatorPrefab, spawnPoint.position, Quaternion.identity);
        indicator.SetActive(true);

        yield return new WaitForSeconds(indicatorDuration); // ✅ Wait before spawning

        Destroy(indicator);
        SpawnEnemy(spawnPoint);
    }

    void SpawnEnemy(Transform spawnPoint)
    {
        EnemyType selectedEnemy = SelectEnemyType();
        if (selectedEnemy != null && currentCoins >= selectedEnemy.cost)
        {
            GameObject enemy = Instantiate(selectedEnemy.enemyPrefab, spawnPoint.position, spawnPoint.rotation);
            activeEnemies.Add(enemy);
            currentCoins -= selectedEnemy.cost;
        }
    }

    Transform GetValidSpawnPoint()
    {
        List<Transform> validSpawnPoints = new List<Transform>();

        foreach (Transform spawnPoint in spawnPoints)
        {
            if (Vector3.Distance(spawnPoint.position, player.transform.position) >= minDistanceFromGem)
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
        List<EnemyType> currentWaveEnemies = waveParent.phases[currentPhaseIndex].waves[currentWaveIndex].enemyTypes;

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

    public void UnlockMainShop()
    {
        isMainShopUnlocked = true;
    }

    public void GameStart()
    {
        gameStarted = true;
        isWaveActive = true;
    }

    bool AllEnemiesDead()
    {
        activeEnemies.RemoveAll(enemy => enemy == null);
        return activeEnemies.Count == 0;
    }
}
