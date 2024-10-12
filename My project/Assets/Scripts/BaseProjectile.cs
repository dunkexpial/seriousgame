using UnityEngine;

public abstract class BaseProjectile : MonoBehaviour
{
    public float speed;
    public Vector2 direction;
    public float lifetime = 5f;
    public int damageAmount = 0;
    private float timer;
    private Collider2D shooterCollider;
    public float spinSpeed = 360f;

    protected virtual void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = direction * speed;
        }

        SetInitialRotation(direction);
        
        timer = lifetime;
    }

    protected virtual void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Destroy(gameObject);
        }

        // Rotate the projectile based on spin speed
        transform.Rotate(Vector3.forward, spinSpeed * Time.deltaTime);
    }

    // Sets the initial rotation of the projectile based on the shooting direction, this MF took me more than an  hour of googling to figure out

    private void SetInitialRotation(Vector2 shootingDirection)
    {
        float angle = Mathf.Atan2(shootingDirection.y, shootingDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    // Initializes the projectile with the shooter
    public void Initialize(GameObject shooter)
    {
        shooterCollider = shooter.GetComponent<Collider2D>();
        if (shooterCollider != null)
        {
            Collider2D projectileCollider = GetComponent<Collider2D>();
            if (projectileCollider != null)
            {
                Physics2D.IgnoreCollision(projectileCollider, shooterCollider);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Damage system and destroy on collision
        AttributesManager targetAttributes = collision.gameObject.GetComponent<AttributesManager>();
        if (targetAttributes != null)
        {
            targetAttributes.TakeDamage(damageAmount);
            Destroy(gameObject);
        }
        // Check if it hits any object with a certain tag (i'm yet to decide for certain)
        if (collision.gameObject.CompareTag("ProjObstacle"))
        {
            Destroy(gameObject);
            return; // no need to check for collision with enemies, it's already destroyed
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        AttributesManager targetAttributes = collision.gameObject.GetComponent<AttributesManager>();
        if (targetAttributes != null)
        {
            targetAttributes.TakeDamage(damageAmount);
            Destroy(gameObject);
        }
        // Check if it hits any object with a certain tag (i'm yet to decide for certain)
        if (collision.gameObject.CompareTag("ProjObstacle"))
        {
            Destroy(gameObject);
            return; // no need to check for collision with enemies, it's already destroyed
        }
    }
}
