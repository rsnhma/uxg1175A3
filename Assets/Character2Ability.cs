using UnityEngine;

public class Character2Ability : MonoBehaviour
{
    public float damageMultiplier = 1.5f;
    public float boostDuration = 5f;
    private float boostEndTime = -Mathf.Infinity;

    public float abilityCooldown = 10f;
    private float lastAbilityTime = -Mathf.Infinity;
    public KeyCode abilityKey = KeyCode.E;

    public GameObject flameEffectsPrefab;   
    private GameObject activeFlameEffects;  

    private void Update()
    {
        // Ability activation input
        if (Input.GetKeyDown(abilityKey))
        {
            TryActivateBoost();
        }

        // Check if boost expired and clean up flame effects
        if (Time.time > boostEndTime && activeFlameEffects != null)
        {
            Destroy(activeFlameEffects);
            activeFlameEffects = null;
        }
    }

    private void TryActivateBoost()
    {
        if (Time.time >= lastAbilityTime + abilityCooldown)
        {
            boostEndTime = Time.time + boostDuration;
            lastAbilityTime = Time.time;
            Debug.Log("Damage boost activated!");

            if (flameEffectsPrefab != null && activeFlameEffects == null)
            {
                // Instantiate flame VFX as child so it moves with character
                activeFlameEffects = Instantiate(flameEffectsPrefab, transform.position, Quaternion.identity, transform);
            }
        }
        else
        {
            Debug.Log("Ability on cooldown!");
        }
    }

    public float DealDamage(float baseDamage)
    {
        // Return boosted damage if within active window
        if (Time.time <= boostEndTime)
        {
            return baseDamage * damageMultiplier;
        }
        else
        {
            return baseDamage;
        }
    }
}
