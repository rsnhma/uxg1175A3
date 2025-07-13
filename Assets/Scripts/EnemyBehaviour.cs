using UnityEngine;

[RequireComponent(typeof(EnemyPlayerAwareness))]
public class EnemyBehaviour : MonoBehaviour
{
    public string enemyId; // For EnemyWaveManager Behvaiour
    public float roamSpeed = 1.5f;  // Slower speed for roaming
    public float maxHealth;
    public float health;
    public float damageToPlayer;
   

    protected EnemyPlayerAwareness awareness;

    protected Rigidbody2D rb; 
    protected Vector2 targetDirection;
    protected Vector2 roamDirection;
    private float changeDirectionCooldown;

    public void Initialize(EnemyData data)
    {
        enemyId = data.id;
        maxHealth = data.health;
        health = data.health;
        damageToPlayer = data.damageToPlayer;
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

        float speed = awareness.AwareOfPlayer ? GetChaseSpeed() : roamSpeed;
        rb.linearVelocity = targetDirection * speed;
    }

    protected virtual float GetChaseSpeed()
    {
        return roamSpeed; // fallback default, but subclasses will override
    }


    public virtual void TakeDamage (float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            if (EnemyWaveManager.Instance != null)
            {
                EnemyWaveManager.Instance.NotifyEnemyDefeated();
            }
            else
            {
                Debug.LogWarning("EnemyWaveManager.Instance is null!");
            }
            Destroy(gameObject);

        }
    }
}
