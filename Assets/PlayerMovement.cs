using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public Animator animator;

    private Vector2 moveInput;
    public Vector2 lastMoveDir;

    private Character3Ability speedAbility;

    // Handgun (bullets)
    public Transform weaponHoldPoint;
    private SpriteRenderer weaponRenderer;
    public GameObject bulletPrefab;
    public Transform bulletBarrelExit;
    public GameObject handgunObject;

    // Beam gun
    public Transform beamWeaponHoldPoint;
    private SpriteRenderer beamWeaponRenderer;
    public Transform beamBarrelExit;
    public GameObject beamPrefab;
    public GameObject beamgunObject;
    private GameObject currentBeam;

    private enum WeaponType { Handgun, Beam }
    private WeaponType currentWeapon = WeaponType.Handgun;

    // Bullet ammo & cooldown
    private int bulletShotsLeft = 5;
    private float bulletCooldownTimer = 0f;
    private bool bulletOnCooldown = false;

    // Beam hold & cooldown
    private float beamHoldTime = 0f;
    private float beamCooldownTimer = 0f;
    private bool beamOnCooldown = false;
    private bool beamIsFiring = false;

    public Vector2 startFacingDirection = new Vector2(0, -1);

    void Start()
    {
        speedAbility = GetComponent<Character3Ability>();

        if (weaponHoldPoint != null)
            weaponRenderer = weaponHoldPoint.GetComponentInChildren<SpriteRenderer>();

        if (beamWeaponHoldPoint != null)
            beamWeaponRenderer = beamWeaponHoldPoint.GetComponentInChildren<SpriteRenderer>();

        
        if (startFacingDirection != Vector2.zero)
            lastMoveDir = startFacingDirection.normalized;
        else
            lastMoveDir = new Vector2(0, -1); // fallback default

        UpdateWeaponDirection();
        UpdateBeamWeaponDirection();
        SwitchWeaponVisuals();
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

            UpdateWeaponDirection();
            UpdateBeamWeaponDirection();
        }

        animator.SetFloat("moveX", moveInput.x);
        animator.SetFloat("moveY", moveInput.y);
        animator.SetBool("isMoving", moveInput != Vector2.zero);

        // Weapon switching
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentWeapon = WeaponType.Handgun;
            StopBeam(); // stop beam immediately if switching
            SwitchWeaponVisuals();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentWeapon = WeaponType.Beam;
            StopBeam(); // just in case
            SwitchWeaponVisuals();
        }

        // Fire input
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (currentWeapon == WeaponType.Handgun)
                TryShootBullet();
            else if (currentWeapon == WeaponType.Beam)
                StartBeam();
        }

        HandleBeamHold();
        HandleCooldowns();
    }

    void FixedUpdate()
    {
        float currentSpeed = moveSpeed;
        if (speedAbility != null)
            currentSpeed = speedAbility.GetCurrentSpeed();

        rb.MovePosition(rb.position + moveInput * currentSpeed * Time.fixedDeltaTime);
    }

    void HandleCooldowns()
    {
        if (bulletOnCooldown)
        {
            bulletCooldownTimer -= Time.deltaTime;
            if (bulletCooldownTimer <= 0f)
            {
                bulletShotsLeft = 5;
                bulletOnCooldown = false;
            }
        }

        if (beamOnCooldown)
        {
            beamCooldownTimer -= Time.deltaTime;
            if (beamCooldownTimer <= 0f)
            {
                beamOnCooldown = false;
            }
        }
    }

    void SwitchWeaponVisuals()
    {
        if (handgunObject != null) handgunObject.SetActive(currentWeapon == WeaponType.Handgun);
        if (beamgunObject != null) beamgunObject.SetActive(currentWeapon == WeaponType.Beam);
    }

    void UpdateWeaponDirection()
    {
        if (weaponHoldPoint == null) return;

        if (lastMoveDir.x > 0)
        {
            weaponHoldPoint.localEulerAngles = new Vector3(0, 0, 0);
            weaponHoldPoint.localScale = new Vector3(1, 1, 1);
            weaponHoldPoint.localPosition = new Vector3(0.25f, -0.25f, 0);
            SetWeaponSorting(weaponRenderer, 0);
        }
        else if (lastMoveDir.x < 0)
        {
            weaponHoldPoint.localEulerAngles = new Vector3(0, 0, 0);
            weaponHoldPoint.localScale = new Vector3(-1, 1, 1);
            weaponHoldPoint.localPosition = new Vector3(-0.25f, -0.25f, 0);
            SetWeaponSorting(weaponRenderer, 0);
        }
        else if (lastMoveDir.y > 0)
        {
            weaponHoldPoint.localEulerAngles = new Vector3(0, 0, 90);
            weaponHoldPoint.localScale = new Vector3(1, 1, 1);
            weaponHoldPoint.localPosition = new Vector3(0.25f, 0.1f, 0);
            SetWeaponSorting(weaponRenderer, -1);
        }
        else if (lastMoveDir.y < 0)
        {
            weaponHoldPoint.localEulerAngles = new Vector3(0, 0, -90);
            weaponHoldPoint.localScale = new Vector3(1, 1, 1);
            weaponHoldPoint.localPosition = new Vector3(0.25f, -0.25f, 0);
            SetWeaponSorting(weaponRenderer, 1);
        }
    }

    void UpdateBeamWeaponDirection()
    {
        if (beamWeaponHoldPoint == null) return;

        if (lastMoveDir.x > 0)
        {
            beamWeaponHoldPoint.localEulerAngles = new Vector3(0, 0, 0);
            beamWeaponHoldPoint.localScale = new Vector3(1, 1, 1);
            beamWeaponHoldPoint.localPosition = new Vector3(0.25f, -0.25f, 0);
            SetWeaponSorting(beamWeaponRenderer, 0);
        }
        else if (lastMoveDir.x < 0)
        {
            beamWeaponHoldPoint.localEulerAngles = new Vector3(0, 0, 0);
            beamWeaponHoldPoint.localScale = new Vector3(-1, 1, 1);
            beamWeaponHoldPoint.localPosition = new Vector3(-0.25f, -0.25f, 0);
            SetWeaponSorting(beamWeaponRenderer, 0);
        }
        else if (lastMoveDir.y > 0)
        {
            beamWeaponHoldPoint.localEulerAngles = new Vector3(0, 0, 90);
            beamWeaponHoldPoint.localScale = new Vector3(1, 1, 1);
            beamWeaponHoldPoint.localPosition = new Vector3(0.25f, 0, 0);
            SetWeaponSorting(beamWeaponRenderer, -1);
        }
        else if (lastMoveDir.y < 0)
        {
            beamWeaponHoldPoint.localEulerAngles = new Vector3(0, 0, -90);
            beamWeaponHoldPoint.localScale = new Vector3(1, 1, 1);
            beamWeaponHoldPoint.localPosition = new Vector3(0.25f, -0.25f, 0);
            SetWeaponSorting(beamWeaponRenderer, 1);
        }
    }

    void SetWeaponSorting(SpriteRenderer renderer, int order)
    {
        if (renderer != null)
            renderer.sortingOrder = order;
    }

    void TryShootBullet()
    {
        if (bulletOnCooldown) return;
        if (bulletShotsLeft <= 0)
        {
            bulletOnCooldown = true;
            bulletCooldownTimer = 5f;
            return;
        }

        bulletShotsLeft--;

        if (bulletPrefab == null || bulletBarrelExit == null) return;

        GameObject bullet = Instantiate(bulletPrefab, bulletBarrelExit.position, Quaternion.identity);
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
            bulletScript.Init(lastMoveDir);

        if (bulletShotsLeft <= 0)
        {
            bulletOnCooldown = true;
            bulletCooldownTimer = 5f;
        }
    }

    void StartBeam()
    {
        if (beamOnCooldown || beamIsFiring) return;

        beamIsFiring = true;
        beamHoldTime = 0f;

        if (beamPrefab != null && beamBarrelExit != null && currentBeam == null)
            currentBeam = Instantiate(beamPrefab, beamBarrelExit.position, Quaternion.identity);
    }

    void HandleBeamHold()
    {
        if (!beamIsFiring) return;

        beamHoldTime += Time.deltaTime;

        if (currentBeam != null)
        {
            currentBeam.transform.position = beamBarrelExit.position;
            currentBeam.transform.right = lastMoveDir;
        }

        if (beamHoldTime >= 5f)
        {
            StopBeam();
            beamOnCooldown = true;
            beamCooldownTimer = 5f;
        }
    }

    void StopBeam()
    {
        if (currentBeam != null)
        {
            Destroy(currentBeam);
            currentBeam = null;
        }

        beamIsFiring = false;
        beamHoldTime = 0f;
    }
}
