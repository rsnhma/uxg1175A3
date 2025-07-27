using UnityEngine;
using TMPro;

public class GameSessionStats : MonoBehaviour
{
    public static GameSessionStats Instance;

    public int totalEnemiesDefeated = 0;

    [Header("UI")]
    public TextMeshProUGUI totalKillsText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep across scenes!
        }
        else
        {
            Destroy(gameObject); // Only one allowed
        }
    }
    public void AddKill()
    {
        totalEnemiesDefeated++;
        UpdateKillsUI();
    }

    public void UpdateKillsUI()
    {
        if (totalKillsText != null)
        {
            totalKillsText.text = totalEnemiesDefeated.ToString();
        }
    }

    public void ResetStats()
    {
        totalEnemiesDefeated = 0;
        UpdateKillsUI();
    }
}
