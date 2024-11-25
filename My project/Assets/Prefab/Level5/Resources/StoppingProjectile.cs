using UnityEngine;
using System;

public class StoppingProjectile : MonoBehaviour
{
    public string scriptNameToAdd1 = "ProjectileBehavior1"; // Name of the first script (component) to add to projectiles
    public string scriptNameToAdd2 = "ProjectileBehavior2"; // Name of the second script (component) to add to projectiles
    public string scriptNameToRemove = "ProjectileOldBehavior"; // Name of the script (component) to remove from projectiles
    public GameObject prefabToSpawn; // The prefab to spawn as a child of the projectile

    public float stopRangeX = 5f; // X range for the elliptical area (can be adjusted in the Inspector)
    public float stopRangeY = 5f; // Y range for the elliptical area (can be adjusted in the Inspector)

    private Type scriptTypeToAdd1;
    private Type scriptTypeToAdd2;
    private Type scriptTypeToRemove;

    private void Start()
    {
        // Convert script names to Types
        scriptTypeToAdd1 = Type.GetType(scriptNameToAdd1);
        scriptTypeToAdd2 = Type.GetType(scriptNameToAdd2);
        scriptTypeToRemove = Type.GetType(scriptNameToRemove);

        // Check if the script types are found
        if (scriptTypeToAdd1 == null)
            Debug.LogError("Script to add 1 not found: " + scriptNameToAdd1);
        if (scriptTypeToAdd2 == null)
            Debug.LogError("Script to add 2 not found: " + scriptNameToAdd2);
        if (scriptTypeToRemove == null)
            Debug.LogError("Script to remove not found: " + scriptNameToRemove);
    }

    void Update()
    {
        // Loop through all projectiles in the scene and check if they are within range
        GameObject[] projectiles = GameObject.FindGameObjectsWithTag("ProjectileTag");
        foreach (GameObject projectile in projectiles)
        {
            HandleProjectile(projectile, transform.position, Vector3.zero); // You can adjust stopRangeOffset if needed
        }
    }

    public void HandleProjectile(GameObject projectile, Vector3 centerPosition, Vector3 stopRangeOffset)
    {
        if (projectile.GetComponent<StoppingProjectileFlag>() == null)
        {
            // Calculate the distance between the projectile and the center position
            Vector3 delta = projectile.transform.position - (centerPosition + stopRangeOffset);
            float normalizedX = delta.x / stopRangeX;
            float normalizedY = delta.y / stopRangeY;

            // Check if within elliptical range (normalizing based on stopRangeX and stopRangeY)
            if ((normalizedX * normalizedX + normalizedY * normalizedY) <= 1f)
            {
                // Add first script
                if (scriptTypeToAdd1 != null && projectile.GetComponent(scriptTypeToAdd1) == null)
                    projectile.AddComponent(scriptTypeToAdd1);

                // Add second script
                if (scriptTypeToAdd2 != null && projectile.GetComponent(scriptTypeToAdd2) == null)
                    projectile.AddComponent(scriptTypeToAdd2);

                // Remove old script
                if (scriptTypeToRemove != null)
                {
                    Component existingComponent = projectile.GetComponent(scriptTypeToRemove);
                    if (existingComponent != null)
                        Destroy(existingComponent);
                }

                // Spawn prefab as child
                if (prefabToSpawn != null)
                {
                    GameObject spawnedPrefab = Instantiate(prefabToSpawn, projectile.transform.position, Quaternion.identity);
                    spawnedPrefab.transform.SetParent(projectile.transform);
                    projectile.AddComponent<StoppingProjectileFlag>();
                }
            }
        }
    }
}

// Custom flag class
public class StoppingProjectileFlag : MonoBehaviour { }
