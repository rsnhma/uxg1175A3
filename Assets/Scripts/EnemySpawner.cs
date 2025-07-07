using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject ratPrefab;
    public GameObject goblinPrefab;
    public GameObject dogPrefab;
    public Transform[] spawnPoints;

    public void SpawnEnemy(string enemyId)
    {
        GameObject prefab = null;

        switch (enemyId)
        {
            case "rat": prefab = ratPrefab; break;
            case "goblin": prefab = goblinPrefab; break;
            case "dog": prefab = dogPrefab; break;
        }

        if (prefab == null) return;

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject enemy = Instantiate(prefab, spawnPoint.position, Quaternion.identity);

        // Initialize from JSON data
        EnemyData data = EnemyDatabase.GetEnemyById(enemyId);
        enemy.GetComponent<EnemyBehaviour>().Initialize(data);
    }
}
