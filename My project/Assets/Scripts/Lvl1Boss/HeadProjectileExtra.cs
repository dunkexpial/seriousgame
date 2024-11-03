using System.Collections;
using UnityEngine;

public class HeadProjectileExtra : MonoBehaviour
{
    public float speed = 5f;  // Default speed of the projectile
    public float increasedSpeed = 8f;  // Speed after changing direction
    public float detectionRadius = 3f;  // Distance at which projectile targets player
    private bool hasUpdatedDirection = false;  // To ensure direction is only updated once
    private Transform playerTransform;
    private Vector2 direction;

    public GameObject childPrefab;  // Prefab to instantiate as a child

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (!hasUpdatedDirection && playerTransform != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

            // Check if the player is within detection radius
            if (distanceToPlayer <= detectionRadius)
            {
                // Update direction to point towards the player
                direction = (playerTransform.position - transform.position).normalized;
                speed = increasedSpeed;  // Change speed
                hasUpdatedDirection = true;  // Ensure this only happens once

                // Instantiate the child object
                SpawnChildObject();
            }
        }

        // Move the projectile in the current direction
        transform.position += (Vector3)direction * speed * Time.deltaTime;
    }

    public void SetDirection(Vector2 initialDirection)
    {
        direction = initialDirection.normalized;
    }

    private void SpawnChildObject()
    {
        // Instantiate the child prefab as a child of the projectile
        GameObject child = Instantiate(childPrefab, transform.position, Quaternion.identity);
        child.transform.SetParent(transform); // Set the projectile as the parent
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("ProjObstacle"))
        {
            Destroy(gameObject);
        }
    }
}
