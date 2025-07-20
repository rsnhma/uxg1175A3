using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaveManager : MonoBehaviour
{
    public static EnemyWaveManager Instance;

    public EnemySpawner spawner;

    private List<LevelWaveData> allWaves;
    private List<WaveData> currentLevelWaves;

    private int currentWaveIndex = 0;
    private int enemiesRemaining;

    public GameObject keycardPrefab;
    //public Transform keycardSpawnPoint;

    private void Awake()
    {
        Instance = this;
        LoadWaveData();
    }

    private void Start()
    {
        StartLevel(1);
    }

    void LoadWaveData()
    {
        TextAsset json = Resources.Load<TextAsset>("enemywaves");
        if (json == null)
        {
            Debug.LogError("enemywaves.json not found in Resources folder!");
            return;
        }

        string fixedJson = "{\"levelWaves\":" + json.text + "}";
        allWaves = JsonUtility.FromJson<LevelWaveList>(fixedJson).levelWaves;
    }

    public void StartLevel(int level)
    {
        LevelWaveData levelData = allWaves.Find(l => l.level == level);
        if (levelData == null)
        {
            Debug.LogError("No wave data found for level " + level);
            return;
        }

        currentWaveIndex = 0;
        enemiesRemaining = 0;
        currentLevelWaves = levelData.waves;

        Debug.Log("Starting level " + level + " with " + currentLevelWaves.Count + " waves.");
        StartCoroutine(SpawnNextWave());
    }

    IEnumerator SpawnNextWave()
    {
        if (currentWaveIndex >= currentLevelWaves.Count)
        {
            Debug.Log("All waves cleared for this level!");
            //SpawnKeycard();
            yield break;
        }

        WaveData wave = currentLevelWaves[currentWaveIndex];
        currentWaveIndex++;

        int spawnCount = wave.count > 0 ? wave.count : Random.Range(wave.countMin, wave.countMax + 1);
        enemiesRemaining = spawnCount;

        Debug.Log($"Spawning Wave {wave.waveNumber} ({spawnCount} x {wave.enemyId})");

        for (int i = 0; i < spawnCount; i++)
        {
            spawner.SpawnEnemy(wave.enemyId);
            yield return new WaitForSeconds(0.4f);
        }

        if (wave.enemyId == "goblin" || wave.enemyId == "spider")
        {
            while (!CheckEnemyLowHealth(wave.enemyId, 0.33f))
            {
                yield return null;
            }
            Debug.Log($"{wave.enemyId} reached low health. Continuing.");
        }

        while (enemiesRemaining > 0)
        {
            yield return null;
        }

        StartCoroutine(SpawnNextWave());
    }

    public void NotifyEnemyDefeated(Vector3 enemyPosition)
    {
        enemiesRemaining--;
        Debug.Log("Enemy defeated. Remaining: " + enemiesRemaining);

        if (enemiesRemaining == 0 && currentWaveIndex >= currentLevelWaves.Count)
        {
            SpawnKeycardAt(enemyPosition);
        }
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

    //public bool IsFinalEnemy()
    //{
    //    return enemiesRemaining == 1 && currentWaveIndex >= currentLevelWaves.Count;
    //}

    public void SpawnKeycardAt(Vector3 position)
    {
        if (keycardPrefab == null)
        {
            Debug.LogWarning("No keycard prefab assigned!");
            return;
        }

        Instantiate(keycardPrefab, position, Quaternion.identity);
        Debug.Log("Keycard spawned at enemy death location!");
    }

}
