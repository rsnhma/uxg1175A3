using UnityEngine;

public class Character2Ability : MonoBehaviour
{
    public float damageMultiplier = 1.5f;
    public float boostDuration = 5f;
    private float boostEndTime = -Mathf.Infinity;

    public float abilityCooldown = 10f;
    private float lastAbilityTime = -Mathf.Infinity;

    public KeyCode abilityKey = KeyCode.E;

    private void Update()
    {
        if (Input.GetKeyDown(abilityKey))
        {
            TryActivateBoost();
        }
    }

    private void TryActivateBoost()
    {
        if (Time.time >= lastAbilityTime + abilityCooldown)
        {
            boostEndTime = Time.time + boostDuration;
            lastAbilityTime = Time.time;
            Debug.Log("Damage boost activated!");
        }
        else
        {
            Debug.Log("Ability on cooldown!");
        }
    }

    public float DealDamage(float baseDamage)
    {
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
