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

    protected override void UpdateTargetDirection()
    {
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

            return; // skip damage!
        }

        base.TakeDamage(amount);
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Player"))
    //    {
    //        PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
    //        if (playerHealth != null)
    //        {
    //            playerHealth.TakeDamage(damageToPlayer);
    //            Debug.Log($"Spider bit player for {damageToPlayer} hearts!");
    //        }
    //    }
    //}
}
