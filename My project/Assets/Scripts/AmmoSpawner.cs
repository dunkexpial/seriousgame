using UnityEngine;

public class AmmoSpawner : MonoBehaviour
{
    [SerializeField] private int ammoCount = 10; // Number of ammo pickups to spawn
    [SerializeField] private GameObject[] ammoPrefabs; // List of ammo prefabs
    [SerializeField] private float spawnRangeX = 30f; // Horizontal spawn area
    [SerializeField] private float spawnRangeY = 30f; // Vertical spawn area
    [SerializeField] private float obstacleDetectionRadius = 2f; // Radius for obstacle detection

    void Start()
    {
        SpawnAllAmmo();
        Destroy(gameObject); // Destroy this spawner after spawning
    }

    private void SpawnAllAmmo()
    {
        int spawned = 0;
        int safetyCounter = 0; // Prevents infinite loops

        int gridSize = Mathf.CeilToInt(Mathf.Sqrt(ammoCount)); // Number of slots per axis
        float stepX = (spawnRangeX * 2) / gridSize;
        float stepY = (spawnRangeY * 2) / gridSize;

        while (spawned < ammoCount && safetyCounter < ammoCount * 50)
        {
            safetyCounter++;

            // Pick a grid cell
            int gx = spawned % gridSize;
            int gy = spawned / gridSize;

            // Base grid position
            float posX = -spawnRangeX + (gx + 0.5f) * stepX;
            float posY = -spawnRangeY + (gy + 0.5f) * stepY;
            Vector3 spawnPosition = new Vector3(posX, posY, 0f) + transform.position;

            // Add small random offset so it's not perfectly aligned
            spawnPosition.x += Random.Range(-stepX * 0.25f, stepX * 0.25f);
            spawnPosition.y += Random.Range(-stepY * 0.25f, stepY * 0.25f);

            // If blocked, keep retrying in random positions until we succeed
            int retries = 0;
            while (IsPositionOccupiedByObstacle(spawnPosition) && retries < 20)
            {
                float randomX = Random.Range(-spawnRangeX, spawnRangeX);
                float randomY = Random.Range(-spawnRangeY, spawnRangeY);
                spawnPosition = new Vector3(randomX, randomY, 0f) + transform.position;
                retries++;
            }

            if (!IsPositionOccupiedByObstacle(spawnPosition))
            {
                int rand = Random.Range(0, ammoPrefabs.Length);
                GameObject ammoToSpawn = ammoPrefabs[rand];
                Instantiate(ammoToSpawn, spawnPosition, Quaternion.identity);
                spawned++;
            }
        }

        if (spawned < ammoCount)
        {
            Debug.LogWarning($"Requested {ammoCount} ammo, but only spawned {spawned} due to obstacles.");
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

    private void OnDrawGizmos()
    {
        // Draw spawn area (blue box)
        Gizmos.color = Color.blue;
        Vector3 spawnAreaCenter = transform.position;
        Vector3 spawnAreaSize = new Vector3(spawnRangeX * 2, spawnRangeY * 2, 0f);
        Gizmos.DrawWireCube(spawnAreaCenter, spawnAreaSize);
    }
}
