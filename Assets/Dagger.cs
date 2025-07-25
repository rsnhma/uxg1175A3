using UnityEngine;

public class Dagger : MonoBehaviour
{
    public float forceAmount = 12f;
    public float maxTravelDistance = 10f;
    public float damage = 25f;
    public GameObject hitEffect;

    private bool hasHit = false;
    private Rigidbody2D rb;
    private Vector2 spawnPosition;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spawnPosition = transform.position;
    }

    void Start()
    {

    }

    void Update()
    {
        float traveled = Vector2.Distance(spawnPosition, transform.position);
        if (!hasHit && traveled >= maxTravelDistance)
        {
            hasHit = true;
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (hasHit) return;
        hasHit = true;

        rb.simulated = false;

        if (hitEffect != null)
        {
            Instantiate(hitEffect, transform.position, Quaternion.identity);
        }

        EnemyBehaviour enemy = other.GetComponentInParent<EnemyBehaviour>(); // safer than GetComponent
        if (enemy != null)
        {
            Debug.Log("Dagger hit enemy: " + other.name);
            enemy.TakeDamage(damage);
            Destroy(gameObject, 0.1f);
        }
        else
        {
            Destroy(gameObject, 2f);
        }
    }
}
