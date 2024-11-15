using System.Collections;
using UnityEngine;

public class Lvl1BossHeadShooting : MonoBehaviour
{
    public GameObject[] projectilePrefabs;  // Array of projectile prefabs to shoot
    public float shootingInterval = 0.5f; // Time between individual shots
    public float startAngle = 0f; // Starting angle in degrees
    public float endAngle = 90f; // Ending angle in degrees
    public float projectileLifetime = 5f; // Lifetime of the projectile
    public string targetTag = "None"; // The tag of the target to check for

    private Coroutine shootingCoroutine; // Store the current shooting coroutine

    private void Update()
    {
        // Check for objects with the specified tag
        if (GameObject.FindWithTag(targetTag) == null)
        {
            // If no object with the target tag is found and not currently shooting, start the shooting event
            if (shootingCoroutine == null)
            {
                shootingCoroutine = StartCoroutine(ShootingEvent());
            }
        }
        else
        {
            // If the target is found, stop shooting and reset the coroutine reference
            if (shootingCoroutine != null)
            {
                StopCoroutine(shootingCoroutine);
                shootingCoroutine = null; // Reset the coroutine reference
            }
        }
    }

    private IEnumerator ShootingEvent()
    {
        while (true) // Continue shooting indefinitely
        {
            ShootProjectile(); // Shoot a projectile
            yield return new WaitForSeconds(shootingInterval); // Wait for the next shot
        }
    }

    private void ShootProjectile()
    {
        // Generate a random angle between startAngle and endAngle
        float randomAngle = Random.Range(startAngle, endAngle);

        // Select a random projectile prefab from the array
        GameObject projectilePrefab = projectilePrefabs[Random.Range(0, projectilePrefabs.Length)];

        // Instantiate the projectile
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        // Calculate the direction based on the random angle
        Vector2 direction = Quaternion.Euler(0, 0, randomAngle) * Vector2.right;

        // Apply the direction to the projectile's Rigidbody2D if needed or store it in the projectile script
        HeadProjectileExtra projectileScript = projectile.GetComponent<HeadProjectileExtra>();
        if (projectileScript != null)
        {
            projectileScript.SetDirection(direction); // Add a method in HeadProjectileExtra to set initial direction
        }

        // Destroy the projectile after the specified lifetime
        Destroy(projectile, projectileLifetime);
    }
}
