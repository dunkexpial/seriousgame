using System.Collections;
using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] prefabs; // Array of prefabs to spawn
    [SerializeField] private float spawnDelay = 2f; // Delay before spawning
    [SerializeField] private float destroyDelay = 5f; // Delay before destroying the spawner
    [SerializeField] private bool isBoss = false; // Is this a boss spawner?
    [SerializeField] private float spawnDistance = 10f; // Distance threshold for boss spawn

    private Transform playerTransform;

    void Start()
    {
        // Find the player object by tag
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogWarning("Player not found! Make sure the player has the 'Player' tag.");
        }

        StartCoroutine(SpawnRandomObject());
    }

    private IEnumerator SpawnRandomObject()
    {
        // Wait for the specified spawn delay before spawning the first object
        yield return new WaitForSeconds(spawnDelay);

        // Check if there are any prefabs in the array
        if (prefabs.Length > 0)
        {
            if (isBoss && playerTransform != null)
            {
                // Wait until player is within the spawn distance
                while (Vector3.Distance(transform.position, playerTransform.position) > spawnDistance)
                {
                    yield return null; // Check again in the next frame
                }
            }

            // Spawn the object once the conditions are met
            SpawnPrefab();
        }

        // Wait for the specified destroy delay before destroying the spawner
        yield return new WaitForSeconds(destroyDelay);

        // Destroy the spawner itself
        Destroy(gameObject);
    }

    private void SpawnPrefab()
    {
        // Choose a random prefab from the array
        int randomIndex = Random.Range(0, prefabs.Length);
        GameObject prefabToSpawn = prefabs[randomIndex];

        // Instantiate the prefab at the spawner's position with no rotation
        Instantiate(prefabToSpawn, transform.position, Quaternion.identity);
    }
}
