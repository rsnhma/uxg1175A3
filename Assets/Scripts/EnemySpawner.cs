using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    public GameObject ratPrefab;
    public GameObject goblinPrefab;
    public GameObject spiderPrefab;

    [Header("Up to 3 Spawn Points")]
    public Transform spawnPoint1;
    public Transform spawnPoint2;
    public Transform spawnPoint3;

    private List<Transform> spawnPoints = new List<Transform>();

    private void Awake()
    {
        // Add only assigned spawn points to the list
        if (spawnPoint1 != null) spawnPoints.Add(spawnPoint1);
        if (spawnPoint2 != null) spawnPoints.Add(spawnPoint2);
        if (spawnPoint3 != null) spawnPoints.Add(spawnPoint3);

        if (spawnPoints.Count == 0)
        {
            Debug.LogError("No spawn points assigned in EnemySpawner!");
        }
    }

    public void SpawnEnemy(string enemyId)
    {
        if (spawnPoints.Count == 0) return;

        GameObject prefab = null;

        switch (enemyId)
        {
            case "rat": prefab = ratPrefab; break;
            case "goblin": prefab = goblinPrefab; break;
            case "spider": prefab = spiderPrefab; break;
        }

        if (prefab == null)
        {
            Debug.LogWarning("Enemy prefab for ID " + enemyId + " is null!");
            return;
        }

        // Pick a random spawn point from available ones
        Transform chosenPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];

        GameObject enemy = Instantiate(prefab, chosenPoint.position, Quaternion.identity);

        // Initialize with data
        EnemyData data = EnemyDatabase.GetEnemyById(enemyId);
        enemy.GetComponent<EnemyBehaviour>().Initialize(data);
    }
}
