using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject projectilePrefab;  // Reference to your projectile prefab
    public Transform shootingPoint;       // Reference to the ShootingPoint GameObject
    public float projectileSpeed = 10f;   // Speed of the projectile

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))  // Default Unity input for shooting (usually "Ctrl" or mouse button)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // Instantiate the projectile
        GameObject projectile = Instantiate(projectilePrefab, shootingPoint.position, Quaternion.identity);

        // Calculate the direction based on player movement or last direction
        Vector2 shootingDirection = GetShootingDirection();
        if (shootingDirection == Vector2.zero)
        {
            shootingDirection = Vector2.right; // Default direction if none is set
        }

        // Set the projectile's velocity
        Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();
        projectileRb.velocity = shootingDirection * projectileSpeed;

        // Optionally, destroy the projectile after some time
        Destroy(projectile, 5f);
    }

    Vector2 GetShootingDirection()
    {
        playermovement playerMovement = GetComponent<playermovement>();
        Vector2 lastDirection = playerMovement.GetLastDirection();

        // Debug logging for lastDirection
        Debug.Log("Last Direction: " + lastDirection);

        // Use the last direction as it is
        return lastDirection != Vector2.zero ? lastDirection : Vector2.right; // Default to right if no last direction
    }
}
