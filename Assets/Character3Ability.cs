using UnityEngine;

public class Character3Ability : MonoBehaviour
{
    public float normalSpeed = 3f;
    public float boostedSpeed = 6f;

    public float boostDuration = 5f;
    private float boostEndTime = -Mathf.Infinity;

    public float abilityCooldown = 10f;
    private float lastAbilityTime = -Mathf.Infinity;

    public KeyCode abilityKey = KeyCode.E;

    private void Update()
    {
        if (Input.GetKeyDown(abilityKey))
        {
            TryActivateSpeedBoost();
        }
    }

    private void TryActivateSpeedBoost()
    {
        if (Time.time >= lastAbilityTime + abilityCooldown)
        {
            boostEndTime = Time.time + boostDuration;
            lastAbilityTime = Time.time;
            Debug.Log("Speed boost activated!");
        }
        else
        {
            Debug.Log("Speed boost on cooldown!");
        }
    }

    public float GetCurrentSpeed()
    {
        if (Time.time <= boostEndTime)
        {
            return boostedSpeed;
        }
        else
        {
            return normalSpeed;
        }
    }
}
