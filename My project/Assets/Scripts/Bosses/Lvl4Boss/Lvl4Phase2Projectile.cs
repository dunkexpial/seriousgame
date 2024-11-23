using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lvl4Phase2Projectile : MonoBehaviour
{
    public float speed;
    public float lifespan = 5f;
    private Rigidbody2D rb;
    private Vector3 direction; // The direction the projectile will move in

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Ensure the direction is normalized when set externally
        rb.velocity = direction * speed;

        // Calculate the rotation to face the direction of movement
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // Destroy the projectile after its lifespan expires
        Destroy(gameObject, lifespan);
    }

    // Method to set the direction from the boss when the projectile is instantiated
    public void SetDirection(Vector3 newDirection)
    {
        direction = newDirection.normalized; // Normalize the direction to ensure consistent movement
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("ProjObstacle"))
        {
            // Only detach particles here if the projectile hits an obstacle
            DetachParticles();
            Destroy(gameObject); // Destroy on obstacle hit
        }
    }

    private void DetachParticles()
    {
        Transform particleSystemChild = transform.childCount > 0 ? transform.GetChild(0) : null;
        if (particleSystemChild != null)
        {
            particleSystemChild.parent = null;
            particleSystemChild.localScale = Vector3.one;
            Destroy(particleSystemChild.gameObject, 2f);
        }
    }
}
