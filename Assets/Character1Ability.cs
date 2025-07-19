using UnityEngine;

public class Character1Ability : MonoBehaviour
{
    public float healCooldown = 10f;
    private float lastHealTime = -Mathf.Infinity;

    public KeyCode healKey = KeyCode.E;

    [Header("Healing Particles")]
    public GameObject healParticlesPrefab;

    private PlayerStats playerStats;

    //private void Start()
    //{
    //    playerStats = GetComponent<PlayerStats>();

    //    if (playerStats == null)
    //    {
    //        Debug.LogError("PlayerStats component not found on this GameObject!");
    //    }
    //}

    private void Update()
    {
        if (Input.GetKeyDown(healKey))
        {
            TryHeal(1f);
        }
    }

    public void TryHeal(float amount)
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

    private void Heal(float amount)
    {
        float oldHealth = PlayerStats.Instance.Health;

        PlayerStats.Instance.Heal(amount);

        // Only play particles if we actually healed
        if (PlayerStats.Instance.Health > oldHealth)
        {
            PlayHealParticles();
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
