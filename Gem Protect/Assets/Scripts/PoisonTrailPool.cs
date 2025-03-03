using System.Collections.Generic;
using UnityEngine;

public class PoisonTrailPool : MonoBehaviour
{
    public static PoisonTrailPool Instance;
    public GameObject poisonTrailPrefab;
    public int poolSize = 5;

    private Queue<GameObject> pool = new Queue<GameObject>();

    void Awake()
    {
        Instance = this;
        FillPool();
    }

    void FillPool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(poisonTrailPrefab);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    public GameObject GetPoisonTrail()
    {
        if (pool.Count == 0) FillPool(); // Expand if needed

        GameObject trail = pool.Dequeue();
        trail.SetActive(true);
        return trail;
    }

    public void ReturnPoisonTrail(GameObject trail)
    {
        trail.SetActive(false);
        pool.Enqueue(trail);
    }
}
