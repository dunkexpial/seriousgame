using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private DialogueUI dialogueUI;
    [SerializeField] private GameObject frozenEffectPrefab;
    [SerializeField] private GameObject afterimagePrefab;
    [SerializeField] private float afterimageInterval = 0.2f;
    [SerializeField] private float cooldownDuration = 10f;
    [SerializeField] private float powerUpDuration = 5f;

    private GameObject frozenEffectInstance;
    private List<GameObject> afterimages = new List<GameObject>();

    public DialogueUI DialogueUI => dialogueUI;
    public Interactable Interactable { get; set; }

    public float moveSpeed;
    public Rigidbody2D rb;
    public Animator animator;
    private Vector2 moveDirection;
    private Vector2 lastDirection;
    private bool _isFrozen = false;
    public bool isSlowed = false;
    private SoundManager soundManager;

    public bool isFrozen
    {
        get => _isFrozen;
        set
        {
            if (_isFrozen != value)
            {
                _isFrozen = value;
                HandleFrozenStateChange();
            }
        }
    }

    [SerializeField] private bool isPowerUpEnabled = false;
    public bool isPowerUpActive = false;
    private float powerUpTimer = 0f;
    private float cooldownTimer = 0f;
    private float lastAfterimageTime = 0f;

    void Start()
    {
        soundManager = FindAnyObjectByType<SoundManager>();
    }
    void Update()
    {
        // Handle cooldown timer
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }

        if (isFrozen)
        {
            animator.speed = 0;
            rb.velocity = Vector2.zero;
            return;
        }
        else if (isSlowed)
        {
            animator.speed = 0.5f;
        }
        else if (!isPowerUpActive)
        {
            animator.speed = 1;  // Set to default speed when power-up is not active
        }

        if (dialogueUI.isOpen)
        {
            moveDirection = Vector2.zero;
            rb.velocity = Vector2.zero;
            animator.SetFloat("Speed", 0);
            return;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Interactable != null)
            {
                Interactable.Interact(player: this);
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && isPowerUpEnabled && cooldownTimer <= 0)
        {
            ActivatePowerUp();
        }

        if (isPowerUpActive)
        {
            HandlePowerUp();
        }

        if (Time.timeScale != 0)
        {
            ProcessInputs();
            UpdateLastDirectionByCursor();

            if (Input.GetMouseButtonDown(0))
            {
                PlayShootingAnimation();
            }
        }
    }

    void FixedUpdate()
    {
        if (isFrozen)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        if (Time.timeScale != 0)
        {
            Move();
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    void ProcessInputs()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector2(moveX, moveY).normalized;

        if (moveDirection != Vector2.zero)
        {
            lastDirection = moveDirection;

            animator.SetFloat("Horizontal", moveDirection.x);
            animator.SetFloat("Vertical", moveDirection.y);
            animator.SetFloat("LastX", moveDirection.x);
            animator.SetFloat("LastY", moveDirection.y);
        }
        else
        {
            animator.SetFloat("Horizontal", 0);
            animator.SetFloat("Vertical", 0);
            animator.SetFloat("LastX", lastDirection.x);
            animator.SetFloat("LastY", lastDirection.y);
        }

        animator.SetFloat("Speed", moveDirection.sqrMagnitude);
    }

    void Move()
    {
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
    }

    void UpdateLastDirectionByCursor()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 cursorDirection = (mousePosition - transform.position).normalized;

        if (cursorDirection != Vector2.zero)
        {
            lastDirection = cursorDirection;
        }
    }

    void PlayShootingAnimation()
    {
        animator.SetTrigger("Shoot");
        animator.SetFloat("LastX", lastDirection.x);
        animator.SetFloat("LastY", lastDirection.y);
    }

    private void HandleFrozenStateChange()
    {
        if (isFrozen)
        {
            if (frozenEffectPrefab != null && frozenEffectInstance == null)
            {
                frozenEffectInstance = Instantiate(frozenEffectPrefab, transform.position, Quaternion.identity, transform);
            }
        }
        else
        {
            if (frozenEffectInstance != null)
            {
                Destroy(frozenEffectInstance);
            }
        }
    }

    void ActivatePowerUp()
    {
        if (!isPowerUpEnabled || cooldownTimer > 0) return;

        isPowerUpActive = true;
        powerUpTimer = powerUpDuration;
        moveSpeed *= 2;
        animator.speed *= 2;
        soundManager.PlaySoundBasedOnCollision("PlayerSonicSpeed");

        cooldownTimer = cooldownDuration;
    }

    void HandlePowerUp()
    {
        powerUpTimer -= Time.deltaTime;

        if (powerUpTimer <= 0)
        {
            DeactivatePowerUp();
        }
        else
        {
            if (Time.time - lastAfterimageTime >= afterimageInterval)
            {
                SpawnAfterimage();
                lastAfterimageTime = Time.time;
            }
        }
    }

    void DeactivatePowerUp()
    {
        isPowerUpActive = false;
        moveSpeed /= 2;
        animator.speed /= 2;

        afterimages.Clear();
    }

    void SpawnAfterimage()
    {
        if (afterimagePrefab != null)
        {
            GameObject afterimage = Instantiate(afterimagePrefab, transform.position, Quaternion.identity);

            SpriteRenderer playerSpriteRenderer = GetComponent<SpriteRenderer>();
            Sprite currentSprite = playerSpriteRenderer.sprite;

            SpriteRenderer afterimageSpriteRenderer = afterimage.GetComponent<SpriteRenderer>();
            if (afterimageSpriteRenderer != null)
            {
                afterimageSpriteRenderer.sprite = currentSprite;
            }

            afterimages.Add(afterimage);
        }
    }

    public void SetPowerUpEnabled(bool isEnabled)
    {
        isPowerUpEnabled = isEnabled;
    }

    public Vector2 GetMoveDirection()
    {
        return moveDirection;
    }

    public Vector2 GetLastDirection()
    {
        return lastDirection;
    }
}
