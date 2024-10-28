using System.Collections;
using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] prefabs; // Array of prefabs to spawn
    [SerializeField] private float spawnDelay = 2f; // Delay before spawning
    [SerializeField] private float destroyDelay = 5f; // Delay before destroying the spawner

    void Start()
    {
        StartCoroutine(SpawnRandomObject());
    }

    private IEnumerator SpawnRandomObject()
    {
        // Wait for the specified spawn delay before spawning the first object
        yield return new WaitForSeconds(spawnDelay);

        // Check if there are any prefabs in the array
        if (prefabs.Length > 0)
        {
            // Choose a random prefab from the array
            int randomIndex = Random.Range(0, prefabs.Length);
            GameObject prefabToSpawn = prefabs[randomIndex];

            // Instantiate the prefab at the spawner's position with no rotation
            Instantiate(prefabToSpawn, transform.position, Quaternion.identity);
        }

        // Wait for the specified destroy delay before destroying the spawner
        yield return new WaitForSeconds(destroyDelay);

        // Destroy the spawner itself
        Destroy(gameObject);
    }
}
