using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public GameObject player;
    private Rigidbody2D rb;
    public float speed;
    public float lifespan = 5f;
    private Transform particleSystemChild;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");

        // Get the particle system child object
        particleSystemChild = transform.GetChild(0);

        // Calculate the direction from the projectile to the player
        Vector3 direction = (player.transform.position - transform.position).normalized;

        // Set the velocity of the projectile towards the player
        rb.velocity = direction * speed;

        // Calculate the rotation to face the direction of movement
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // Destroy the projectile after its lifespan
        Destroy(gameObject, lifespan);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("ProjObstacle") || collider.CompareTag("Player"))
        {
            // Detach the particle system so it stays when the projectile is destroyed
            if (particleSystemChild != null)
            {
                particleSystemChild.parent = null;
                particleSystemChild.localScale = Vector3.one; // Reset scale
                Destroy(particleSystemChild.gameObject, 2f); // Optional cleanup time
            }

            // Destroy the projectile only if it hits an obstacle
            if (collider.CompareTag("ProjObstacle"))
            {
                Destroy(gameObject);
            }
        }
    }
}
