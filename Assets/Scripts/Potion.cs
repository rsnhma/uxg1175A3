using UnityEngine;

public class Potion : MonoBehaviour
{
    public float speed = 10f;
    public float damage = 15f;

    void Start()
    {
        GetComponent<Rigidbody2D>().linearVelocity = transform.right * speed;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Assuming enemy has a Health script
            //other.GetComponent<EnemyHealth>()?.TakeDamage(damage);
            Destroy(gameObject);  // Destroy potion on hit
        }
    }
}