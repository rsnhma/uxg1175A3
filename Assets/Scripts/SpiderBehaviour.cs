using UnityEngine;

public class SpiderBehaviour : EnemyBehaviour
{
    public float moveSpeed = 3.0f;
    [Range(0f, 1f)]
    public float dodgeChance = 0.3f;
    public float dodgeDistance = 1.5f; // how far to dodge
    public float dodgeSpeed = 10f;     // how fast the dodge happens

    private bool isDodging = false;
    private Vector2 dodgeTarget;

    public Animator animator;

    private float biteCooldown = 2f; // spider stun
    private float biteTimer = 0f;

    protected override void UpdateTargetDirection()
    {
        if (biteTimer > 0f)
        {
            biteTimer -= Time.deltaTime;
            targetDirection = Vector2.zero; // stop moving while stunned

            animator.speed = 0f; // PAUSE anim when stunned
            return;
        }
        else
        {
            animator.speed = 1f; // RESUME when unstunned
        }

        if (isDodging)
        {
            // Move towards the dodge target
            Vector2 direction = (dodgeTarget - (Vector2)transform.position).normalized;
            rb.linearVelocity = direction * dodgeSpeed;

            // If close enough, stop dodging
            if (Vector2.Distance(transform.position, dodgeTarget) < 0.1f)
            {
                isDodging = false;
            }
        }
        else if (awareness.AwareOfPlayer)
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

    protected override float GetChaseSpeed()
    {
        return moveSpeed;
    }

    public override void TakeDamage(float amount)
    {
        if (Random.value <= dodgeChance && !isDodging)
        {
            Debug.Log("Spider dodged by moving!");

            // Pick a dodge direction perpendicular to player direction
            Vector2 toPlayer = awareness.DirectionToPlayer;
            Vector2 dodgeDir = Vector2.Perpendicular(toPlayer).normalized;

            // Randomly flip left or right
            if (Random.value < 0.5f) dodgeDir *= -1;

            dodgeTarget = (Vector2)transform.position + dodgeDir * dodgeDistance;
            isDodging = true;

            return; // skip damage
        }

        base.TakeDamage(amount);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && biteTimer <= 0f)
        {
            PlayerStats.Instance.TakeDamage(2f);
            Debug.Log("PLayer took 2 damage from Spider");
            biteTimer = biteCooldown;
        }
    }
}
