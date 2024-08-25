using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float rotationSpeed = 360f;  // Degrees per second
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the projectile collided with the player
        if (collision.gameObject.CompareTag("PlayerTag"))
        {
            // Ignore the collision or handle it differently
            return;
        }

        // Handle collisions with other objects (e.g., enemies, walls, etc.)
        Destroy(gameObject); // Destroy the projectile when it hits something
    }
        void Update()
    {
        // Rotate the projectile around its Z-axis
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
}
