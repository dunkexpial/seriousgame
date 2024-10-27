using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobSpawner : MonoBehaviour
{
    [SerializeField] private float spawnRateMin = 0.1f;
    [SerializeField] private float spawnRateMax = 10f;
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private bool canSpawn = true;
    [SerializeField] private float spawnRangeX = 30f;
    [SerializeField] private float spawnRangeY = 30f;
    [SerializeField] private float checkRadius = 150f;
    [SerializeField] private int maxEnemiesInRadius = 5;
    [SerializeField] private float minDistanceToPlayer = 30f; // Minimum distance to player for spawning

    void Start()
    {
        StartCoroutine(Spawner());
    }

    private IEnumerator Spawner()
    {
        while (canSpawn)
        {
            float randomSpawnRate = Random.Range(spawnRateMin, spawnRateMax);
            WaitForSeconds wait = new WaitForSeconds(randomSpawnRate);

            yield return wait;

            // Check for existing enemies within the defined radius
            Collider2D[] nearbyEnemies = Physics2D.OverlapCircleAll(transform.position, checkRadius);
            int enemyCount = 0;

            // Count only objects with the "Enemy" tag
            foreach (var obj in nearbyEnemies)
            {
                if (obj.CompareTag("Enemy"))
                {
                    enemyCount++;
                }
            }

            if (enemyCount <= maxEnemiesInRadius)
            {
                Vector3 randomOffset = new Vector3(
                    Random.Range(-spawnRangeX, spawnRangeX),
                    Random.Range(-spawnRangeY, spawnRangeY),
                    0f
                );

                Vector3 spawnPosition = transform.position + randomOffset;

                // Check if the spawn position is too close to the player
                Collider2D playerCheck = Physics2D.OverlapCircle(spawnPosition, minDistanceToPlayer, LayerMask.GetMask("Player"));
                if (playerCheck == null && !IsPositionOccupiedByObstacle(spawnPosition))
                {
                    int rand = Random.Range(0, enemyPrefabs.Length);
                    GameObject enemyToSpawn = enemyPrefabs[rand];

                    Instantiate(enemyToSpawn, spawnPosition, Quaternion.identity);
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
}
