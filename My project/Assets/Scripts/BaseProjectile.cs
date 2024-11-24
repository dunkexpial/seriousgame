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
    private SoundManager soundManager;  // Ensure this is not null

    protected virtual void Start()
    {
        // Initialize Rigidbody2D
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = direction * speed;
        }

        // Set initial rotation
        SetInitialRotation(direction);

        // Initialize the timer
        timer = lifetime;

        // Initialize SoundManager (look for SoundManager in parent objects or assign directly)
        soundManager = FindObjectOfType<SoundManager>(); // Assuming it's somewhere in the scene
        if (soundManager == null)
        {
            Debug.LogWarning("SoundManager not found in scene.");
        }
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

    private void SetInitialRotation(Vector2 shootingDirection)
    {
        float angle = Mathf.Atan2(shootingDirection.y, shootingDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

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
        AttributesManager targetAttributes = collision.gameObject.GetComponent<AttributesManager>();
        if (targetAttributes != null)
        {
            targetAttributes.TakeDamage(damageAmount);
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("ProjObstacle"))
        {
            Destroy(gameObject);
            return;
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

        if (collision.gameObject.CompareTag("ProjObstacle"))
        {
            if (soundManager != null)
            {
                soundManager.PlaySoundBasedOnCollision("PlayerProjParede");
            }
            Destroy(gameObject);
            return;
        }
    }
}
