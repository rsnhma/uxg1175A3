using UnityEngine;

public class Character1Ability : MonoBehaviour
{
    public float healCooldown = 10f;
    private float lastHealTime = -Mathf.Infinity;

    public KeyCode healKey = KeyCode.E;

    [Header("Healing Particles")]
    public GameObject healParticlesPrefab;

    private PlayerHealth playerHealth;

    private void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();

        if (playerHealth == null)
        {
            Debug.LogError("PlayerHealth component not found on this GameObject!");
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
        if (playerHealth != null)
        {
            int oldHealth = playerHealth.currentHealth;
            playerHealth.currentHealth += amount;
            playerHealth.currentHealth = Mathf.Clamp(playerHealth.currentHealth, 0, playerHealth.maxHealth);

            if (playerHealth.currentHealth != oldHealth)
            {
                playerHealth.UpdateHealthUI();
                PlayHealParticles();
            }
        }
    }

    private void PlayHealParticles()
    {
        if (healParticlesPrefab != null)
        {
            Instantiate(healParticlesPrefab, transform.position, Quaternion.identity, transform)
                .GetComponent<ParticleSystem>()?.Play();
        }
    }
}
