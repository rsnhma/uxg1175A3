using UnityEngine;

[RequireComponent(typeof(EnemyPlayerAwareness))]
public class EnemyBehaviour : MonoBehaviour
{
    public string enemyId;
    public float moveSpeed;      // Used for chasing
    public float roamSpeed = 1.5f;  // Slower speed for roaming
    public float maxHealth;
    public float health;
    public float damageToPlayer;
    public string behavior;

    private EnemyPlayerAwareness awareness;

    private Rigidbody2D rb; // Recommended for smoother movement
    private Vector2 targetDirection;
    private Vector2 roamDirection;
    private float changeDirectionCooldown;

    public void Initialize(EnemyData data)
    {
        enemyId = data.id;
        moveSpeed = data.speed;
        maxHealth = data.health;
        health = data.health;
        damageToPlayer = data.damageToPlayer;
        behavior = data.behavior;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        awareness = GetComponent<EnemyPlayerAwareness>();
    }

    private void Start()
    {
        PickNewRoamDirection();
    }

    private void FixedUpdate()
    {
        UpdateTargetDirection();
        Move();
    }

    private void UpdateTargetDirection()
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

    private void HandleRoam()
    {
        changeDirectionCooldown -= Time.deltaTime;

        if (changeDirectionCooldown <= 0f)
        {
            PickNewRoamDirection();
        }
    }

    private void PickNewRoamDirection()
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

        float speed = awareness.AwareOfPlayer ? moveSpeed : roamSpeed;
        rb.linearVelocity = targetDirection * speed;
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Destroy(gameObject);
            EnemyWaveManager.Instance.NotifyEnemyDefeated();
        }
    }
}
