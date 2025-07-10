using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public Animator animator;

    private Vector2 moveInput;
    [HideInInspector] public Vector2 lastMoveDir;

    private Character3Ability speedAbility;

    void Start()
    {
        speedAbility = GetComponent<Character3Ability>();
    }

    void Update()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        if (moveInput != Vector2.zero)
        {
            moveInput.Normalize();
            lastMoveDir = moveInput;

            animator.SetFloat("lastMoveX", lastMoveDir.x);
            animator.SetFloat("lastMoveY", lastMoveDir.y);
        }

        animator.SetFloat("moveX", moveInput.x);
        animator.SetFloat("moveY", moveInput.y);
        animator.SetBool("isMoving", moveInput != Vector2.zero);
    }

    void FixedUpdate()
    {
        float currentSpeed = moveSpeed;

        if (speedAbility != null)
        {
            currentSpeed = speedAbility.GetCurrentSpeed();
        }

        rb.MovePosition(rb.position + moveInput * currentSpeed * Time.fixedDeltaTime);
    }
}
