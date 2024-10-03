using UnityEngine;

public abstract class BaseProjectile : MonoBehaviour
{
    public float speed;
    public Vector2 direction;
    public float lifetime = 5f;
    public int damageAmount = 0;
    private float timer;
    private Collider2D shooterCollider;

    protected virtual void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = direction * speed;
        }
        
        timer = lifetime;
    }

    protected virtual void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Destroy(gameObject);
        }
    }

    //Inicializa o projétil com o atirador
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
        // Sistema de dano e destruir ao colidir
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