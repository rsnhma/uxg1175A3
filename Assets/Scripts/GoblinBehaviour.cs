using UnityEngine;
using System.Collections;

public class GoblinBehaviour : EnemyBehaviour
{
    public float moveSpeed;
    public float swingCooldown = 2f;

    private float swingTimer = 0f;
    private bool isStriking = false;
    private bool isStunned = false;

    public Animator animator;

    protected override void UpdateTargetDirection()
    {
        if (isStriking || isStunned)
        {
            targetDirection = Vector2.zero;
            return;
        }

        if (awareness.AwareOfPlayer)
        {
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isStriking && swingTimer <= 0f)
        {
            StartCoroutine(StrikeRoutine(collision.transform));
        }
    }

    private IEnumerator StrikeRoutine(Transform playerTransform)
    {
        isStriking = true;
        animator.SetBool("isStriking", true);

        // Face the player
        bool isPlayerRight = playerTransform.position.x > transform.position.x;
        animator.SetBool("FacingRight", isPlayerRight);

        // Trigger correct strike animation
        if (isPlayerRight)
            animator.SetTrigger("StrikeRight");
        else
            animator.SetTrigger("StrikeLeft");

        // Damage Player
        PlayerStats.Instance.TakeDamage(1f);
        Debug.Log("Player took 1 damage from Goblin");

        // Wait for animation to finish (adjust time to match your anim clip)
        yield return new WaitForSeconds(0.7f);

        // Enter stun/cooldown
        isStriking = false;
        animator.SetBool("isStriking", false);

        // Start stun/cooldown
        isStunned = true;
        yield return new WaitForSeconds(swingCooldown); // 2 seconds or whatever cooldown
        isStunned = false;
    }

    protected override float GetChaseSpeed()
    {
        return moveSpeed;
    }
}
