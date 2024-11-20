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

            // Count all "Enemy" tagged objects in the scene
            int enemyCountInScene = GameObject.FindGameObjectsWithTag("Enemy").Length;

            if (enemyCountInScene >= maxEnemiesInScene)
            {
                Debug.Log("Max enemies in scene reached: " + enemyCountInScene);
                continue;
            }

            // Define a smaller spawn area for nearby enemy checks
            Vector2 smallerSpawnArea = new Vector2(spawnRangeX * 0.8f, spawnRangeY * 0.8f);
            Collider2D[] nearbyEnemies = Physics2D.OverlapBoxAll(transform.position, smallerSpawnArea, 0f);

            int enemyCount = 0;

            foreach (var obj in nearbyEnemies)
            {
                if (obj.CompareTag("Enemy"))
                {
                    enemyCount++;
                }
            }

            if (enemyCount > maxEnemiesInRange)
            {
                Debug.Log("Too many enemies nearby: " + enemyCount);
                continue;
            }

            bool spawnSuccessful = false;
            float retryTimer = 1f; // Maximum time for retrying spawn
            float startTime = Time.time;

            while (!spawnSuccessful && Time.time - startTime < retryTimer)
            {
                Vector3 randomOffset = new Vector3(
                    Random.Range(-spawnRangeX, spawnRangeX),
                    Random.Range(-spawnRangeY, spawnRangeY),
                    0f
                );

                Vector3 spawnPosition = transform.position + randomOffset;

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
                yield return null; // Yield to prevent freezing the game
            }

            if (!spawnSuccessful)
            {
                Debug.LogWarning("Failed to find a valid spawn position after " + retryTimer + " seconds.");
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

    private void OnDrawGizmos()
    {
        // Set the gizmo color to a semi-transparent yellow
        Gizmos.color = new Color(1f, 1f, 0f, 0.1f); // RGBA (yellow, 10% opacity)
        Gizmos.DrawCube(transform.position, new Vector3(spawnRangeX * 2, spawnRangeY * 2, 0f));

        if (Application.isPlaying && playerTransform != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(playerTransform.position, minDistanceToPlayer); // Minimum spawn distance
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(playerTransform.position, maxDistanceToPlayer); // Maximum spawn distance
        }
    }
}
