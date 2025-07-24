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
        // Hit enemy
        if (collision.CompareTag("Enemy"))
        {
            EnemyBehaviour enemy = collision.GetComponent<EnemyBehaviour>();
            if (enemy != null)
            {
                float damageToDeal = damage;

                Character2Ability boost = FindObjectOfType<Character2Ability>();
                if (boost != null)
                    damageToDeal = boost.DealDamage(damageToDeal);

                enemy.TakeDamage(damageToDeal);
            }

            Destroy(gameObject);
            return;
        }

        // Hit wall: either Border or Walls layer
        int layer = collision.gameObject.layer;
        if (layer == LayerMask.NameToLayer("Border") || layer == LayerMask.NameToLayer("Walls"))
        {
            Destroy(gameObject);
        }
    }
}
