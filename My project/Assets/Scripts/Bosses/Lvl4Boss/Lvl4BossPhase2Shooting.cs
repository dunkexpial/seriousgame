using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lvl4BossPhase2Shooting : MonoBehaviour
{
    public GameObject[] projectiles;
    public float baseShootingInterval = 1f; // Base time between shots
    public float shootingIntervalVariance = 0.5f; // Variance for shooting interval
    private float nextShootTime;
    public int projectilesPerShot = 8; // Number of projectiles to shoot per shot
    public float projectileSpeed = 5f; // Speed of the projectiles
    public float spreadVariance = 10f; // Amount of angle variation for projectile spread
    public string ignoreTag = "IgnoreTag";
    public bool checkForTags = true;

    private Lvl4BossPhase2Movement bossMovement;
    private SoundManager soundManager;

    void Start()
    {
        soundManager = FindAnyObjectByType<SoundManager>();
        // Add a random initial delay for the first shot
        nextShootTime = Time.time + Random.Range(0, baseShootingInterval);
    }

    void Update()
    {
        // Continuously check for Lvl4BossPhase2Movement component
        bossMovement = GetComponent<Lvl4BossPhase2Movement>();

        // Stop shooting if no Lvl4BossPhase2Movement component is found or canShoot is false
        if (bossMovement == null || !bossMovement.canShoot)
        {
            return; // Exit early if movement component is missing or canShoot is false
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

        if (Time.time >= nextShootTime)
        {
            Shoot();
            // Calculate the next shooting time with some randomness
            nextShootTime = Time.time + baseShootingInterval + Random.Range(-shootingIntervalVariance, shootingIntervalVariance);
        }
    }

    void Shoot()
    {
        // Calculate the angle increment based on the number of projectiles
        float angleIncrement = 360f / projectilesPerShot;

        // Add a global random offset to the entire shot cluster
        float globalRandomOffset = Random.Range(-spreadVariance, spreadVariance);

        soundManager.PlaySoundBasedOnCollision("Boss4Projectile"); 

        // Shoot the projectiles in all directions with slight randomness
        for (int i = 0; i < projectilesPerShot; i++)
        {
            // Calculate the base angle for each projectile based on its index
            float angle = i * angleIncrement;

            // Add the global random offset to each angle to spread the projectiles
            float finalAngle = angle + globalRandomOffset;

            // Convert the final angle to a direction vector (unit vector)
            Vector3 direction = new Vector3(Mathf.Cos(Mathf.Deg2Rad * finalAngle), Mathf.Sin(Mathf.Deg2Rad * finalAngle), 0).normalized;

            // Randomly choose a projectile prefab from the array
            int projectileIndex = Random.Range(0, projectiles.Length);
            GameObject projectilePrefab = projectiles[projectileIndex];

            // Instantiate the projectile
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

            // Get the Lvl4Phase2Projectile component and set the direction
            Lvl4Phase2Projectile projectileScript = projectile.GetComponent<Lvl4Phase2Projectile>();
            if (projectileScript != null)
            {
                projectileScript.SetDirection(direction); // Set the direction for the projectile
            }

            // Set the speed of the projectile
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = direction * projectileSpeed;
            }

            // Adjust the rotation of the projectile to match its movement direction
            float projectileAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            projectile.transform.rotation = Quaternion.Euler(0, 0, projectileAngle);
        }
    }
}
