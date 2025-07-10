using UnityEngine;

public class Potion : MonoBehaviour
{
    public float forceAmount = 10f;
    public float maxTravelDistance = 10f;  // max distance potion can fly
    public float damage = 15f;
    public GameObject poofEffect;

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
        //rb.AddForce(transform.right * forceAmount, ForceMode2D.Impulse);
    }

    void Update()
    {
        // Check how far we've travelled
        float traveled = Vector2.Distance(spawnPosition, transform.position);
        if (!hasHit && traveled >= maxTravelDistance)
        {
            hasHit = true;  // prevent multiple destroys
            Destroy(gameObject);  // destroy if max distance reached without hit
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (hasHit) return;
        hasHit = true;

        rb.simulated = false;

        if (poofEffect != null)
        {
            Instantiate(poofEffect, transform.position, Quaternion.identity);
        }

        EnemyBehaviour enemy = other.GetComponent<EnemyBehaviour>();
        if (enemy != null)
        {
            Debug.Log("Enemy hit. Destroying potion...");
            enemy.TakeDamage(damage);
            Destroy(gameObject, 0.1f);
        }
        else
        {
            Destroy(gameObject, 2f);
        }

    }
}