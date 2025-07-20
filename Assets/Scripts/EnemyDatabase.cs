using UnityEngine;
using System.Collections.Generic;

public class EnemyDatabase
{
    public static List<EnemyData> enemyList;

    static EnemyDatabase()
    {
        LoadDatabase();
    }

    private static void LoadDatabase()
    {
        TextAsset jsonText = Resources.Load<TextAsset>("enemies"); // loads Resources/enemies.json
        if (jsonText == null)
        {
            Debug.LogError("enemies.json not found in Resources!");
            enemyList = new List<EnemyData>();
            return;
        }

        enemyList = JsonUtility.FromJson<Wrapper>(FixJson(jsonText.text)).enemies; // loads and parses the enemies.json file into a List<EnemyData> by wrapping the array in an object
    }

    [System.Serializable]
    // wrapper's job is to match the structure of the enemy JSON file
    private class Wrapper
    {
        public List<EnemyData> enemies;
    }

    private static string FixJson(string value)
    {
        return "{\"enemies\":" + value + "}"; // wrap in an object
    }

    public static EnemyData GetEnemyById(string id)
    {
        return enemyList.Find(e => e.id == id);
    }

}
