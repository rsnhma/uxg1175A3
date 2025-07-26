using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(EnemyPlayerAwareness))]
public class EnemyBehaviour : MonoBehaviour
{
    public string enemyId; // For EnemyWaveManager Behvaiour
    public float roamSpeed = 1.5f;  // Slower speed for roaming

    public float maxHealth;
    public float health;
    public float damageToPlayer;

    public Slider healthBarSlider;

    protected EnemyPlayerAwareness awareness;
    protected Rigidbody2D rb; 
    protected Vector2 targetDirection;
    protected Vector2 roamDirection;
    private float changeDirectionCooldown;

    [SerializeField] private LayerMask obstacleLayers; // assign to walls, border and decor
    [SerializeField] private float obstacleCheckDistance = 0.5f; // tweak to match our enemy size

    public void Initialize(EnemyData data)
    {
        enemyId = data.id;
        maxHealth = data.health;
        health = data.health;
        damageToPlayer = data.damageToPlayer;
    }

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        awareness = GetComponent<EnemyPlayerAwareness>();

    }

    private void Start()
    {
        health = maxHealth;
        UpdateHealthBar();
        PickNewRoamDirection();
  
    }

    private void FixedUpdate()
    {
        UpdateTargetDirection();
        Move();
    }

    protected virtual void UpdateTargetDirection()
    {
        if (awareness.AwareOfPlayer)
        {
            // Chase player
            targetDirection = awareness.DirectionToPlayer;
        }
        else
        {
            // Roam randomly
            HandleRoam();
            targetDirection = roamDirection;
        }
    }

    protected void HandleRoam()
    {
        changeDirectionCooldown -= Time.deltaTime;

        if (changeDirectionCooldown <= 0f)
        {
            PickNewRoamDirection();
        }
    }

    protected void PickNewRoamDirection()
    {
        float angle = Random.Range(0f, 360f);
        roamDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;
        changeDirectionCooldown = Random.Range(1f, 3f);
    }

    private void Move()
    {
        if (targetDirection == Vector2.zero)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 moveDir = targetDirection.normalized;

        if (IsPathBlocked(moveDir))
        {
            if (!awareness.AwareOfPlayer)
            {
                // Roaming & hit a wall ? pick new roam direction
                PickNewRoamDirection();
                return;
            }
            else
            {
                // Chasing & blocked ? stop for now (or add smart logic later)
                rb.linearVelocity = Vector2.zero;
                return;
            }

        }

        float speed = awareness.AwareOfPlayer ? GetChaseSpeed() : roamSpeed;
        rb.linearVelocity = moveDir * speed;
    }


    protected virtual bool IsPathBlocked(Vector2 dir)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, obstacleCheckDistance, obstacleLayers);
        return hit.collider != null;
    }

    protected virtual float GetChaseSpeed()
    {
        return roamSpeed; // fallback default, but subclasses will override
    }

    public virtual void TakeDamage(float amount)
    {
        health -= amount;
        health = Mathf.Clamp(health, 0, maxHealth);
        UpdateHealthBar();

        if (health <= 0)
        {
            Debug.Log($"{enemyId} health is zero or less, calling Die()");
            Die();
        }
    }

    protected virtual void UpdateHealthBar()
    {
        if (healthBarSlider != null)
            healthBarSlider.value = health / maxHealth;
    }

    protected virtual void Die()
    {
        Debug.Log($"Enemy {enemyId} died at {transform.position}");
        if (EnemyWaveManager.Instance != null)
            EnemyWaveManager.Instance.NotifyEnemyDefeated(transform.position);

        Destroy(gameObject);
    }
}
