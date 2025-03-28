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
    public float teleportYOffset = 2f; // Adjust this value to set the desired Y offset
    private PlayOnCreateAndDestroy playOnCreateAndDestroy;
    private AttributesManager attributesManager;

    private Type scriptTypeToAdd1;
    private Type scriptTypeToAdd2;
    private Type scriptTypeToRemove;
    private SoundManager soundManager;

    private void Start()
    {
        // Convert script names to Types
        scriptTypeToAdd1 = Type.GetType(scriptNameToAdd1);
        scriptTypeToAdd2 = Type.GetType(scriptNameToAdd2);
        scriptTypeToRemove = Type.GetType(scriptNameToRemove);
        soundManager = FindAnyObjectByType<SoundManager>();

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
        // Get the list of objects with the target tag
        GameObject[] targetObjects = GameObject.FindGameObjectsWithTag(targetTag);

        // Access the AttributesManager component
        var attributesManager = GetComponent<AttributesManager>();
        var playOnCreateAndDestroy = GetComponent<PlayOnCreateAndDestroy>();
        if (attributesManager != null && targetObjects.Length <= 0)
        {
            attributesManager.health = targetObjects.Length == 0 ? 0 : attributesManager.health;
            attributesManager.itemDrop();
            playOnCreateAndDestroy.PlayDestroySmokeEffect();
            Destroy(gameObject);
        }

        if (targetObjects.Length > 1)
        {
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
    }

    public void Teleport()
    {
        Debug.Log("Teleporting...");

        // Find all objects with the target tag
        GameObject[] targetObjects = GameObject.FindGameObjectsWithTag(targetTag);
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.LogError("Player not found!");
            return;
        }


        // Find the closest target to the player that is NOT the boss's current position
        GameObject closestTarget = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject target in targetObjects)
        {
            if (target.transform.position == transform.position) continue; // skip if boss is already on it

            float distance = Vector3.Distance(target.transform.position, player.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestTarget = target;
            }
        }

        // If there is no valid target (could happen if the only target is under the boss)
        if (closestTarget == null)
        {
            Debug.Log("No valid teleport target found.");
            return;
        }

        // Save the current position of the boss
        Vector3 oldPosition = transform.position;

        // Move the boss to the new target's position, applying the Y offset
        Vector3 targetPosition = closestTarget.transform.position;
        soundManager.PlaySoundBasedOnCollision("teleportSound");
        transform.position = new Vector3(targetPosition.x, targetPosition.y + teleportYOffset, targetPosition.z);

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
