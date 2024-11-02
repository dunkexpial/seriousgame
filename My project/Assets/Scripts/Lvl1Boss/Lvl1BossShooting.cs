using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lvl1BossShooting : MonoBehaviour
{
    public GameObject[] projectiles; // Array of projectile prefabs
    public float baseShootingInterval = 1f; // Base time between shots
    public float shootingIntervalVariance = 0.5f; // Variance for shooting interval
    private float nextShootTime; // Time when the next shot can be fired

    void Start()
    {
        nextShootTime = Time.time; // Initialize the next shoot time
    }

    void Update()
    {
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
        int projectileIndex = Random.Range(0, projectiles.Length);
        GameObject projectilePrefab = projectiles[projectileIndex];

        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        EnemyProjectile enemyProjectile = projectile.GetComponent<EnemyProjectile>();
        if (enemyProjectile != null)
        {
            enemyProjectile.player = GameObject.FindGameObjectWithTag("Player");

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
