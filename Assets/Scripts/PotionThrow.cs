using UnityEngine;

public class PotionThrow : MonoBehaviour
{
    public GameObject potionPrefab;
    public Transform throwPoint;
    public float throwForce = 10f;

    private PlayerMovement playerMovement;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();  // assumes this script is on the player GameObject
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ThrowPotion();
        }
    }

    void ThrowPotion()
    {
        Vector2 throwDirection = Vector2.right; // default

        if (playerMovement != null && playerMovement.lastMoveDir != Vector2.zero)
        {
            throwDirection = playerMovement.lastMoveDir.normalized;
        }

        // Instantiate potion at throwPoint position
        GameObject potion = Instantiate(potionPrefab, throwPoint.position, Quaternion.identity);

        Rigidbody2D rb = potion.GetComponent<Rigidbody2D>();
        Collider2D potionCollider = potion.GetComponent<Collider2D>();
        Collider2D playerCollider = GetComponent<Collider2D>(); // assumes this script is on player

        if (playerCollider != null && potionCollider != null)
        {
            Physics2D.IgnoreCollision(potionCollider, playerCollider);
        }

        // Apply force to potion rigidbody in facing direction
        rb.AddForce(throwDirection * throwForce, ForceMode2D.Impulse);
    }
}