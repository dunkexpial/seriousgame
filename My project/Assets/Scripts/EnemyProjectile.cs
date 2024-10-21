using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public GameObject player;
    private Rigidbody2D rb;
    public float speed;
    public float lifespan = 5f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");

        // Calculate the direction from the projectile to the player
        Vector3 direction = (player.transform.position - transform.position).normalized;

        // Set the velocity of the projectile towards the player
        rb.velocity = direction * speed;

        // Calculate the rotation to face the direction of movement
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        Destroy(gameObject, lifespan);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("ProjObstacle"))
        {
            Destroy(gameObject);
        }
    }
}
