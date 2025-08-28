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

    private Vector2 lastSeenPosition;
    private bool hasSeenPlayer = false;  // Track if the AI has ever seen the player

    // Public variable to enable/disable the last seen position behavior
    public bool useLastSeenPosition = true;

    // Reflex delay time (time AI needs to see the player before acting)
    public float reflexDelay = 1.5f; // in seconds
    private float timeSeenPlayer = 0f; // Timer for how long the AI has seen the player

    private bool hasFirstSightedPlayer = false; // Track if this is the first time the AI sees the player
    private Vector2 lastRaycastHitPoint; // Store the exact point where the raycast hit
    private float maxTimeToChaseLastSeenPosition = 3f; // Time in seconds
    private float timeSinceLastSeen = 0f; // Timer for tracking how long AI is moving to the last seen position
    private float difficulty;
    private float reverseDifficulty;

    // How much space to keep from obstacles
    [SerializeField]
    private float wallAvoidanceDistance = 10f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("PlayerRaycast");
        lastSeenPosition = Vector2.zero;  // Initial value (could be anything, but we want to avoid using this as a "last known position")
        difficulty = PlayerPrefs.GetFloat("Difficulty");
        reverseDifficulty = PlayerPrefs.GetFloat("ReverseDifficulty");
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

        // If AI is within 30f, follow the center of the player
        if (distance <= 20f)
        {
            hasFirstSightedPlayer = true; // Skip reflex delay since we're focusing on proximity
            timeSinceLastSeen = 0f; // Reset the timer for the last seen position

            if (Vector2.Distance(transform.position, player.transform.position) > 10f)
            {
                // Move towards the player's center position, but avoid obstacles
                Vector2 targetPos = player.transform.position;
                Vector2 moveDir = (targetPos - (Vector2)transform.position).normalized;
                Vector2 avoidance = GetWallAvoidanceVector();
                Vector2 finalDir = (moveDir + avoidance).normalized;

                transform.position = Vector2.MoveTowards(this.transform.position, (Vector2)transform.position + finalDir, speed * Mathf.Pow(difficulty, 0.75f) * Time.deltaTime);

                lastX = moveDir.x;
                lastY = moveDir.y;

                // Update animator
                animator.SetFloat("enemyX", direction.x);
                animator.SetFloat("enemyY", direction.y);
                animator.SetFloat("Moving", 1);

                // Store the player's position when seen
                lastSeenPosition = player.transform.position;
                hasSeenPlayer = true; // Mark that the AI has seen the player
            }
        }
        else if (distance < (radius * difficulty) && hasLineOfSight)
        {
            timeSeenPlayer += Time.deltaTime; // Increment the timer since AI has line of sight to the player

            if (!hasFirstSightedPlayer) // If this is the first time AI sees the player
            {
                if (timeSeenPlayer >= reflexDelay) // Apply reflex delay before acting
                {
                    hasFirstSightedPlayer = true; // Mark that the AI has seen the player for the first time
                    timeSeenPlayer = 0f; // Reset the time since sighted
                }
            }

            if (hasFirstSightedPlayer) // Once the reflex delay is over, move towards the raycast hit point
            {
                // Move towards the raycast hit point (not the player center), but avoid obstacles
                Vector2 targetPos = lastRaycastHitPoint;
                Vector2 moveDir = (targetPos - (Vector2)transform.position).normalized;
                Vector2 avoidance = GetWallAvoidanceVector();
                Vector2 finalDir = (moveDir + avoidance).normalized;

                transform.position = Vector2.MoveTowards(this.transform.position, (Vector2)transform.position + finalDir, speed * Mathf.Pow(difficulty, 0.75f) * Time.deltaTime);

                lastX = moveDir.x;
                lastY = moveDir.y;

                // Update animator
                animator.SetFloat("enemyX", direction.x);
                animator.SetFloat("enemyY", direction.y);
                animator.SetFloat("Moving", 1);

                // Store the player's position when seen
                lastSeenPosition = player.transform.position;
                hasSeenPlayer = true; // Mark that the AI has seen the player
                timeSinceLastSeen = 0f; // Reset the timer for last seen position
            }
        }
        // If AI doesn't have line of sight and has seen the player before
        else if (hasSeenPlayer && useLastSeenPosition)
        {
            timeSeenPlayer = 0f; // Reset the timer when player is not visible

            // Increment the timer for moving to the last seen position
            timeSinceLastSeen += Time.deltaTime;

            if (timeSinceLastSeen < maxTimeToChaseLastSeenPosition) // Only move for the set duration
            {
                // Move to the last seen position, but avoid obstacles
                Vector2 targetPos = lastSeenPosition;
                Vector2 moveDir = (targetPos - (Vector2)transform.position).normalized;
                Vector2 avoidance = GetWallAvoidanceVector();
                Vector2 finalDir = (moveDir + avoidance).normalized;

                transform.position = Vector2.MoveTowards(this.transform.position, (Vector2)transform.position + finalDir, speed * Mathf.Pow(difficulty, 0.75f) * Time.deltaTime);

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
                // Stop moving when the timer expires
                animator.SetFloat("Moving", 0);
                hasSeenPlayer = false; // Reset seeing the player
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
                Vector2 direction = playerCorner - aiCorner;
                RaycastHit2D hit = Physics2D.Raycast(aiCorner, direction, radius * difficulty, layerMask); // Limit to AI radius

                if (hit.collider != null)
                {
                    Debug.DrawRay(aiCorner, direction, hit.collider.CompareTag("PlayerRaycast") ? Color.green : Color.red);
                    if (hit.collider.CompareTag("PlayerRaycast"))
                    {
                        hasLineOfSight = true;
                        lastRaycastHitPoint = hit.point;
                        return;
                    }
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

            if (hit.collider != null && hit.collider.CompareTag("PlayerRaycast"))
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

    // Returns a vector to steer away from nearby obstacles
    private Vector2 GetWallAvoidanceVector()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, wallAvoidanceDistance);
        Vector2 avoidance = Vector2.zero;
        int count = 0;
        foreach (var hit in hits)
        {
            if (hit.gameObject != this.gameObject && hit.CompareTag("ProjObstacle"))
            {
                Vector2 diff = (Vector2)transform.position - (Vector2)hit.ClosestPoint(transform.position);
                float dist = diff.magnitude;
                if (dist > 0)
                {
                    avoidance += diff.normalized / dist; // Stronger repulsion when closer
                    count++;
                }
            }
        }
        if (count > 0)
            avoidance = avoidance.normalized;
        return avoidance;
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
