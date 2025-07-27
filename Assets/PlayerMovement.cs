using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public Animator animator;

    private Vector2 moveInput;
    public Vector2 lastMoveDir;

    public Vector2 startFacingDirection = new Vector2(0, -1);

    private Character3Ability speedAbility;

    // handgun 
    public Transform weaponHoldPoint;
    private SpriteRenderer weaponRenderer;
    public GameObject bulletPrefab;
    public Transform bulletBarrelExit;
    public GameObject handgunObject;

    // beam ray
    public Transform beamWeaponHoldPoint;
    private SpriteRenderer beamWeaponRenderer;
    public Transform beamBarrelExit;
    public GameObject beamPrefab;
    public GameObject beamgunObject;
    private GameObject currentBeam;

    // potion 
    public GameObject potionPrefab;
    public Transform throwPoint;

    // dagger
    public GameObject daggerPrefab;
    public Transform throwPoint1;

    private enum WeaponType { Handgun, Beam, Potion, Dagger }
    private WeaponType currentWeapon = WeaponType.Handgun;

    // bullet ammo & cooldown
    private int bulletShotsLeft = 5;
    private float bulletCooldownTimer = 5f;
    private bool bulletOnCooldown = false;

    // beam ray hold & cooldown
    private float beamHoldTime = 5f;
    private float beamCooldownTimer = 5f;
    private bool beamOnCooldown = false;
    private bool beamIsFiring = false;

    // potion throwing & cooldown
    public float throwForce = 5f;
    private int potionThrowsLeft = 5;
    private float potionCooldownTimer = 5f;
    private bool potionOnCooldown = false;

    // dagger throwing & cooldown
    public float throwForce1 = 5f;
    private int daggerThrowsLeft = 5;
    private float daggerCooldownTimer = 5f;
    private bool daggerOnCooldown = false;

    // UI reference
    public WeaponUIManager weaponUIManager;


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
            lastMoveDir = new Vector2(0, -1); // default

        UpdateHandgunWeaponDirection();
        UpdateBeamWeaponDirection();
        SwitchWeaponVisuals();
    }



    void Update()
    {
        if (Time.timeScale == 0f) return;
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        if (moveInput != Vector2.zero)
        {
            moveInput.Normalize();
            lastMoveDir = moveInput;

            animator.SetFloat("lastMoveX", lastMoveDir.x);
            animator.SetFloat("lastMoveY", lastMoveDir.y);

            UpdateHandgunWeaponDirection();
            UpdateBeamWeaponDirection();
        }

        animator.SetFloat("moveX", moveInput.x);
        animator.SetFloat("moveY", moveInput.y);
        animator.SetBool("isMoving", moveInput != Vector2.zero);

        // weapon switching
        if (Input.GetKeyDown(KeyCode.Alpha1)) // select handgun
        {
            currentWeapon = WeaponType.Handgun;
            StopBeam(); // stop beam immediately if switching
            SwitchWeaponVisuals();
            weaponUIManager?.UpdateSelectedWeaponUI("Handgun");

        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) // select beam ray
        {
            currentWeapon = WeaponType.Beam;
            StopBeam();
            SwitchWeaponVisuals();
            weaponUIManager?.UpdateSelectedWeaponUI("Beam");
        }

        if (Input.GetKeyDown(KeyCode.Alpha3)) // select potion
        {
            currentWeapon = WeaponType.Potion;
            StopBeam();
            SwitchWeaponVisuals();
            weaponUIManager?.UpdateSelectedWeaponUI("Potion");
        }


        if (Input.GetKeyDown(KeyCode.Alpha4)) // select dagger
        {
            currentWeapon = WeaponType.Dagger;
            StopBeam();
            SwitchWeaponVisuals();
            weaponUIManager?.UpdateSelectedWeaponUI("Dagger");
        }

        // fire input
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (currentWeapon == WeaponType.Handgun)
                ShootBullet();
            else if (currentWeapon == WeaponType.Beam)
                StartBeam();
            else if (currentWeapon == WeaponType.Potion)
                ThrowPotion();
            else if (currentWeapon == WeaponType.Dagger)
                ThrowDagger();
        }

        BeamHold();
        Cooldowns();
    }

    void FixedUpdate()
    {
        if (Time.timeScale == 0f) return;

        float currentSpeed = moveSpeed;
        if (speedAbility != null)
            currentSpeed = speedAbility.GetCurrentSpeed();

        rb.MovePosition(rb.position + moveInput * currentSpeed * Time.fixedDeltaTime);
    }

    void Cooldowns()
    {
        if (bulletOnCooldown)
        {
            bulletCooldownTimer -= Time.deltaTime;
            if (bulletCooldownTimer <= 0f)
            {
                bulletShotsLeft = 5;
                bulletOnCooldown = false;
                weaponUIManager?.UpdateAmmo("Handgun", bulletShotsLeft, 5);
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

        if (potionOnCooldown)
        {
            potionCooldownTimer -= Time.deltaTime;
            if (potionCooldownTimer <= 0f)
            {
                potionThrowsLeft = 5;
                potionOnCooldown = false;
                weaponUIManager?.UpdateAmmo("Potion", potionThrowsLeft, 5);
            }
        }

        if (daggerOnCooldown)
        {
            daggerCooldownTimer -= Time.deltaTime;
            if (daggerCooldownTimer <= 0f)
            {
                daggerThrowsLeft = 5;
                daggerOnCooldown = false;
                weaponUIManager?.UpdateAmmo("Dagger", daggerThrowsLeft, 5);
            }
        }

        weaponUIManager?.UpdateBeamCooldown(beamOnCooldown ? 1f - beamCooldownTimer / 5f : 1f);

    }

    public void SetInitialFacingDirection(Vector2 direction)
    {
        lastMoveDir = direction.normalized;

        // update animator so it shows the correct idle pose
        animator.SetFloat("lastMoveX", lastMoveDir.x);
        animator.SetFloat("lastMoveY", lastMoveDir.y);

        UpdateHandgunWeaponDirection();
        UpdateBeamWeaponDirection();
    }


    void SwitchWeaponVisuals()
    {
        if (handgunObject != null) handgunObject.SetActive(currentWeapon == WeaponType.Handgun);
        if (beamgunObject != null) beamgunObject.SetActive(currentWeapon == WeaponType.Beam);
    }

    void UpdateHandgunWeaponDirection()
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

    void ShootBullet()
    {
        if (bulletOnCooldown) return;
        if (bulletShotsLeft <= 0)
        {
            bulletOnCooldown = true;
            bulletCooldownTimer = 5f;
            return;
        }

        bulletShotsLeft--;
        weaponUIManager?.UpdateAmmo("Handgun", bulletShotsLeft, 5);

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

    void BeamHold()
    {
        if (!beamIsFiring) return;

        beamHoldTime += Time.deltaTime;

        if (currentBeam != null)
        {
            currentBeam.transform.position = beamBarrelExit.position;
            currentBeam.transform.right = lastMoveDir;

            float maxDistance = 10f;

            // create a combined layerMask for border and walls
            LayerMask wallLayers = LayerMask.GetMask("Border", "Walls");

            // raycast for anything in the wallLayers
            RaycastHit2D wallHit = Physics2D.Raycast(beamBarrelExit.position, lastMoveDir, maxDistance, wallLayers);

            // raycast for enemies (no layer mask so it hits anything)
            RaycastHit2D enemyHit = Physics2D.Raycast(beamBarrelExit.position, lastMoveDir, maxDistance);

            float wallDist = wallHit.collider != null ? wallHit.distance : maxDistance;
            float enemyDist = (enemyHit.collider != null && enemyHit.collider.CompareTag("Enemy")) ? enemyHit.distance : maxDistance;

            // use whichever hit is closer
            float actualDistance = Mathf.Min(wallDist, enemyDist);

            // shorten beam to that distance
            currentBeam.transform.localScale = new Vector3(actualDistance, currentBeam.transform.localScale.y, 1f);

            // debug line
            Debug.DrawRay(beamBarrelExit.position, lastMoveDir * actualDistance, Color.red);
        }

        if (beamHoldTime >= 5f)
        {
            StopBeam();
            beamOnCooldown = true;
            beamCooldownTimer = 5f;
        }
    }

    void ThrowPotion()
    {
        if (potionOnCooldown)
            return;

        if (potionThrowsLeft <= 0)
        {
            potionOnCooldown = true;
            potionCooldownTimer = 5f; // cooldown duration
            return;
        }

        potionThrowsLeft--;
        weaponUIManager?.UpdateAmmo("Potion", potionThrowsLeft, 5);

        Vector2 throwDirection = lastMoveDir != Vector2.zero ? lastMoveDir.normalized : Vector2.right;

        if (potionPrefab == null || throwPoint == null)
        {
            Debug.LogWarning("Missing potionPrefab or throwPoint.");
            return;
        }

        GameObject potion = Instantiate(potionPrefab, throwPoint.position, Quaternion.identity);

        Rigidbody2D rb = potion.GetComponent<Rigidbody2D>();
        Collider2D potionCollider = potion.GetComponent<Collider2D>();
        Collider2D playerCollider = GetComponent<Collider2D>();

        if (rb != null)
            rb.AddForce(throwDirection * throwForce, ForceMode2D.Impulse);

        if (playerCollider != null && potionCollider != null)
            Physics2D.IgnoreCollision(potionCollider, playerCollider);

        if (potionThrowsLeft <= 0)
        {
            potionOnCooldown = true;
            potionCooldownTimer = 5f;
        }
    }

    void ThrowDagger()
    {
        if (daggerOnCooldown) return;

        if (daggerThrowsLeft <= 0)
        {
            daggerOnCooldown = true;
            daggerCooldownTimer = 5f;
            return;
        }

        daggerThrowsLeft--;
        weaponUIManager?.UpdateAmmo("Dagger", daggerThrowsLeft, 5);


        Vector2 throwDirection = lastMoveDir != Vector2.zero ? lastMoveDir.normalized : Vector2.right;

        if (daggerPrefab == null || throwPoint1 == null)
        {
            Debug.LogWarning("Missing daggerPrefab or throwPoint1.");
            return;
        }

        float angle = Mathf.Atan2(throwDirection.y, throwDirection.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0, 0, angle - 90f);

        // instantiate dagger with correct rotation
        GameObject dagger = Instantiate(daggerPrefab, throwPoint1.position, rotation);

        // add force using the exact same throwDirection
        Rigidbody2D rb = dagger.GetComponent<Rigidbody2D>();
        Collider2D daggerCollider = dagger.GetComponent<Collider2D>();
        Collider2D playerCollider = GetComponent<Collider2D>();

        if (rb != null)
            rb.AddForce(throwDirection * throwForce1, ForceMode2D.Impulse);

        if (playerCollider != null && daggerCollider != null)
            Physics2D.IgnoreCollision(daggerCollider, playerCollider);

        if (daggerThrowsLeft <= 0)
        {
            daggerOnCooldown = true;
            daggerCooldownTimer = 5f;
        }
    }

}
