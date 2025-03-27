using System.Collections;
using UnityEngine;

public class HeadProjectileExtra : MonoBehaviour
{
    public float speed = 5f;  // Default speed of the projectile
    public float increasedSpeed = 8f;  // Speed after changing direction
    public float detectionRadius = 3f;  // Distance at which projectile targets player
    private bool hasUpdatedDirection = false;  // To ensure direction is only updated once
    private Transform playerTransform;
    private PlayerCollision playerCollision;  // Reference to check player invincibility
    private Vector2 direction;

    public GameObject childPrefab;  // Prefab to instantiate as a child
    private Transform particleSystemChild;  // Reference to the particle system child

    private Rigidbody2D rb;  // Rigidbody2D reference for movement

    public float shootingInaccuracy = 0.1f;  // Inaccuracy factor (higher value = more inaccuracy)
    private float difficulty;
    private float reverseDifficulty;

    void Start()
    {
        // Find the player (set by the BossShooting script)
        playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
        playerCollision = playerTransform?.GetComponent<PlayerCollision>();
        difficulty = PlayerPrefs.GetFloat("Difficulty");
        reverseDifficulty = PlayerPrefs.GetFloat("ReverseDifficulty");

        // Get the particle system child object (assuming it's the first child)
        particleSystemChild = transform.GetChild(0);

        // Initialize the Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();

        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component not found on the projectile!");
            return;
        }

        // If the player is found, calculate the direction to the player and apply the velocity
        if (playerTransform != null)
        {
            // Set initial direction towards the player
            direction = (playerTransform.position - transform.position).normalized;

            // Add some inaccuracy to the direction
            direction = AddInaccuracy(direction);

            // Apply the velocity towards the player immediately
            rb.velocity = direction * (speed * Mathf.Pow(difficulty, 0.5f));

            // Debugging log to check initial velocity
            Debug.Log("Initial velocity set to: " + rb.velocity);
        }
    }

    void Update()
    {
        if (playerTransform != null && !hasUpdatedDirection)
        {
            // Check if the player is within detection radius
            float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
            if (distanceToPlayer <= detectionRadius)
            {
                // Update direction to point towards the player
                direction = (playerTransform.position - transform.position).normalized;

                // Add inaccuracy to the direction
                direction = AddInaccuracy(direction);

                speed = increasedSpeed * Mathf.Pow(difficulty, 0.5f);  // Change speed when detecting player
                hasUpdatedDirection = true;  // Ensure this only happens once

                // Instantiate the child object
                SpawnChildObject();

                // Apply velocity towards player
                if (rb != null)
                {
                    rb.velocity = direction * speed;  // Apply new velocity
                }

                // Debugging log to check the updated velocity
                Debug.Log("Velocity updated towards player: " + rb.velocity);
            }
        }
    }

    public void SetDirection(Vector2 initialDirection)
    {
        direction = initialDirection.normalized;

        // Add inaccuracy to the direction
        direction = AddInaccuracy(direction);

        // Apply initial velocity
        if (rb != null)
        {
            rb.velocity = direction * speed;  // Apply velocity immediately
        }

        // Debugging log to check initial direction
        Debug.Log("Initial direction set to: " + direction);
    }

    private void SpawnChildObject()
    {
        // Instantiate the child prefab as a child of the projectile
        GameObject child = Instantiate(childPrefab, transform.position, Quaternion.identity);

        // Set the parent to be the first child of the projectile
        if (particleSystemChild != null) // Ensure the particle system child exists
        {
            child.transform.SetParent(particleSystemChild); // Set the particle system as the parent
        }
    }

    private Vector2 AddInaccuracy(Vector2 originalDirection)
    {
        // Add a random inaccuracy to the direction
        float randomX = Random.Range(-shootingInaccuracy, shootingInaccuracy);
        float randomY = Random.Range(-shootingInaccuracy, shootingInaccuracy);

        // Apply inaccuracy by adding a random offset
        Vector2 inaccurateDirection = new Vector2(originalDirection.x + randomX, originalDirection.y + randomY);

        // Normalize the direction to maintain consistent speed
        return inaccurateDirection.normalized;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player") && playerCollision != null)
        {
            // Check if the player is invincible
            if (playerCollision.isInvincible)
            {
                DetachParticles();
                Destroy(gameObject);  // Destroy the projectile even if player is invincible
                return;
            }
        }

        // Handle collisions with obstacles and the player
        if (collider.CompareTag("ProjObstacle"))
        {
            DetachParticles();
            Destroy(gameObject);  // Destroy on obstacle hit
        }
    }

    private void DetachParticles()
    {
        if (particleSystemChild != null)
        {
            particleSystemChild.parent = null;
            particleSystemChild.localScale = Vector3.one; // Reset scale if necessary
            Destroy(particleSystemChild.gameObject, 2f); // Optional cleanup time
        }
    }
}
