using System.Collections;
using UnityEngine;

public class MobSpawner : MonoBehaviour
{
    [SerializeField] private float spawnInterval = 5f; // Time between spawns
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private bool canSpawn = true;
    [SerializeField] private float spawnRangeX = 30f; // Horizontal range of spawn area
    [SerializeField] private float spawnRangeY = 30f; // Vertical range of spawn area
    [SerializeField] private int maxEnemiesInRange = 5;
    [SerializeField] private float minDistanceToPlayer = 30f; // Minimum distance to player
    [SerializeField] private float maxDistanceToPlayer = 100f; // Maximum distance to player
    [SerializeField] private float obstacleDetectionRadius = 2f; // Configurable radius for obstacle detection
    [SerializeField] private float minDistanceToOtherEnemies = 30f; // Minimum distance from other enemies

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
                Debug.Log("Max enemies in scene reached: " + enemyCountInScene);
                continue;
            }

            bool spawnSuccessful = false;

            while (!spawnSuccessful)
            {
                // Get a random viewport position within the visible area
                Vector3 viewportPos = new Vector3(
                    Random.Range(0f, 1f), // X within viewport
                    Random.Range(0f, 1f), // Y within viewport
                    mainCamera.nearClipPlane // Z depth
                );

                // Convert the viewport position to world position
                Vector3 spawnPosition = mainCamera.ViewportToWorldPoint(viewportPos);
                spawnPosition.z = 0f; // Ensure the spawn position is on the 2D plane

                // Check distance constraints and obstacle-free status
                float distanceToPlayer = Vector3.Distance(spawnPosition, playerTransform.position);

                if (distanceToPlayer >= minDistanceToPlayer &&
                    distanceToPlayer <= maxDistanceToPlayer &&
                    !IsPositionOccupiedByObstacle(spawnPosition) &&
                    !IsPositionTooCloseToOtherEnemies(spawnPosition))
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

    private void OnDrawGizmos()
    {
        if (Application.isPlaying && mainCamera != null)
        {
            Gizmos.color = Color.yellow;
            Vector3 bottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0f, 0f, mainCamera.nearClipPlane));
            Vector3 topRight = mainCamera.ViewportToWorldPoint(new Vector3(1f, 1f, mainCamera.nearClipPlane));
            Vector3 size = topRight - bottomLeft;

            Gizmos.DrawWireCube(bottomLeft + size / 2, size);
        }

        if (Application.isPlaying && playerTransform != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(playerTransform.position, minDistanceToPlayer); // Minimum spawn distance
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(playerTransform.position, maxDistanceToPlayer); // Maximum spawn distance
        }
    }
}
