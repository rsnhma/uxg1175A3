using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject ratPrefab;
    public GameObject goblinPrefab;
    public GameObject spiderPrefab;

    public Transform spawnPoint;
    //public Vector2 spawnAreaMin = new Vector2(-8, -4);
    //public Vector2 spawnAreaMax = new Vector2(8, 4);

    public void SpawnEnemy(string enemyId)
    {
        GameObject prefab = null;

        switch (enemyId)
        {
            case "rat": prefab = ratPrefab; break;
            case "goblin": prefab = goblinPrefab; break;
            case "spider": prefab = spiderPrefab; break;
        }

        if (prefab == null) return;

        // Always spawn at your chosen spawn point's position
        GameObject enemy = Instantiate(prefab, spawnPoint.position, Quaternion.identity);

        // Initialize from JSON data
        EnemyData data = EnemyDatabase.GetEnemyById(enemyId);
        enemy.GetComponent<EnemyBehaviour>().Initialize(data);
    }
}
