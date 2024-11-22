using UnityEngine;

public class MagneticProjectile : BaseProjectile
{
    public float followDistance = 5f; // The distance at which the projectile starts following the enemy
    private GameObject nearestEnemy; // Reference to the nearest enemy
    public LayerMask obstacleLayer; // The layer used for obstacles
    public float attractionStrength = 5f; // How fast the projectile adjusts its direction (smoothness)
    private Vector2 targetDirection; // The direction the projectile is trying to move towards

    protected override void Start()
    {
        damageAmount = 3; // Set the damage specific to this type of projectile
        speed = 300f;
        spinSpeed = 1000f;
        base.Start();

        // Initially set the target direction to the initial direction
        targetDirection = direction;
    }

    protected override void Update()
    {
        base.Update();

        // Find all objects with the "Enemy" tag in the vicinity
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        // Find the nearest enemy within the followDistance
        nearestEnemy = null;
        float closestDistance = followDistance;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < closestDistance)
            {
                closestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        // If a nearby enemy was found and has line of sight, move the projectile towards it
        if (nearestEnemy != null)
        {
            Vector2 directionToEnemy = (nearestEnemy.transform.position - transform.position).normalized;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToEnemy, followDistance, obstacleLayer);

            if (hit.collider == null)
            {
                targetDirection = Vector2.Lerp(targetDirection, directionToEnemy, Time.deltaTime * attractionStrength);

                // Update direction in the base class
                this.direction = targetDirection;

                // Update the velocity based on the smoothed direction
                Rigidbody2D rb = GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.velocity = targetDirection * speed;
                }
            }
        }
    }
}
