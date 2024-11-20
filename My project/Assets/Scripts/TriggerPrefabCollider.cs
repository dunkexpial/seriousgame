using UnityEngine;

public class TriggerPrefabCollider : MonoBehaviour
{
    public GameObject prefabToSpawn; // Prefab to instantiate
    private GameObject spawnedPrefab; // Reference to the instantiated prefab

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object entering the trigger is the player (e.g., check by tag)
        if (other.CompareTag("Player") && spawnedPrefab == null)
        {
            // Find all enemies in the scene and destroy them
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            GameObject[] spawners = GameObject.FindGameObjectsWithTag("Spawner");
            foreach (GameObject enemy in enemies) 
            {
                Destroy(enemy);
            }
            foreach (GameObject spawner in spawners) 
            {
                Destroy(spawner);
            }

            // Instantiate the prefab at the trigger's position
            spawnedPrefab = Instantiate(prefabToSpawn, transform.position, transform.rotation);
        }
    }
}
