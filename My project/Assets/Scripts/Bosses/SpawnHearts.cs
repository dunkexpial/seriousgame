using UnityEngine;

public class SpawnHearts : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject[] prefabs; // Array of prefabs to spawn
    public float spawnInterval = 10f; // Time in seconds between spawns

    [Header("Spawn Area")]
    public Vector2 areaSize = new Vector2(560f, 300f); // Width and height of the spawn area
    public Color gizmoColor = new Color(0f, 1f, 0f, 0.3f); // Gizmo color in the editor

    private float timer;

    private void Start()
    {
        timer = spawnInterval;
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            SpawnPrefab();
            timer = spawnInterval; // Reset timer
        }
    }

    private void SpawnPrefab()
    {
        if (prefabs != null && prefabs.Length > 0)
        {
            while (true)
            {
                // Random position within the spawn area
                Vector2 spawnPosition = new Vector2(
                    Random.Range(-areaSize.x / 2, areaSize.x / 2),
                    Random.Range(-areaSize.y / 2, areaSize.y / 2)
                );

                // Check for collisions with "Obstacle" or "ProjObstacle"
                Collider2D hit = Physics2D.OverlapPoint((Vector2)transform.position + spawnPosition);

                if (hit == null || (hit.CompareTag("Obstacle") == false && hit.CompareTag("ProjObstacle") == false))
                {
                    // Pick a random prefab from the array
                    GameObject prefabToSpawn = prefabs[Random.Range(0, prefabs.Length)];

                    // Spawn it
                    Instantiate(prefabToSpawn, (Vector2)transform.position + spawnPosition, Quaternion.identity);
                    return;
                }
            }
        }
        else
        {
            Debug.LogWarning("No prefabs assigned in SpawnHearts!");
        }
    }

    private void OnDrawGizmos()
    {
        // Visualize the spawn area in the editor
        Gizmos.color = gizmoColor;
        Gizmos.DrawCube(transform.position, new Vector3(areaSize.x, areaSize.y, 0f));
    }
}
