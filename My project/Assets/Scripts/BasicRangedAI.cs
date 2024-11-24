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
    private bool hadLineOfSightLastFrame = false;

    private Vector2 playerTargetPositionOffset = new Vector2(0, 0);
    private float lastX;
    private float lastY;

    private float shootTimer;
    public float shootCooldown = 0.5f;
    public float sightDelay = 1.5f; // Delay after seeing the player before shooting
    private float sightTimer = 0f;

    private Vector2 lastSeenPosition;
    private bool hasSeenPlayer = false;
    private Vector2 lastRaycastHitPoint;

    public bool useLastSeenPosition = true;
    private bool hasFirstSightedPlayer = false;

    // New variable to track time since the player was last seen
    private float timeSinceLastSeen = Mathf.Infinity;
    private float timeBeforeGivingUp = 3f; // Time after which AI stops moving towards the last seen position

    private AudioSource audioSource;

    public AudioClip enemyshoot;

    void Start()
{
    player = GameObject.FindGameObjectWithTag("PlayerRaycast");
    lastSeenPosition = Vector2.zero;

    // Inicializa o AudioSource
    audioSource = GetComponent<AudioSource>();
    
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

        if (distance < radius && hasLineOfSight)
        {
            // Reset the time since the player was last seen
            timeSinceLastSeen = 0f;

            sightTimer += Time.deltaTime;
            if (!hasFirstSightedPlayer)
            {
                if (sightTimer >= sightDelay)
                {
                    hasFirstSightedPlayer = true;
                    sightTimer = 0f;
                }
            }

            if (hasFirstSightedPlayer)
            {
                transform.position = Vector2.MoveTowards(transform.position, lastRaycastHitPoint, speed * Time.deltaTime);

                lastX = direction.x;
                lastY = direction.y;

                animator.SetFloat("enemyX", direction.x);
                animator.SetFloat("enemyY", direction.y);
                animator.SetFloat("Moving", 1);

                lastSeenPosition = player.transform.position;
                hasSeenPlayer = true;
            }
        }
        else if (hasSeenPlayer && useLastSeenPosition)
        {
            sightTimer = 0f;

            // Increment time since last seen
            timeSinceLastSeen += Time.deltaTime;

            if (timeSinceLastSeen <= timeBeforeGivingUp)
            {
                transform.position = Vector2.MoveTowards(transform.position, lastSeenPosition, speed * Time.deltaTime);

                if (Vector2.Distance(transform.position, lastSeenPosition) < 0.1f)
                {
                    animator.SetFloat("Moving", 0);
                }
                else
                {
                    animator.SetFloat("Moving", 1);
                }
            }
            else
            {
                // AI stops moving after giving up
                animator.SetFloat("Moving", 0);
            }
        }
        else
        {
            animator.SetFloat("enemyX", 0);
            animator.SetFloat("enemyY", 0);
            animator.SetFloat("Moving", 0);

            animator.SetFloat("lastX", lastX);
            animator.SetFloat("lastY", lastY);

            timeSinceLastSeen += Time.deltaTime; // Increment time since the player was last seen
        }

        if (distance < radius && hasLineOfSight)
        {
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
            sightTimer = 0;
            shootTimer = shootCooldown;
        }

        hadLineOfSightLastFrame = hasLineOfSight;
    }

    private void FixedUpdate()
    {
        hasLineOfSight = false;

        Collider2D aiCollider = GetComponent<Collider2D>();
        Collider2D playerCollider = player.GetComponent<Collider2D>();

        if (aiCollider == null || playerCollider == null) return;

        Bounds aiBounds = aiCollider.bounds;
        Bounds playerBounds = playerCollider.bounds;

        Vector2[] aiCorners = GetBoundsCorners(aiBounds);
        Vector2[] playerCorners = GetBoundsCorners(playerBounds);
        Vector2 aiCenter = aiBounds.center;
        Vector2[] aiSideCenters = GetSideCenters(aiBounds);

        foreach (Vector2 aiCorner in aiCorners)
        {
            foreach (Vector2 playerCorner in playerCorners)
            {
                Vector2 direction = playerCorner - aiCorner;
                RaycastHit2D hit = Physics2D.Raycast(aiCorner, direction, direction.magnitude, layerMask);

                if (hit.collider != null && hit.collider.CompareTag("PlayerRaycast"))
                {
                    hasLineOfSight = true;
                    lastRaycastHitPoint = hit.point;
                    Debug.DrawRay(aiCorner, direction, Color.green);
                    return;
                }
                else
                {
                    Debug.DrawRay(aiCorner, direction, Color.red);
                }
            }
        }

        foreach (Vector2 aiSideCenter in aiSideCenters)
        {
            foreach (Vector2 playerCorner in playerCorners)
            {
                Vector2 direction = playerCorner - aiSideCenter;
                RaycastHit2D hit = Physics2D.Raycast(aiSideCenter, direction, direction.magnitude, layerMask);

                if (hit.collider != null && hit.collider.CompareTag("PlayerRaycast"))
                {
                    hasLineOfSight = true;
                    lastRaycastHitPoint = hit.point;
                    Debug.DrawRay(aiSideCenter, direction, Color.green);
                    return;
                }
                else
                {
                    Debug.DrawRay(aiSideCenter, direction, Color.red);
                }
            }
        }

        foreach (Vector2 playerCorner in playerCorners)
        {
            Vector2 direction = playerCorner - aiCenter;
            RaycastHit2D hit = Physics2D.Raycast(aiCenter, direction, direction.magnitude, layerMask);

            if (hit.collider != null && hit.collider.CompareTag("PlayerRaycast"))
            {
                hasLineOfSight = true;
                lastRaycastHitPoint = hit.point;
                Debug.DrawRay(aiCenter, direction, Color.green);
                return;
            }
            else
            {
                Debug.DrawRay(aiCenter, direction, Color.red);
            }
        }
    }

    private Vector2[] GetBoundsCorners(Bounds bounds)
    {
        return new Vector2[]
        {
            new Vector2(bounds.min.x, bounds.min.y),
            new Vector2(bounds.max.x, bounds.min.y),
            new Vector2(bounds.min.x, bounds.max.y),
            new Vector2(bounds.max.x, bounds.max.y)
        };
    }

    private Vector2[] GetSideCenters(Bounds bounds)
    {
        return new Vector2[]
        {
            new Vector2(bounds.center.x, bounds.min.y),
            new Vector2(bounds.center.x, bounds.max.y),
            new Vector2(bounds.min.x, bounds.center.y),
            new Vector2(bounds.max.x, bounds.center.y)
        };
    }

    void shoot()
    {
        if (ghostProjPos != null)
        {
            Instantiate(ghostProjectile, ghostProjPos.transform.position, Quaternion.identity);
            audioSource.PlayOneShot(enemyshoot);
        }
        else
        {
            Debug.LogWarning("No shooting point assigned for ghostProjPos!");
        }
    }
}
