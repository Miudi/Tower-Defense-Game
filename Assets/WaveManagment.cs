using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Wave
{
    public List<int> enemyTypeCounts = new List<int>();
}

public class WaveManagment : MonoBehaviour
{
    public List<GameObject> enemyType;

    public List<Wave> waves = new List<Wave>();

    public float spawnDelay = 1f;

    private int currentWaveIndex = 0;
    private Transform startSpawnPoint;
    private bool isSpawningWave = false;
    private bool started = false;

    void Start()
    {
        StartCoroutine(FindStartSpawnPoint());
    }

    IEnumerator FindStartSpawnPoint()
    {
        yield return new WaitUntil(() => GameObject.Find("Waypoints/Start") != null);
        startSpawnPoint = GameObject.Find("Waypoints/Start").transform;
        started = true;
    }

    void Update()
    {
        if (started == true)
        {
            if (!isSpawningWave && AllEnemiesDestroyed())
            {

                if (currentWaveIndex < waves.Count)
                {
                    Wave currentWave = waves[currentWaveIndex];
                    StartCoroutine(SpawnEnemiesWithDelay(currentWave));
                }
                else
                {
                    Debug.Log("All waves completed.");
                }

                currentWaveIndex++;
            }
        }
    }

    // Spawn enemies for the specified wave
    IEnumerator SpawnEnemiesWithDelay(Wave wave)
    {
        isSpawningWave = true;

        for (int i = 0; i < wave.enemyTypeCounts.Count; i++) {
            for (int j = 0; j < wave.enemyTypeCounts[i]; j++) {
                GameObject newEnemy = Instantiate(enemyType[i], startSpawnPoint.position, Quaternion.identity);
                yield return new WaitForSeconds(spawnDelay);
            }
        }

        isSpawningWave = false;

    }

    bool AllEnemiesDestroyed()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        return enemies.Length == 0;
    }
}
