using UnityEngine;
using System;

public class Lvl3BossLogic : MonoBehaviour
{
    public string targetTag = "TeleportTarget"; // The tag of the target objects
    public GameObject teleportEffectPrefab; // The prefab to leave behind when teleporting
    public float baseIntervalTime = 2f; // The base time interval for teleportation
    public float intervalRandomnessPercentage = 0.5f; // Randomness percentage to adjust the interval time
    public float stopRangeX = 5f; // Range within which the boss affects projectiles along the X-axis
    public float stopRangeY = 3f; // Range within which the boss affects projectiles along the Y-axis
    public Vector3 stopRangeOffset = Vector3.zero; // Offset for the center of the stop range
    public string scriptNameToAdd1 = "ProjectileBehavior1"; // Name of the first script (component) to add to projectiles
    public string scriptNameToAdd2 = "ProjectileBehavior2"; // Name of the second script (component) to add to projectiles
    public string scriptNameToRemove = "ProjectileOldBehavior"; // Name of the script (component) to remove from projectiles
    public GameObject prefabToSpawn; // The prefab to spawn as a child of the projectile

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

        InvokeRepeating(nameof(Teleport), 0f, GetRandomizedIntervalTime());
    }

    void Update()
    {
        // Check if there are any objects with the target tag
        GameObject[] targetObjects = GameObject.FindGameObjectsWithTag(targetTag);

        // Access the AttributesManager component
        var attributesManager = GetComponent<AttributesManager>();
        if (attributesManager != null && targetObjects.Length <= 0)
        {
            attributesManager.health = targetObjects.Length == 0 ? 0 : attributesManager.health;
            attributesManager.itemDrop();
            Destroy(gameObject);
        }

        // Existing logic for handling projectiles
        GameObject[] projectiles = GameObject.FindGameObjectsWithTag("ProjectileTag");

        foreach (GameObject projectile in projectiles)
        {
            if (projectile.GetComponent<ProjectilePrefabFlag>() == null)
            {
                Vector3 delta = projectile.transform.position - (transform.position + stopRangeOffset);
                float normalizedX = delta.x / stopRangeX;
                float normalizedY = delta.y / stopRangeY;

                // Check if within elliptical range
                if ((normalizedX * normalizedX + normalizedY * normalizedY) <= 1f)
                {
                    Debug.Log("Projectile within stop range: " + projectile.name);

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
                        projectile.AddComponent<ProjectilePrefabFlag>();
                    }
                }
            }
        }
    }

    void Teleport()
    {
        // Find all objects with the target tag
        GameObject[] targetObjects = GameObject.FindGameObjectsWithTag(targetTag);

        // If there are no objects with the tag, exit early
        if (targetObjects.Length == 0) return;

        // Filter out the object the boss is already on
        GameObject currentTarget = null;
        foreach (GameObject obj in targetObjects)
        {
            if (obj.transform.position == transform.position)
            {
                currentTarget = obj;
                break;
            }
        }

        // If the only available object is the current one, do nothing
        if (targetObjects.Length == 1 && currentTarget != null) return;

        // Choose a random target object that isn't the current one
        GameObject newTarget;
        do
        {
            newTarget = targetObjects[UnityEngine.Random.Range(0, targetObjects.Length)];
        } while (newTarget.transform.position == transform.position);

        // Save the current position of the boss
        Vector3 oldPosition = transform.position;

        // Move the boss to the new target's position
        transform.position = newTarget.transform.position;

        // Spawn the teleport effect at the old position (where the boss was)
        if (teleportEffectPrefab != null)
        {
            Instantiate(teleportEffectPrefab, oldPosition, Quaternion.identity);
        }

        // Recalculate the next random interval time
        float nextInterval = GetRandomizedIntervalTime();
        CancelInvoke(nameof(Teleport)); // Cancel the current invoke
        InvokeRepeating(nameof(Teleport), nextInterval, nextInterval); // Reinvoke with the new interval
    }

    float GetRandomizedIntervalTime()
    {
        return baseIntervalTime + UnityEngine.Random.Range(-baseIntervalTime * intervalRandomnessPercentage, baseIntervalTime * intervalRandomnessPercentage);
    }

    private void OnDrawGizmos()
    {
        // Visualize elliptical range
        Gizmos.color = Color.red;
        Gizmos.matrix = Matrix4x4.TRS(transform.position + stopRangeOffset, Quaternion.identity, new Vector3(stopRangeX, stopRangeY, 1));
        Gizmos.DrawWireSphere(Vector3.zero, 1f); // Scaled to ellipse
    }
}

// Custom flag class
public class ProjectilePrefabFlag : MonoBehaviour { }
