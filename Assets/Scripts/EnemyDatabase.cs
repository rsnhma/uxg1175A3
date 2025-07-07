using UnityEngine;
using System.Collections.Generic;

public class EnemyDatabase : MonoBehaviour
{
    public static List<EnemyData> enemyList;

    void Awake()
    {
        TextAsset jsonText = Resources.Load<TextAsset>("enemies"); // loads Resources/enemies.json
        enemyList = JsonUtility.FromJson<Wrapper>(FixJson(jsonText.text)).enemies;
    }

    [System.Serializable]
    // Wrapper's job is to match the structure of the enemy JSON file
    private class Wrapper
    {
        public List<EnemyData> enemies;
    }

    private string FixJson(string value)
    {
        return "{\"enemies\":" + value + "}"; // wrap in an object
    }

    public static EnemyData GetEnemyById(string id)
    {
        return enemyList.Find(e => e.id == id);
    }

}
