using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public Animator animator;

    private Vector2 moveInput;
    private Vector2 lastMoveDir;

    void Update()
    {
        // Get WASD or arrow input (no smoothing)
        moveInput.x = Input.GetAxisRaw("Horizontal"); // A/D or Left/Right
        moveInput.y = Input.GetAxisRaw("Vertical");   // W/S or Up/Down

        // Normalize so diagonal isn't faster
        if (moveInput != Vector2.zero)
        {
            moveInput.Normalize();

            // Store last direction for idle facing
            lastMoveDir = moveInput;

            animator.SetFloat("lastMoveX", lastMoveDir.x);
            animator.SetFloat("lastMoveY", lastMoveDir.y);
        }

        // Pass movement to animator
        animator.SetFloat("moveX", moveInput.x);
        animator.SetFloat("moveY", moveInput.y);
        animator.SetBool("isMoving", moveInput != Vector2.zero);
    }

    void FixedUpdate()
    {
        // Actually move the player
        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
    }
}
