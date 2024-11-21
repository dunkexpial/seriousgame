using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxProjectile : MonoBehaviour
{
    public GameObject player;
    private Rigidbody2D rb;
    public float speed;
    public float lifespan = 5f;
    private int obstacleHitCount = 0; // Counter for obstacle hits

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");

        Vector3 direction = (player.transform.position - transform.position).normalized;

        rb.velocity = direction * speed;

        Destroy(gameObject, lifespan);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("ProjObstacle"))
        {
            obstacleHitCount++;

            if (obstacleHitCount <= 2)
            {
                // Ricochet: Redirect projectile toward the player
                RicochetTowardsPlayer();
            }
            else if (obstacleHitCount > 2)
            {
                // Destroy on third hit
                DetachParticles();
                Destroy(gameObject);
            }
        }
    }

    private void RicochetTowardsPlayer()
    {
        if (player == null) return; // Safety check if player is null

        Vector3 newDirection = (player.transform.position - transform.position).normalized;

        rb.velocity = newDirection * speed;
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
