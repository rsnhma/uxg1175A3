using UnityEngine;

public class RatBehaviour : EnemyBehaviour
{
    public float moveSpeed;  
    private float speedBoostMultiplier = 1.5f;

    private bool boosted = false;

    public Animator animator;

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
        }
        else
        {
            HandleRoam();
            targetDirection = roamDirection;
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

    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageToPlayer);
                Debug.Log($"Rat damaged player for {damageToPlayer} hearts!");
            }
        }
    }*/
}
