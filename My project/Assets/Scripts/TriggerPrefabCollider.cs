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
            // Instantiate the prefab at the trigger's position
            spawnedPrefab = Instantiate(prefabToSpawn, transform.position, transform.rotation);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Check if the object exiting the trigger is the player
        if (other.CompareTag("Player") && spawnedPrefab != null)
        {
            // Destroy the instantiated prefab
            Destroy(spawnedPrefab);
            spawnedPrefab = null;
        }
    }
}
