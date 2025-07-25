using UnityEngine;

public class SpiderBehaviour : EnemyBehaviour
{
    [Header("Spider Stats")]
    public float moveSpeed = 3.0f;

    [Range(0f, 1f)]
    public float armorChance = 0.4f; // chance to activate armor when hit
    public float armorDuration = 2.0f;
    public int maxArmorUses = 5;

    private bool isArmored = false;
    private float armorTimer = 0f;
    private int armorUsesLeft;

    public Animator animator;

    [Header("Player Bite")]
    public float biteCooldown = 2f;
    private float biteTimer = 0f;

    [Header("Visuals")]
    public SpriteRenderer spriteRenderer; // assign in Inspector
    public Color armoredColor = Color.black;
    private Color originalColor;

    public GameObject armorText; // UI text or exclamation icon for armor

    [Header("Awareness UI")]
    public GameObject exclamationMark;

    protected override void Awake()
    {
        base.Awake();
        originalColor = spriteRenderer.color;
        if (armorText != null) armorText.SetActive(false);
        armorUsesLeft = maxArmorUses;
    }

    protected override void UpdateTargetDirection()
    {
        if (isArmored)
        {
            armorTimer -= Time.deltaTime;

            // Stop movement while armored
            targetDirection = Vector2.zero;
            rb.linearVelocity = Vector2.zero;

            if (armorTimer <= 0f)
            {
                isArmored = false;
                spriteRenderer.color = originalColor;
                if (armorText != null) armorText.SetActive(false);
            }

            animator.speed = 0f;
            return;
        }
        else
        {
            animator.speed = 1f; // resume anims if not armored
        }

        // Chase or roam
        if (awareness.AwareOfPlayer)
        {
            targetDirection = awareness.DirectionToPlayer;

            if (exclamationMark != null && !exclamationMark.activeSelf)
                exclamationMark.SetActive(true);
        }
        else
        {
            HandleRoam();
            targetDirection = roamDirection;

            if (exclamationMark != null && exclamationMark.activeSelf)
                exclamationMark.SetActive(false);
        }

        if (targetDirection.magnitude > 0.01f)
        {
            animator.SetFloat("MoveX", targetDirection.x);
            animator.SetFloat("MoveY", targetDirection.y);
        }
    }

    protected override float GetChaseSpeed()
    {
        return moveSpeed;
    }

    public override void TakeDamage(float amount)
    {
        if (isArmored)
        {
            Debug.Log("Spider is armored! No damage taken.");
            return;
        }

        if (armorUsesLeft > 0 && Random.value <= armorChance)
        {
            armorUsesLeft--;
            Debug.Log($"Spider activated armor! Uses left: {armorUsesLeft}");

            isArmored = true;
            armorTimer = armorDuration;

            spriteRenderer.color = armoredColor;
            if (armorText != null) armorText.SetActive(true);

            rb.linearVelocity = Vector2.zero; // stop immediately

            return; // skip damage
        }

        base.TakeDamage(amount);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && biteTimer <= 0f)
        {
            PlayerStats.Instance.TakeDamage(2f);
            Debug.Log("Player took 2 damage from Spider");
            biteTimer = biteCooldown;
        }
    }

    private void Update()
    {
        if (biteTimer > 0f)
            biteTimer -= Time.deltaTime;
    }
}
