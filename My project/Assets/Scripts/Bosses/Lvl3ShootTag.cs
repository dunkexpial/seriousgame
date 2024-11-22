using System.Collections;
using UnityEngine;

public class Lvl3ShootTag : MonoBehaviour
{
    [Header("Shooting Settings")]
    public GameObject projectilePrefab;
    public float minShootDelay = 1f;
    public float maxShootDelay = 3f;
    public float projectileSpeed = 10f;

    [Header("Spawn Settings")]
    public GameObject spawnPrefab;
    public string spawnerTag = "Spawner"; // Assuming spawners are tagged as "Spawner"
    
    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; // Get player transform
        StartCoroutine(ShootRoutine());
    }

    private IEnumerator ShootRoutine()
    {
        while (true)
        {
            // Wait for a random delay
            float delay = Random.Range(minShootDelay, maxShootDelay);
            yield return new WaitForSeconds(delay);

            // Find the closest spawner
            GameObject closestSpawner = GetClosestSpawner();

            if (closestSpawner != null)
            {
                // Shoot a projectile at the closest spawner
                ShootAtTarget(closestSpawner);
            }
        }
    }

    private GameObject GetClosestSpawner()
    {
        GameObject[] spawners = GameObject.FindGameObjectsWithTag(spawnerTag);
        
        if (spawners.Length == 0) return null;

        GameObject closestSpawner = null;
        float closestDistance = Mathf.Infinity;

        // Loop through all spawners and find the closest one to the player
        foreach (GameObject spawner in spawners)
        {
            float distance = Vector2.Distance(player.position, spawner.transform.position);
            if (distance < closestDistance)
            {
                closestSpawner = spawner;
                closestDistance = distance;
            }
        }

        return closestSpawner;
    }

    private void ShootAtTarget(GameObject target)
    {
        if (projectilePrefab == null || target == null) return;

        // Instantiate the projectile
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        // Calculate the direction to the target (2D)
        Vector2 direction = (target.transform.position - transform.position).normalized;

        // Assign the direction and speed to the projectile
        Lvl3SpawnProjectile projectileScript = projectile.GetComponent<Lvl3SpawnProjectile>();
        if (projectileScript != null)
        {
            projectileScript.direction = direction;
            projectileScript.speed = projectileSpeed;
            projectileScript.spawnPrefab = spawnPrefab;
        }
    }
}
