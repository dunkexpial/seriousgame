using UnityEngine;

public class Lvl4BossProjectile : MonoBehaviour
{
    public float speed = 10f; // Speed of the projectile
    public float inaccuracy = 10f; // Angle of inaccuracy in degrees
    private bool hasRicocheted = false; // To ensure the ricochet happens only once
    private Vector2 direction; // Current movement direction of the projectile

    void Start()
    {
        // Set the initial direction toward the player with some inaccuracy
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            Vector2 playerDirection = (player.transform.position - transform.position).normalized;
            direction = Quaternion.Euler(0, 0, Random.Range(-inaccuracy, inaccuracy)) * playerDirection;
        }
        else
        {
            // Default to east if no player is found
            direction = Vector2.right;
        }
    }

    void Update()
    {
        // Move the projectile in the current direction
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    // Trigger-based collision with obstacles
    private void OnTriggerEnter2D(Collider2D collider)
    {

        if (collider.CompareTag("ProjObstacle"))
        {
            if (!hasRicocheted)
            {
                hasRicocheted = true;

                // Calculate a new inaccurate direction toward the player after ricochet
                GameObject player = GameObject.FindWithTag("Player");
                if (player != null)
                {
                    Vector2 playerDirection = (player.transform.position - transform.position).normalized;
                    direction = Quaternion.Euler(0, 0, Random.Range(-inaccuracy, inaccuracy)) * playerDirection;

                    // Clone the projectile, but only if there are fewer than 20 projectiles
                    if (CountActiveProjectiles() < 20)
                    {
                        GameObject clone = Instantiate(gameObject, transform.position, Quaternion.identity);
                        Lvl4BossProjectile cloneScript = clone.GetComponent<Lvl4BossProjectile>();
                        if (cloneScript != null)
                        {
                            cloneScript.hasRicocheted = true; // Prevent the clone from ricocheting again
                            cloneScript.direction = Quaternion.Euler(0, 0, Random.Range(-inaccuracy, inaccuracy)) * playerDirection;
                        }
                    }
                }
            }
            else
            {
                // If the projectile has already ricocheted, destroy it but preserve the "Particle" child
                Transform particleChild = transform.Find("Particle");
                if (particleChild != null)
                {
                    // Detach and preserve the "Particle" child before destroying the projectile
                    particleChild.SetParent(null);
                    Destroy(particleChild.gameObject, 2f); // Destroy the particle after 2 seconds
                }

                // Destroy the projectile itself
                Destroy(gameObject);
            }
        }
    }

    // Helper method to count how many projectiles are active
    private int CountActiveProjectiles()
    {
        return FindObjectsOfType<Lvl4BossProjectile>().Length;
    }
}
