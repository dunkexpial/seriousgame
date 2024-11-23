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
    public float sightDelay = 1.5f; // Delay after seeing the player before shooting
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
    private Vector2 lastRaycastHitPoint; // Store the exact point where the raycast hit

    // Public variable to enable/disable the last seen position behavior
    public bool useLastSeenPosition = true;

    // New flag to check if it's the first time the AI sees the player
    private bool hasFirstSightedPlayer = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("PlayerRaycast");
        lastSeenPosition = Vector2.zero;  // Initial value (could be anything, but we want to avoid using this as a "last known position")
    }

    void Update()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("PlayerRaycast");
            if (player == null) return;
        }

        distance = Vector2.Distance(transform.position, player.transform.position);
        Vector2 direction = player.transform.position - transform.position;

        // If AI has line of sight to the player
        if (distance < radius && hasLineOfSight)
        {
            sightTimer += Time.deltaTime;  // Increment the sight timer when AI sees the player

            // Only apply the sight delay the first time the AI sees the player
            if (!hasFirstSightedPlayer)
            {
                if (sightTimer >= sightDelay)
                {
                    hasFirstSightedPlayer = true; // Mark the first sight
                    sightTimer = 0f; // Reset the timer after the first sight
                }
            }

            // If the sight delay has passed (or if it's after the first time)
            if (hasFirstSightedPlayer)
            {
                // Move towards the point where the AI last saw the player (raycast hit point)
                transform.position = Vector2.MoveTowards(transform.position, lastRaycastHitPoint, speed * Time.deltaTime);

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
        }
        // If AI doesn't have line of sight and has seen the player before
        else if (hasSeenPlayer && useLastSeenPosition)
        {
            sightTimer = 0f;  // Reset the timer when the player is not in sight

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

        // Get colliders for raycasting
        Collider2D aiCollider = GetComponent<Collider2D>();
        Collider2D playerCollider = player.GetComponent<Collider2D>();

        if (aiCollider == null || playerCollider == null) return;

        // Get bounds of both colliders
        Bounds aiBounds = aiCollider.bounds;
        Bounds playerBounds = playerCollider.bounds;

        // Get the corners of the AI's collider bounds
        Vector2[] aiCorners = GetBoundsCorners(aiBounds);

        // Get the corners of the player's collider bounds
        Vector2[] playerCorners = GetBoundsCorners(playerBounds);

        // Get the center and midpoints of the AI's collider sides
        Vector2 aiCenter = aiBounds.center;
        Vector2[] aiSideCenters = GetSideCenters(aiBounds);

        // Check all combinations of corners and midpoints
        foreach (Vector2 aiCorner in aiCorners)
        {
            foreach (Vector2 playerCorner in playerCorners)
            {
                // Cast a ray from the AI corner to the player corner
                Vector2 direction = playerCorner - aiCorner;
                RaycastHit2D hit = Physics2D.Raycast(aiCorner, direction, direction.magnitude, layerMask);

                // Check if the ray hits the player
                if (hit.collider != null && hit.collider.CompareTag("PlayerRaycast"))
                {
                    hasLineOfSight = true;
                    lastRaycastHitPoint = hit.point;  // Store the point where the raycast hit
                    Debug.DrawRay(aiCorner, direction, Color.green); // Raycast hit, visualize with green
                    return; // Stop checking if we already have line of sight
                }
                else
                {
                    Debug.DrawRay(aiCorner, direction, Color.red); // Raycast missed or blocked, visualize with red
                }
            }
        }

        // Check rays from the AI's side centers and middle
        foreach (Vector2 aiSideCenter in aiSideCenters)
        {
            foreach (Vector2 playerCorner in playerCorners)
            {
                // Cast a ray from the AI side center to the player corner
                Vector2 direction = playerCorner - aiSideCenter;
                RaycastHit2D hit = Physics2D.Raycast(aiSideCenter, direction, direction.magnitude, layerMask);

                if (hit.collider != null && hit.collider.CompareTag("PlayerRaycast"))
                {
                    hasLineOfSight = true;
                    lastRaycastHitPoint = hit.point;  // Store the point where the raycast hit
                    Debug.DrawRay(aiSideCenter, direction, Color.green);
                    return; // Stop checking if we already have line of sight
                }
                else
                {
                    Debug.DrawRay(aiSideCenter, direction, Color.red);
                }
            }
        }

        // Ray from the center of the AI's collider
        foreach (Vector2 playerCorner in playerCorners)
        {
            Vector2 direction = playerCorner - aiCenter;
            RaycastHit2D hit = Physics2D.Raycast(aiCenter, direction, direction.magnitude, layerMask);

            if (hit.collider != null && hit.collider.CompareTag("PlayerRaycast"))
            {
                hasLineOfSight = true;
                lastRaycastHitPoint = hit.point;  // Store the point where the raycast hit
                Debug.DrawRay(aiCenter, direction, Color.green);
                return;
            }
            else
            {
                Debug.DrawRay(aiCenter, direction, Color.red);
            }
        }
    }

    // Helper function to get the corners of a collider's bounds
    private Vector2[] GetBoundsCorners(Bounds bounds)
    {
        return new Vector2[]
        {
            new Vector2(bounds.min.x, bounds.min.y), // Bottom-left corner
            new Vector2(bounds.max.x, bounds.min.y), // Bottom-right corner
            new Vector2(bounds.min.x, bounds.max.y), // Top-left corner
            new Vector2(bounds.max.x, bounds.max.y)  // Top-right corner
        };
    }

    // Helper function to get the center points of the sides of a collider's bounds
    private Vector2[] GetSideCenters(Bounds bounds)
    {
        return new Vector2[]
        {
            new Vector2(bounds.center.x, bounds.min.y), // Bottom-center
            new Vector2(bounds.center.x, bounds.max.y), // Top-center
            new Vector2(bounds.min.x, bounds.center.y), // Left-center
            new Vector2(bounds.max.x, bounds.center.y)  // Right-center
        };
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
