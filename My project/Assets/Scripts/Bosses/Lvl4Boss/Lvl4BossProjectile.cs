using UnityEngine;

public class Lvl4BossProjectile : MonoBehaviour
{
    public float speed = 10f;
    public float inaccuracy = 10f;
    public float lifetime = 1f;
    private bool hasRicocheted = false;
    private Vector2 direction;
    private float lifetimeTimer;
    private float difficulty;
    private float reverseDifficulty;

    void Start()
    {
        // Initialize the lifetime timer
        lifetimeTimer = lifetime;

        difficulty = PlayerPrefs.GetFloat("Difficulty");
        reverseDifficulty = PlayerPrefs.GetFloat("ReverseDifficulty");

        // Set the initial direction toward the player with some inaccuracy
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            Vector2 playerDirection = (player.transform.position - transform.position).normalized;
            direction = Quaternion.Euler(0, 0, Random.Range(-inaccuracy, inaccuracy)) * playerDirection;
        }
        else
        {
            direction = Vector2.right;
        }
    }

    void Update()
    {
        // Move the projectile in the current direction
        transform.Translate(direction * (Mathf.Pow(difficulty, 0.5f) * speed) * Time.deltaTime, Space.World);

        // Decrease the lifetime timer
        lifetimeTimer -= Time.deltaTime;

        // Destroy the projectile if the lifetime expires
        if (lifetimeTimer <= 0f)
        {
            DestroyProjectile();
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("ProjObstacle"))
        {
            if (!hasRicocheted)
            {
                hasRicocheted = true;

                // Calculate the ricochet direction
                Vector2 collisionNormal = collider.GetComponent<Collider2D>().ClosestPoint(transform.position) - (Vector2)transform.position;
                collisionNormal = collisionNormal.normalized;
                direction = Vector2.Reflect(direction, collisionNormal);

                // Apply some inaccuracy to the ricochet direction
                direction = Quaternion.Euler(0, 0, Random.Range(-inaccuracy, inaccuracy)) * direction;

                // Clone the projectile, but only if there are fewer than 20 projectiles
                if (CountActiveProjectiles() < 20)
                {
                    GameObject clone = Instantiate(gameObject, transform.position, Quaternion.identity);
                    Lvl4BossProjectile cloneScript = clone.GetComponent<Lvl4BossProjectile>();
                    if (cloneScript != null)
                    {
                        cloneScript.hasRicocheted = true; // Prevent the clone from ricocheting again
                        cloneScript.direction = Quaternion.Euler(0, 0, Random.Range(-inaccuracy, inaccuracy)) * direction;
                    }
                }
            }
            else
            {
                DestroyProjectile();
            }
        }
    }

    private void DestroyProjectile()
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

    private int CountActiveProjectiles()
    {
        return FindObjectsOfType<Lvl4BossProjectile>().Length;
    }
}
