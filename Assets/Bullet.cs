using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public float lifeTime = 2f;

    private float timer;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    public void Init(Vector2 direction)
    {
        // Move in world space with velocity
        GetComponent<Rigidbody2D>().linearVelocity = direction.normalized * speed;

        // Rotate the bullet so its up matches velocity
        transform.up = direction;
    }
}
