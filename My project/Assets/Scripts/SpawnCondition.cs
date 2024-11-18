using UnityEngine;

public class SpawnCondition : MonoBehaviour
{
    public GameObject prefabToSpawn;  // The prefab to spawn
    public string[] tagsToCheck;      // Array of tags to check for

    private bool hasSpawned = false;  // To track if the prefab has already been spawned

    void Update()
    {
        // Only check and spawn if the prefab has not been spawned already
        if (!hasSpawned)
        {
            CheckAndSpawn();
        }
    }

    void CheckAndSpawn()
    {
        // Iterate through all the tags
        foreach (string tag in tagsToCheck)
        {
            // Look for any object with this tag
            GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag(tag);

            // If any object with the current tag is found, do nothing and exit
            if (taggedObjects.Length > 0)
            {
                return;
            }
        }

        // If no objects with the specified tags are found, spawn the prefab
        if (prefabToSpawn != null)
        {
            Instantiate(prefabToSpawn, transform.position, Quaternion.identity);
            hasSpawned = true;  // Set flag to prevent spawning again
        }
        else
        {
            Debug.LogWarning("Prefab to spawn is not assigned!");
        }
    }
}
