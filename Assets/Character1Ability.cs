using UnityEngine;

public class Character1Ability : MonoBehaviour
{
    public int maxHearts = 5;
    public int currentHearts { get; private set; }

    public float healCooldown = 10f;
    private float lastHealTime = -Mathf.Infinity;

    public KeyCode healKey = KeyCode.E; 

    public HealthUI healthUI; 

    private void Start()
    {
        currentHearts = maxHearts;

        // Initialize UI
        if (healthUI != null)
        {
            healthUI.SetMaxHearts(maxHearts);
            healthUI.UpdateHearts(currentHearts);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(healKey))
        {
            TryHeal(1);
        }
    }

    public void TryHeal(int amount)
    {
        if (Time.time >= lastHealTime + healCooldown)
        {
            Heal(amount);
            lastHealTime = Time.time;
        }
        else
        {
            Debug.Log("Heal on cooldown!");
        }
    }

    private void Heal(int amount)
    {
        int oldHearts = currentHearts;
        currentHearts += amount;
        if (currentHearts > maxHearts)
        {
            currentHearts = maxHearts;
        }

        if (currentHearts != oldHearts)
        {
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        if (healthUI != null)
        {
            healthUI.UpdateHearts(currentHearts);
        }
    }
}
