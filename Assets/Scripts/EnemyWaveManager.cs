using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaveManager : MonoBehaviour
{
    public static EnemyWaveManager Instance;

    public EnemySpawner spawner;

    private List<LevelWaveData> allWaves;
    private List<WaveData> currentLevelWaves;

    private int currentWaveIndex = 1;
    private int enemiesRemaining = 0;

    private void Awake()
    {
        Instance = this;
        LoadWaveData();
    }

    void LoadWaveData()
    {
        TextAsset json = Resources.Load<TextAsset>("enemywaves");
        string fixedJson = "{\"levelWaves\":" + json.text + "}"; // Wrap as an object
        allWaves = JsonUtility.FromJson<LevelWaveList>(fixedJson).levelWaves;
    }

    public void StartLevel(int level)
    {
        currentWaveIndex = 1;
        enemiesRemaining = 0;
        currentLevelWaves = allWaves.Find(l => l.level == level).waves;
        StartCoroutine(SpawnNextWave());
    }

    IEnumerator SpawnNextWave()
    {
        if (currentWaveIndex >= currentLevelWaves.Count)
        {
            Debug.Log("All waves cleared for this level!");
            // Spawn keycard here if needed
            yield break;
        }

        WaveData wave = currentLevelWaves[currentWaveIndex];
        currentWaveIndex++;

        int spawnCount = wave.count > 0 ? wave.count : Random.Range(wave.countMin, wave.countMax + 1);
        enemiesRemaining = spawnCount;

        for (int i = 0; i < spawnCount; i++)
        {
            spawner.SpawnEnemy(wave.enemyId);
            yield return new WaitForSeconds(0.4f);
        }

        // Goblin/dog: wait until 1/3 HP before next wave
        if (wave.enemyId == "goblin" || wave.enemyId == "spider")
        {
            while (!CheckEnemyLowHealth(wave.enemyId, 0.33f))
            {
                yield return null;
            }
        }

        // Wait for enemies to die before next wave
        while (enemiesRemaining > 0)
        {
            yield return null;
        }

        StartCoroutine(SpawnNextWave());
    }

    public void NotifyEnemyDefeated()
    {
        enemiesRemaining--;
    }

    bool CheckEnemyLowHealth(string enemyId, float threshold)
    {
        EnemyBehaviour[] allEnemies = FindObjectsOfType<EnemyBehaviour>();
        foreach (EnemyBehaviour e in allEnemies)
        {
            if (e.enemyId == enemyId && e.health <= e.maxHealth * threshold)
            {
                return true;
            }
        }
        return false;
    }
}
