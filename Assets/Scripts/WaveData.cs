using System;
using System.Collections.Generic;

[Serializable]
public class WaveData
{
    public int waveNumber;
    public string enemyId;
    public int count;
    public int countMin;
    public int countMax;
}

[Serializable]
public class LevelWaveData
{
    public int level;
    public List<WaveData> waves;
}

[Serializable]
public class LevelWaveList
{
    public List<LevelWaveData> levelWaves;
}
