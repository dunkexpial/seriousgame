using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAI : MonoBehaviour
{
    public float speed;
    public float radius;
    private GameObject player;
    private float distance;
    public bool hasLineOfSight = false;

    public LayerMask layerMask;
    public Animator animator;

    public Vector2 playerTargetPositionOffset = new Vector2(0, 0);

    private float lastX;
    private float lastY;

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

    // Reflex delay time (time AI needs to see the player before acting)
    public float reflexDelay = 1.5f; // in seconds
    private float timeSeenPlayer = 0f; // Timer for how long the AI has seen the player

    private bool hasFirstSightedPlayer = false; // Track if this is the first time the AI sees the player
    private Vector2 lastRaycastHitPoint; // Store the exact point where the raycast hit

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
            timeSeenPlayer += Time.deltaTime;  // Increment the timer since AI has line of sight to the player

            if (!hasFirstSightedPlayer) // If this is the first time AI sees the player
            {
                if (timeSeenPlayer >= reflexDelay) // Apply reflex delay before acting
                {
                    hasFirstSightedPlayer = true; // Mark that the AI has seen the player for the first time
                    timeSeenPlayer = 0f; // Reset the time since sighted
                }
            }

            if (hasFirstSightedPlayer)  // Once the reflex delay is over, move towards the raycast hit point
            {
                // Move towards the raycast hit point (not the player center)
                transform.position = Vector2.MoveTowards(this.transform.position, lastRaycastHitPoint, speed * Time.deltaTime);

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
            timeSeenPlayer = 0f; // Reset the timer when player is not visible

            // Move to the last seen position
            transform.position = Vector2.MoveTowards(this.transform.position, lastSeenPosition, speed * Time.deltaTime);

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
    }

    private void FixedUpdate()
    {
        hasLineOfSight = false;

        // Get colliders
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
                if (hit.collider != null && hit.collider.CompareTag("Player"))
                {
                    hasLineOfSight = true;
                    lastRaycastHitPoint = hit.point;  // Store the exact point where the raycast hit
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

                if (hit.collider != null && hit.collider.CompareTag("Player"))
                {
                    hasLineOfSight = true;
                    lastRaycastHitPoint = hit.point;  // Store the exact point where the raycast hit
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

            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                hasLineOfSight = true;
                lastRaycastHitPoint = hit.point;  // Store the exact point where the raycast hit
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
            new Vector2(bounds.min.x, bounds.min.y), // Bottom-left
            new Vector2(bounds.min.x, bounds.max.y), // Top-left
            new Vector2(bounds.max.x, bounds.min.y), // Bottom-right
            new Vector2(bounds.max.x, bounds.max.y)  // Top-right
        };
    }

    // Helper function to get the center points of the sides of a collider's bounds
    private Vector2[] GetSideCenters(Bounds bounds)
    {
        return new Vector2[]
        {
            new Vector2(bounds.center.x, bounds.min.y), // Bottom center
            new Vector2(bounds.center.x, bounds.max.y), // Top center
            new Vector2(bounds.min.x, bounds.center.y), // Left center
            new Vector2(bounds.max.x, bounds.center.y)  // Right center
        };
    }

    // Gizmos to visualize the raycasts
    private void OnDrawGizmos()
    {
        if (player == null) return;

        // Get colliders
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

        // Gizmos for AI collider corners
        Gizmos.color = Color.green;
        foreach (Vector2 aiCorner in aiCorners)
        {
            Gizmos.DrawWireSphere(aiCorner, 0.1f);
        }

        // Gizmos for player collider corners
        Gizmos.color = Color.red;
        foreach (Vector2 playerCorner in playerCorners)
        {
            Gizmos.DrawWireSphere(playerCorner, 0.1f);
        }

        // Gizmos for AI side centers
        Gizmos.color = Color.yellow;
        foreach (Vector2 aiSideCenter in aiSideCenters)
        {
            Gizmos.DrawWireSphere(aiSideCenter, 0.1f);
        }

        // Gizmos for AI center
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(aiCenter, 0.1f);
    }
}
