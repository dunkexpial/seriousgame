using System.Collections;
using UnityEngine;

public class MobSpawner : MonoBehaviour
{
    [SerializeField] private float spawnInterval = 5f; // Time between spawns
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private bool canSpawn = true;
    [SerializeField] private float spawnRangeX = 30f; // Horizontal range of spawn area
    [SerializeField] private float spawnRangeY = 30f; // Vertical range of spawn area
    [SerializeField] private float minDistanceToPlayer = 30f; // Minimum distance to player
    [SerializeField] private float obstacleDetectionRadius = 2f; // Configurable radius for obstacle detection
    private float minDistanceToOtherEnemies = 50f; // Minimum distance from other enemies

    private int maxEnemiesInScene = 10; // Maximum enemies allowed in the scene
    private Transform playerTransform; // Reference to player's Transform
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
            float randomizedInterval = spawnInterval * Random.Range(0.5f, 1.5f);
            yield return new WaitForSeconds(randomizedInterval);

            if (playerTransform == null || mainCamera == null)
            {
                Debug.LogWarning("Player transform or Camera is null. Spawner will not proceed.");
                continue;
            }

            int enemyCountInScene = GameObject.FindGameObjectsWithTag("Enemy").Length;

            if (enemyCountInScene >= maxEnemiesInScene)
            {
                continue;
            }

            bool spawnSuccessful = false;

            while (!spawnSuccessful)
            {
                // Generate a random position within the defined spawn area
                float randomX = Random.Range(-spawnRangeX, spawnRangeX);
                float randomY = Random.Range(-spawnRangeY, spawnRangeY);
                Vector3 spawnPosition = new Vector3(randomX, randomY, 0f) + transform.position; // Add spawner's position to offset

                // Check distance constraints and obstacle-free status
                float distanceToPlayer = Vector3.Distance(spawnPosition, playerTransform.position);

                if (distanceToPlayer >= minDistanceToPlayer &&
                    !IsPositionOccupiedByObstacle(spawnPosition) &&
                    !IsPositionTooCloseToOtherEnemies(spawnPosition) &&
                    IsPositionWithinViewport(spawnPosition)) // Check if spawn position is within viewport
                {
                    int rand = Random.Range(0, enemyPrefabs.Length);
                    GameObject enemyToSpawn = enemyPrefabs[rand];

                    Instantiate(enemyToSpawn, spawnPosition, Quaternion.identity);
                    spawnSuccessful = true;
                }

                // Yield for a frame to avoid freezing the game while searching for a valid position
                yield return null;
            }
        }
    }

    private bool IsPositionOccupiedByObstacle(Vector3 position)
    {
        Collider2D[] obstacles = Physics2D.OverlapCircleAll(position, obstacleDetectionRadius);

        foreach (var obstacle in obstacles)
        {
            if (obstacle.CompareTag("Obstacle") || obstacle.CompareTag("ProjObstacle"))
            {
                return true;
            }

            if (obstacle.CompareTag("Safezone") && obstacle.isTrigger)
            {
                return true;
            }
        }
        return false;
    }

    private bool IsPositionTooCloseToOtherEnemies(Vector3 spawnPosition)
    {
        // Get all existing enemies in the scene
        GameObject[] existingEnemies = GameObject.FindGameObjectsWithTag("Enemy");

        // Check if the spawn position is too close to any existing enemy
        foreach (var enemy in existingEnemies)
        {
            float distanceToEnemy = Vector3.Distance(spawnPosition, enemy.transform.position);
            if (distanceToEnemy < minDistanceToOtherEnemies)
            {
                return true; // Position is too close to another enemy
            }
        }

        return false; // Position is valid
    }

    // New function to check if the position is within the camera's viewport
    private bool IsPositionWithinViewport(Vector3 position)
    {
        Vector3 viewportPosition = mainCamera.WorldToViewportPoint(position);

        // Check if the spawn position is within the camera's visible area
        return viewportPosition.x >= 0f && viewportPosition.x <= 1f && viewportPosition.y >= 0f && viewportPosition.y <= 1f;
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying && mainCamera != null)
        {
            // Gizmo to show the camera's visible area
            Gizmos.color = Color.yellow;
            Vector3 bottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0f, 0f, mainCamera.nearClipPlane));
            Vector3 topRight = mainCamera.ViewportToWorldPoint(new Vector3(1f, 1f, mainCamera.nearClipPlane));
            Vector3 size = topRight - bottomLeft;
            Gizmos.DrawWireCube(bottomLeft + size / 2, size);
        }

        if (Application.isPlaying && playerTransform != null)
        {
            // Gizmo to show the player’s minimum spawn distance
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(playerTransform.position, minDistanceToPlayer); // Minimum spawn distance
        }

        // Gizmos to show the full spawn area (in 2D space)
        Gizmos.color = Color.blue;
        Vector3 spawnAreaCenter = transform.position; // Use the spawner's position as the center of the spawn area
        Vector3 spawnAreaSize = new Vector3(spawnRangeX * 2, spawnRangeY * 2, 0f); // Full spawn area size (double the range to account for both positive/negative)

        // Draw the spawn area as a wireframe rectangle
        Gizmos.DrawWireCube(spawnAreaCenter, spawnAreaSize);
    }
}
