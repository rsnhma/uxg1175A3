using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{

    public string enemyId;
    public float moveSpeed;
    public float maxHealth;
    public float health;
    public float damageToPlayer;
    public string behavior;

    private Transform player; //TBC Brian's part, need sync up
    public void Initialize(EnemyData data)
    {
        enemyId = data.id;
        moveSpeed = data.speed;
        maxHealth = data.health;  // Set maxHealth from JSON
        health = data.health;     // Start at full health
        damageToPlayer = data.damageToPlayer;
        behavior = data.behavior;
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (player == null) return;

        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Destroy(gameObject);
            // Let the wave manager know this one is dead
            EnemyWaveManager.Instance.NotifyEnemyDefeated();
        }
    }
}
