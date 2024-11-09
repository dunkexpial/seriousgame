using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private DialogueUI dialogueUI;
    [SerializeField] private GameObject frozenEffectPrefab; // Add prefab reference

    private GameObject frozenEffectInstance; // Store the instantiated effect

    public DialogueUI DialogueUI => dialogueUI;
    public Interactable Interactable { get; set; }

    public float moveSpeed;
    public Rigidbody2D rb;
    public Animator animator;
    private Vector2 moveDirection;
    private Vector2 lastDirection;
    private bool _isFrozen = false;

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

    void Update()
    {
        if (isFrozen)
        {
            animator.speed = 0;
            rb.velocity = Vector2.zero;
            return;
        }
        else
        {
            animator.speed = 1;
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
            // Spawn the prefab as a child of the player
            if (frozenEffectPrefab != null && frozenEffectInstance == null)
            {
                frozenEffectInstance = Instantiate(frozenEffectPrefab, transform.position, Quaternion.identity, transform);
            }
        }
        else
        {
            // Destroy the frozen effect prefab
            if (frozenEffectInstance != null)
            {
                Destroy(frozenEffectInstance);
            }
        }
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
