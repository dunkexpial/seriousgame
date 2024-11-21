using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicRangedAI : MonoBehaviour
{
    public float speed;
    public float radius;
    public float inRange;
    public GameObject ghostProjectile;
    public GameObject ghostProjPos; // Changed to GameObject
    public LayerMask layerMask;
    public Animator animator;

    private GameObject player;
    private float distance;
    private bool hasLineOfSight = false;
    private bool hadLineOfSightLastFrame = false; // New variable to track previous line of sight

    private Vector2 playerTargetPositionOffset = new Vector2(0, 0);
    private float lastX;
    private float lastY;

    private float shootTimer;
    public float shootCooldown = 0.5f;
    float sightDelay = 0.5f; // Delay after seeing the player before shooting
    float sightTimer = 0f; // Time spent with line of sight

    private Vector2[] raycastOffsets = new Vector2[] 
    {
        new Vector2(0, -2f),
        new Vector2(5.49f, -2f),
        new Vector2(-5.49f, -2f),
        new Vector2(0, 3.49f),
        new Vector2(0, -5.49f),
        new Vector2(5.49f, 3.49f),
        new Vector2(-5.49f, 3.49f),
        new Vector2(5.49f, -5.49f),
        new Vector2(-5.49f, -5.49f)
    };

    private Vector2 lastSeenPosition;
    private bool hasSeenPlayer = false;  // Track if the AI has ever seen the player

    // Public variable to enable/disable the last seen position behavior
    public bool useLastSeenPosition = true;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        lastSeenPosition = Vector2.zero;  // Initial value (could be anything, but we want to avoid using this as a "last known position")
    }

    void Update()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            if (player == null) return;
        }

        distance = Vector2.Distance(transform.position, player.transform.position);
        Vector2 direction = player.transform.position - transform.position;

        // If AI has line of sight to the player
        if (distance < radius && hasLineOfSight)
        {
            // Move towards the player
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);

            lastX = direction.x;
            lastY = direction.y;

            // Update animator
            animator.SetFloat("enemyX", direction.x);
            animator.SetFloat("enemyY", direction.y);
            animator.SetFloat("Moving", 1);

            // Store the player's position when seen
            lastSeenPosition = player.transform.position;
            hasSeenPlayer = true; // Mark that the AI has seen the player
        }
        // If AI doesn't have line of sight and has seen the player before
        else if (hasSeenPlayer && useLastSeenPosition)
        {
            // Move to the last seen position
            transform.position = Vector2.MoveTowards(transform.position, lastSeenPosition, speed * Time.deltaTime);

            // Check if the AI has reached the last seen position
            if (Vector2.Distance(transform.position, lastSeenPosition) < 0.1f)
            {
                // Stop moving and update the animator
                animator.SetFloat("Moving", 0);
            }
            else
            {
                animator.SetFloat("Moving", 1);
            }
        }
        else
        {
            // AI has never seen the player or useLastSeenPosition is disabled, so remain idle
            animator.SetFloat("enemyX", 0);
            animator.SetFloat("enemyY", 0);
            animator.SetFloat("Moving", 0);

            animator.SetFloat("lastX", lastX);
            animator.SetFloat("lastY", lastY);
        }

        if (distance < radius && hasLineOfSight)
        {
            // Increment the sight timer while the enemy has line of sight
            sightTimer += Time.deltaTime;

            if (sightTimer >= sightDelay)
            {
                shootTimer += Time.deltaTime;
                if (shootTimer >= shootCooldown)
                {
                    shoot();
                    shootTimer = 0;
                }
            }
        }
        else
        {
            // Reset timers when line of sight is lost
            sightTimer = 0;
            shootTimer = shootCooldown; // Reset the cooldown so that it doesn't shoot immediately upon regaining sight
        }

        hadLineOfSightLastFrame = hasLineOfSight;
    }

    private void FixedUpdate()
    {
        hasLineOfSight = false;

        foreach (Vector2 offset in raycastOffsets)
        {
            Vector2 targetPosition = (Vector2)player.transform.position + playerTargetPositionOffset + offset;
            RaycastHit2D ray = Physics2D.Raycast(transform.position, targetPosition - (Vector2)transform.position, Mathf.Infinity, layerMask);

            if(ray.collider != null && ray.collider.CompareTag("Player"))
            {
                hasLineOfSight = true;
                Debug.DrawRay(transform.position, targetPosition - (Vector2)transform.position, Color.green);
                break;
            }
            else
            {
                Debug.DrawRay(transform.position, targetPosition - (Vector2)transform.position, Color.red);
            }
        }
    }

    void shoot()
    {
        if (ghostProjPos != null)
        {
            Instantiate(ghostProjectile, ghostProjPos.transform.position, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("No shooting point assigned for ghostProjPos!");
        }
    }
}
