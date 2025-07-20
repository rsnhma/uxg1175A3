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
            targetDirection = Vector2.zero; // stop moving while stunned

            animator.speed = 0f; // PAUSE anim when stunned
            return;
        }
        else
        {
            animator.speed = 1f; // RESUME when unstunned
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
        if (collision.gameObject.CompareTag("Player"))
        {
            if (swingTimer <= 0f)
            {
                // Stop moving and strike
                swingTimer = swingCooldown; // cooldown starts

                // Determine strike direction
                bool isFacingRight = targetDirection.x > 0;
                animator.SetBool("FacingRight", isFacingRight);

                animator.SetTrigger("Strike"); // triggers StrikeLeft or StrikeRight state

                PlayerStats.Instance.TakeDamage(1f);
                Debug.Log("Player took 1 damage from Goblin");

            }
        }
    }
}
