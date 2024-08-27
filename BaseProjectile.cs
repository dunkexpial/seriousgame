using UnityEngine;

public abstract class BaseProjectile : MonoBehaviour
{
    public float speed;
    public Vector2 direction;
    public float lifetime = 5f;

    private float timer;

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


    // COLISÕES AQUI
    // initial commit by Luís
}
