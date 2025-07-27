using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class GameStatsManager : MonoBehaviour
{
    public static GameStatsManager Instance;

    public int totalEnemiesDefeated = 0;
    public float totalDamageTaken = 0f;
    public List<float> waveTimes = new List<float>();
    private float currentWaveStartTime;

    private string logPath;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        logPath = Path.Combine(Application.persistentDataPath, "game_stats_log.json");
    }

    public void StartWaveTimer()
    {
        currentWaveStartTime = Time.time;
    }

    public void EndWaveTimer()
    {
        float time = Time.time - currentWaveStartTime;
        waveTimes.Add(time);
    }

    public void AddEnemyKill()
    {
        totalEnemiesDefeated++;
    }

    public void AddDamage(float dmg)
    {
        totalDamageTaken += dmg;
    }

    public float GetAvgWaveTime()
    {
        if (waveTimes.Count == 0) return 0f;
        float sum = 0f;
        foreach (float t in waveTimes) sum += t;
        return sum / waveTimes.Count;
    }

    public void SaveStatsToFile()
    {
        GameStatsData data = new GameStatsData
        {
            timestamp = DateTime.Now.ToString(),
            enemiesDefeated = totalEnemiesDefeated,
            totalDamageTaken = totalDamageTaken,
            averageWaveTime = GetAvgWaveTime()
        };

        string json = JsonUtility.ToJson(data, true);
        File.AppendAllText(logPath, json + "\n");
    }

    [Serializable]
    public class GameStatsData
    {
        public string timestamp;
        public int enemiesDefeated;
        public float totalDamageTaken;
        public float averageWaveTime;
    }
    void OnApplicationQuit()
    {
        SaveStatsToFile();
    }
}
