using UnityEngine;

public class GoblinBehaviour : EnemyBehaviour
{
    public float moveSpeed; 
    public float swingCooldown = 2f; // seconds between swings

    private float swingTimer = 0f;

    public Animator animator;

    protected override void UpdateTargetDirection()
    {
        if (awareness.AwareOfPlayer)
        {
            targetDirection = awareness.DirectionToPlayer;
        }
        else
        {
            HandleRoam();
            targetDirection = roamDirection;
        }

        // Count down swing timer while alive
        if (swingTimer > 0f)
        {
            swingTimer -= Time.deltaTime;
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

   //private void OnCollisionEnter2D(Collision2D collision)
   // {
   //     if (collision.gameObject.CompareTag("Player"))
   //     {
   //         if (swingTimer <= 0f)
   //         {
   //             PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
   //             if (playerHealth != null)
   //             {
   //                 playerHealth.TakeDamage(damageToPlayer);
   //                 Debug.Log($"Goblin swings at player! Deals {damageToPlayer} hearts.");
   //             }

   //             swingTimer = swingCooldown; // reset cooldown
   //         }
   //         else
   //         {
   //             Debug.Log("Goblin swing is on cooldown!");
   //         }
   //     }
   // }
}
