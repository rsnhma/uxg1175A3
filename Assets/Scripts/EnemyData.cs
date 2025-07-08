using System;
using System.Collections.Generic;

[System.Serializable]
public class EnemyData
{
    public string id;
    public string name;
    public float speed;
    public int health;
    public int damageToPlayer;
    public string behavior;
}

[System.Serializable]
public class EnemyDataList
{
    public List<EnemyData> enemies;
}
