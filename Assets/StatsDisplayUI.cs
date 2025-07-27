using UnityEngine;
using TMPro;

public class StatsDisplayUI : MonoBehaviour
{
    public TextMeshProUGUI enemiesValue;
    public TextMeshProUGUI damageValue;
    public TextMeshProUGUI avgTimeValue;

    void Start()
    {
        if (GameStatsManager.Instance != null)
        {
            enemiesValue.text = GameStatsManager.Instance.totalEnemiesDefeated.ToString();
            damageValue.text = GameStatsManager.Instance.totalDamageTaken.ToString("F0");
            avgTimeValue.text = GameStatsManager.Instance.GetAvgWaveTime().ToString("F2") + "s";
        }
        else
        {
            enemiesValue.text = "-";
            damageValue.text = "-";
            avgTimeValue.text = "-";
        }
    }
}
