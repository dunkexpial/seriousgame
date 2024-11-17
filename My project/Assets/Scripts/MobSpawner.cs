using System.Collections;
using UnityEngine;

public class MobSpawner : MonoBehaviour
{
    [SerializeField] private float spawnInterval = 5f; // Fixed base time between spawns
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private bool canSpawn = true;
    [SerializeField] private float spawnRangeX = 30f; // Horizontal range (Width of the spawn area)
    [SerializeField] private float spawnRangeY = 30f; // Vertical range (Height of the spawn area)
    [SerializeField] private int maxEnemiesInRange = 5;
    [SerializeField] private float minDistanceToPlayer = 30f; // Minimum distance to player for spawning
    [SerializeField] private float maxDistanceToPlayer = 100f; // Maximum distance to player for spawning
    private int maxEnemiesInScene = 10; // Max number of enemies allowed in the scene

    private Transform playerTransform; // Reference to the player's transform
    private Camera mainCamera; // Reference to the main camera

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogError("Player object not found. Ensure the player has the 'Player' tag.");
        }

        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Main camera not found.");
        }
        StartCoroutine(Spawner());
    }

    private IEnumerator Spawner()
    {
        while (canSpawn)
        {
            // Add randomness of ±50% to the spawn interval
            float randomizedInterval = spawnInterval * Random.Range(0.5f, 1.5f);
            yield return new WaitForSeconds(randomizedInterval);

            if (playerTransform == null || mainCamera == null)
            {
                Debug.LogWarning("Player transform or Camera is null. Spawner will not proceed.");
                continue;
            }

            // Count all "Enemy" tagged objects in the scene
            int enemyCountInScene = GameObject.FindGameObjectsWithTag("Enemy").Length;

            if (enemyCountInScene >= maxEnemiesInScene)
            {
                // If there are more than the allowed number of enemies, stop spawning
                continue;
            }

            // Define the spawn area with the 16:9 ratio
            Vector2 spawnArea = new Vector2(spawnRangeX * 2, spawnRangeY * 2);
            Collider2D[] nearbyEnemies = Physics2D.OverlapBoxAll(transform.position, spawnArea, 0f);

            int enemyCount = 0;

            // Count only objects with the "Enemy" tag
            foreach (var obj in nearbyEnemies)
            {
                if (obj.CompareTag("Enemy"))
                {
                    enemyCount++;
                }
            }

            if (enemyCount <= maxEnemiesInRange)
            {
                bool spawnSuccessful = false;
                int retryCount = 0; // Counter for retries
                int maxRetries = 10; // Max retries before giving up

                // Keep trying to find a valid spawn location until the max retries are reached
                while (!spawnSuccessful && retryCount < maxRetries)
                {
                    Vector3 randomOffset = new Vector3(
                        Random.Range(-spawnRangeX, spawnRangeX),
                        Random.Range(-spawnRangeY, spawnRangeY),
                        0f
                    );

                    Vector3 spawnPosition = transform.position + randomOffset;

                    // Check if the spawn position is within the camera's viewport (16:9 aspect ratio)
                    Vector3 viewportPosition = mainCamera.WorldToViewportPoint(spawnPosition);

                    // Ensure the spawn position is within the camera's view (x: 0 to 1, y: 0 to 1)
                    if (viewportPosition.x >= 0f && viewportPosition.x <= 1f && viewportPosition.y >= 0f && viewportPosition.y <= 1f)
                    {
                        // Check if the spawn position is within the valid distance range from the player
                        float distanceToPlayer = Vector3.Distance(spawnPosition, playerTransform.position);
                        if (distanceToPlayer >= minDistanceToPlayer && distanceToPlayer <= maxDistanceToPlayer &&
                            !IsPositionOccupiedByObstacle(spawnPosition))
                        {
                            int rand = Random.Range(0, enemyPrefabs.Length);
                            GameObject enemyToSpawn = enemyPrefabs[rand];

                            Instantiate(enemyToSpawn, spawnPosition, Quaternion.identity);
                            spawnSuccessful = true; // Spawn successful, exit the loop
                        }
                    }

                    retryCount++; // Increment the retry counter
                    yield return null; // Yield to prevent freezing the game
                }

                // If max retries are reached and no valid spawn was found, log a warning
                if (!spawnSuccessful)
                {
                    Debug.LogWarning("Failed to find a valid spawn position after " + maxRetries + " attempts.");
                }
            }
        }
    }

    private bool IsPositionOccupiedByObstacle(Vector3 position)
    {
        Collider2D[] obstacles = Physics2D.OverlapCircleAll(position, 0.5f);
        foreach (var obstacle in obstacles)
        {
            if (obstacle.CompareTag("Obstacle"))
            {
                return true;
            }
        }
        return false;
    }

    // This method draws a gizmo box in the scene view to visualize spawn area
    private void OnDrawGizmos()
    {
        // Set the gizmo color to a semi-transparent yellow
        Color gizmoColor = new Color(1f, 1f, 0f, 0.05f); // RGBA (yellow, 30% opacity)
        Gizmos.color = gizmoColor;

        // Draw a filled cube representing the spawn area
        Gizmos.DrawCube(transform.position, new Vector3(spawnRangeX * 2, spawnRangeY * 2, 0f));

        // Set the gizmo color to a slightly darker version for the borders
        Gizmos.color = new Color(1f, 1f, 0f, 0.05f); // RGBA (yellow, 60% opacity)

        // Draw a wireframe box that represents the spawn area borders
        Gizmos.DrawWireCube(transform.position, new Vector3(spawnRangeX * 2, spawnRangeY * 2, 0f));
    }
}
