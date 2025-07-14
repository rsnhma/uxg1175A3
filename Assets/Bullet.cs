using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public float lifeTime = 2f;
    public float damage = 1f;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    public void Init(Vector2 direction)
    {
        GetComponent<Rigidbody2D>().linearVelocity = direction.normalized * speed;
        transform.up = direction;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyBehaviour enemy = collision.GetComponent<EnemyBehaviour>();
        if (enemy != null)
        {
            float damageToDeal = 1f;

            // Check if player has Character2Ability and modify damage
            Character2Ability boost = FindObjectOfType<Character2Ability>();
            if (boost != null)
            {
                damageToDeal = boost.DealDamage(damageToDeal);
            }

            enemy.TakeDamage(damageToDeal);
            Destroy(gameObject);
        }
    }

}
