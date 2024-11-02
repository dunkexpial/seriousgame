using System.Collections;
using UnityEngine;

public class Lvl1BossHeadShooting : MonoBehaviour
{
    public GameObject[] projectilePrefabs;  // Array of projectile prefabs to shoot
    public float shootingInterval = 0.5f; // Time between individual shots
    public float startAngle = 0f; // Starting angle in degrees
    public float endAngle = 90f; // Ending angle in degrees
    public float shootingDuration = 5f; // Total duration for which shooting occurs
    public string targetTag = "None"; // The tag of the target to check for

    private Coroutine shootingCoroutine; // Store the current shooting coroutine
    private float shootingEndTime; // Store the time when shooting should stop

    private void Update()
    {
        // Check for objects with the specified tag
        if (GameObject.FindWithTag(targetTag) == null)
        {
            // If no object with the target tag is found and not currently shooting, start the shooting event
            if (shootingCoroutine == null)
            {
                shootingEndTime = Time.time + shootingDuration; // Set the end time for shooting
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
        while (Time.time < shootingEndTime) // Continue shooting until the timer expires
        {
            ShootProjectile(); // Shoot a projectile
            yield return new WaitForSeconds(shootingInterval); // Wait for the next shot
        }

        shootingCoroutine = null; // Reset the coroutine reference when finished
    }

    private void ShootProjectile()
    {
        // Generate a random angle between startAngle and endAngle
        float randomAngle = Random.Range(startAngle, endAngle);

        // Select a random projectile prefab from the array
        GameObject projectilePrefab = projectilePrefabs[Random.Range(0, projectilePrefabs.Length)];

        // Instantiate the projectile
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.Euler(0, 0, randomAngle));
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

        // Calculate the direction based on the random angle
        Vector2 direction = Quaternion.Euler(0, 0, randomAngle) * Vector2.right; // Adjust direction based on angle
        rb.velocity = direction * 100f; // Set the velocity (100f can be adjusted based on desired speed)

        // Optionally, you can destroy the projectile after a certain time
        Destroy(projectile, 5f); // Destroy after 5 seconds
    }
}
