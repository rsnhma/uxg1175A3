using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject ratPrefab;
    public GameObject goblinPrefab;
    public GameObject dogPrefab;
    public Vector2 spawnAreaMin = new Vector2(-8, -4);
    public Vector2 spawnAreaMax = new Vector2(8, 4);

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

        Vector2 spawnPos = new Vector2( Random.Range(spawnAreaMin.x, spawnAreaMax.x),Random.Range(spawnAreaMin.y, spawnAreaMax.y));
        GameObject enemy = Instantiate(prefab, spawnPos, Quaternion.identity);

        // Initialize from JSON data
        EnemyData data = EnemyDatabase.GetEnemyById(enemyId);
        enemy.GetComponent<EnemyBehaviour>().Initialize(data);
    }
}
