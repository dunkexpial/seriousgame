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
            // Randomize the interval between spawns
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
                continue; // Stop this spawn attempt and wait for the next interval
            }

            // Try to spawn until a valid position is found
            bool spawnSuccessful = false;

            while (!spawnSuccessful)
            {
                // Randomize a spawn offset within the defined range
                Vector3 randomOffset = new Vector3(
                    Random.Range(-spawnRangeX, spawnRangeX),
                    Random.Range(-spawnRangeY, spawnRangeY),
                    0f
                );

                Vector3 spawnPosition = transform.position + randomOffset;

                // Check if the spawn position is within the valid distance range from the player
                float distanceToPlayer = Vector3.Distance(spawnPosition, playerTransform.position);

                if (distanceToPlayer >= minDistanceToPlayer &&
                    distanceToPlayer <= maxDistanceToPlayer &&
                    !IsPositionOccupiedByObstacle(spawnPosition))
                {
                    int rand = Random.Range(0, enemyPrefabs.Length);
                    GameObject enemyToSpawn = enemyPrefabs[rand];

                    Instantiate(enemyToSpawn, spawnPosition, Quaternion.identity);
                    spawnSuccessful = true; // Exit the loop after a successful spawn
                }

                // Yield for a frame to avoid freezing the game while searching for a valid position
                yield return null;
            }
        }
    }

    private bool IsPositionOccupiedByObstacle(Vector3 position)
    {
        // Find all colliders within the detection radius
        Collider2D[] obstacles = Physics2D.OverlapCircleAll(position, obstacleDetectionRadius);
        
        foreach (var obstacle in obstacles)
        {
            // Check for "Obstacle" or "ProjObstacle" tags
            if (obstacle.CompareTag("Obstacle") || obstacle.CompareTag("ProjObstacle"))
            {
                return true;
            }

            // Check for "safezone" with isTrigger enabled
            if (obstacle.CompareTag("Safezone") && obstacle.isTrigger)
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
