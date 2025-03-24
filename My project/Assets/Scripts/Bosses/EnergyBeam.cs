using UnityEngine;

public class EnergyBeam : MonoBehaviour
{
    public LineRenderer lineRenderer;          // The LineRenderer to display the laser
    public Transform firePoint;                // The point where the laser originates
    public LayerMask hitLayers;                // Layers the laser can hit
    public GameObject prefab;                  // The prefab to instantiate along the laser path
    public GameObject hitPrefab;               // The prefab to instantiate at the hit point
    public float prefabSpacing = 1f;           // The distance between each prefab instance along the laser
    public float prefabInterval = 1f;          // Time interval in seconds between each prefab instantiation
    public float hitPrefabInterval = 1f;       // Time interval in seconds for the hit prefab instantiation

    private HealthManager healthManager;
    private RaycastHit2D hit;
    private float timer = 0f;                  // Timer to track time for prefab instantiation along the laser
    private float hitPrefabTimer = 0f;         // Timer to track time for hit prefab instantiation
    private bool laserActive = false;          // To check if the laser is active (fired)

    void Update()
    {
        ShootLaser();
    }

    void Start()
    {
        healthManager = FindObjectOfType<HealthManager>();
    }

    void ShootLaser()
    {
        // Shoot the laser in the direction the object is facing (forward)
        Vector2 laserDirection = transform.right;  // Use the object's right direction (forward in 2D)

        // Perform a hitscan raycast
        hit = Physics2D.Raycast(firePoint.position, laserDirection, Mathf.Infinity, hitLayers);

        // Debug: Draw the laser direction in the scene view
        Debug.DrawRay(firePoint.position, laserDirection * 10, Color.red); // Visualize the laser ray

        Vector2 endPosition = firePoint.position;

        if (hit.collider != null)
        {
            // If the laser hits something, set the end position to the hit point
            endPosition = hit.point;

            // Debug: Log what the laser is hitting
            Debug.Log("Laser hit: " + hit.collider.name);

            // Check if the hit object is on the "Player" layer
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                // Log that the player was hit
                Debug.Log("Player hit! Starting damage coroutine...");

                // Get the PlayerCollision component from the parent of the hit object
                PlayerCollision playerCollision = hit.collider.transform.parent.GetComponent<PlayerCollision>();
                if (playerCollision != null && !playerCollision.isInvincible && HealthManager.health > 0)
                {
                    // Trigger the damage logic in the other scripts
                    playerCollision.EnergyBeamClusterfuck();
                }
            }

            // Set the laser as active
            laserActive = true;
        }
        else
        {
            // If no hit, deactivate the laser
            laserActive = false;
        }

        // Update the laser visuals (LineRenderer)
        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, endPosition);

        // Generate prefabs every few seconds if the laser is active
        if (laserActive)
        {
            timer += Time.deltaTime;  // Increase the timer by the time passed since last frame

            if (timer >= prefabInterval)
            {
                // Reset the timer
                timer = 0f;

                // Instantiate prefab along the laser path
                CreatePrefabsAlongLaser(firePoint.position, endPosition);
            }

            // Generate hit prefab at the hit position with a timer
            hitPrefabTimer += Time.deltaTime;

            if (hitPrefabTimer >= hitPrefabInterval)
            {
                // Reset the timer
                hitPrefabTimer = 0f;

                // Instantiate hit prefab at the hit point if there was a hit
                if (hit.collider != null)
                {
                    Instantiate(hitPrefab, hit.point, Quaternion.identity);
                }
            }
        }
    }

    void CreatePrefabsAlongLaser(Vector2 startPosition, Vector2 endPosition)
    {
        // Calculate the direction and distance between the start and end points
        float distance = Vector2.Distance(startPosition, endPosition);
        Vector2 direction = (endPosition - startPosition).normalized;

        // Instantiate prefabs at intervals along the laser's path
        for (float i = 0f; i < distance; i += prefabSpacing)
        {
            Vector2 position = startPosition + direction * i;
            Instantiate(prefab, position, Quaternion.identity);
        }
    }
}
