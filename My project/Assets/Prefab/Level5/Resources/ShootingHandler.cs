using System.Collections;
using UnityEngine;

public class ShootingHandler : MonoBehaviour
{
    [Header("Prefab and Timer Settings")]
    public GameObject prefabToSpawn; // The prefab to spawn as a child
    public float spawnDuration = 2f; // How long the prefab remains active

    [Header("Sprite and Animator Settings")]
    public Sprite temporarySprite; // The sprite to set during the prefab's lifetime

    private Sprite originalSprite; // To store the original sprite
    private SpriteRenderer spriteRenderer; // SpriteRenderer of the parent object
    private Animator animator; // Animator of the parent object

private void Start()
{
    Debug.Log("Initializing PrefabSpawner...");
    
    spriteRenderer = GetComponent<SpriteRenderer>();
    if (spriteRenderer == null)
    {
        Debug.LogError("SpriteRenderer component not found on this GameObject.");
    }
    else
    {
        originalSprite = spriteRenderer.sprite;
        Debug.Log("Original sprite captured successfully.");
    }

    animator = GetComponent<Animator>();
    if (animator == null)
    {
        Debug.LogError("Animator component not found on this GameObject.");
    }
    else
    {
        Debug.Log("Animator component found successfully.");
    }

    if (prefabToSpawn == null)
    {
        Debug.LogError("Prefab to spawn is not assigned.");
    }
    else
    {
        Debug.Log("Prefab assigned successfully.");
    }

    if (prefabToSpawn != null && spriteRenderer != null && animator != null)
    {
        Debug.Log("All checks passed, starting the coroutine.");
        StartCoroutine(SpawnPrefabLoop());
    }
    else
    {
        Debug.LogError("Missing required references. Coroutine not started.");
    }
}


    private IEnumerator SpawnPrefabLoop()
    {
        Debug.Log("Entering spawn loop...");
        while (true)
        {
            Debug.Log("Spawning prefab and updating parent properties...");
            // Disable the animator and change the sprite
            animator.enabled = false;
            spriteRenderer.sprite = temporarySprite;

            // Spawn the prefab as a child
            GameObject spawnedPrefab = Instantiate(prefabToSpawn, transform);

            // Wait for the spawn duration
            yield return new WaitForSeconds(spawnDuration);

            // Destroy the spawned prefab
            Destroy(spawnedPrefab);

            // Revert the sprite and re-enable the animator
            spriteRenderer.sprite = originalSprite;
            animator.enabled = true;

            // Wait before restarting the loop (optional, set to spawnDuration if no extra delay is needed)
            yield return new WaitForSeconds(spawnDuration);
        }
    }
}
