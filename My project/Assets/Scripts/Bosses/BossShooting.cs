using System.Collections;
using UnityEngine;

public class BossShooting : MonoBehaviour
{
    public GameObject[] projectiles; // Array of projectile prefabs
    public float baseShootingInterval = 1f; // Base time between shots
    public float shootingIntervalVariance = 0.5f; // Variance for shooting interval
    private float nextShootTime; // Time when the next shot can be fired
    private GameObject player; // Reference to the player
    public string ignoreTag = "IgnoreTag";  // Optional tag to ignore for starting shooting
    public bool checkForTags = true;  // Flag to enable/disable checking for objects with the ignoreTag

    void Start()
    {
        // Add a random initial delay for the first shot
        nextShootTime = Time.time + Random.Range(0, baseShootingInterval);
        player = GameObject.FindGameObjectWithTag("Player"); // Find player at the start
    }

    void Update()
    {
        // Stop shooting if no player is found
        if (player == null)
        {
            return; // Exit early if player is not found
        }

        // Only check for objects with the ignore tag if checkForTags is true
        if (checkForTags)
        {
            GameObject[] objectsToIgnore = GameObject.FindGameObjectsWithTag(ignoreTag);
            if (objectsToIgnore.Length > 0)
            {
                return; // Don't shoot if there are objects with the ignore tag
            }
        }

        // Check if it's time to shoot
        if (Time.time >= nextShootTime)
        {
            Shoot();
            // Calculate the next shooting time with some randomness
            nextShootTime = Time.time + baseShootingInterval + Random.Range(-shootingIntervalVariance, shootingIntervalVariance);
        }
    }

    void Shoot()
    {
        if (player == null) return; // Ensure the player is still found before shooting

        int projectileIndex = Random.Range(0, projectiles.Length);
        GameObject projectilePrefab = projectiles[projectileIndex];

        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        EnemyProjectile enemyProjectile = projectile.GetComponent<EnemyProjectile>();
        if (enemyProjectile != null)
        {
            enemyProjectile.player = player;

            Vector3 direction = (enemyProjectile.player.transform.position - transform.position).normalized;

            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = direction * enemyProjectile.speed;
            }

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            projectile.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
