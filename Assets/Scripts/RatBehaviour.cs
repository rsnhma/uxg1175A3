using UnityEngine;
using UnityEngine.UI;

public class RatBehaviour : EnemyBehaviour
{
    public float moveSpeed;  
    private float speedBoostMultiplier = 1.5f;
    private float damageCooldown = 1f;
    private float lastDamageTime = -Mathf.Infinity;
    private bool boosted = false;

    public Animator animator;

    [Header("Awareness UI")]
    public GameObject exclamationMark;

    protected override void UpdateTargetDirection()
    {
        if (awareness.AwareOfPlayer)
        {
            if (!boosted)
            {
                moveSpeed *= speedBoostMultiplier;
                boosted = true;
                Debug.Log("Rat spotted player! Speed boosted.");
            }
            targetDirection = awareness.DirectionToPlayer;

            if (exclamationMark != null && !exclamationMark.activeSelf)
                exclamationMark.SetActive(true);
        }
        else
        {
            HandleRoam();
            targetDirection = roamDirection;

            if (exclamationMark != null && exclamationMark.activeSelf)
                exclamationMark.SetActive(false);
        }

        if (targetDirection.magnitude > 0.01f)
        {
            
            animator.SetFloat("MoveX", targetDirection.x);
            animator.SetFloat("MoveY", targetDirection.y);
        }

    }

    protected override float GetChaseSpeed()
    {
        return moveSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && Time.time >= lastDamageTime + damageCooldown)
        {
            PlayerStats.Instance.TakeDamage(0.5f);
            Debug.Log("PLayer took 0.5 damage from Rat");
            lastDamageTime = Time.time;
        }
    }
}
